using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sanctuary.Harry.Movement;
using Sanctuary.Harry.Combat;
using Sanctuary.Harry.Core;
using Sanctuary.Harry.Attributes;
using System;
using UnityEngine.EventSystems;
using UnityEngine.AI;

namespace Sanctuary.Harry.Control
{
    public class PlayerController : MonoBehaviour
    {
        Health health;

        [System.Serializable] struct CursorMapping { public CursorType type; public Texture2D texture; public Vector2 hotspot; }

        [SerializeField] CursorMapping[] cursorMappings = null;
        [SerializeField] float maxNavMeshProjDist = 1f, maxNavPathLength = 40f;

        private void Awake()
        {
            health = GetComponent<Health>();
        }

        private void Update()
        {
            if (InteractWithUI()) return;

            if (health.IsDead())
            {
                SetCursor(CursorType.None);
                return;
            }

            if (HandleComponent()) return;
            
            if (HandleMovement()) return;
            
            SetCursor(CursorType.None);
        }

        private bool HandleComponent()
        {
            RaycastHit[] hits = RaycastAllSorted();
            foreach (RaycastHit hit in hits)
            {
                IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();
                foreach (IRaycastable raycastable in raycastables)
                {
                    if (raycastable.HandleRaycast(this))
                    {
                        SetCursor(raycastable.GetCursorType());
                        return true;
                    }
                }
            }
            return false;
        }

        RaycastHit[] RaycastAllSorted()
        {
            //Get all hits
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            
            //Sort by distance
                //build array distances
            float[] distances = new float[hits.Length];
            for (int i = 0; i < hits.Length; i++)
            {
                distances[i] = hits[i].distance;
            }

            //sort hits
            Array.Sort(distances, hits);

            //Return
            return hits;
        }

        private bool InteractWithUI()
        {
            if(EventSystem.current.IsPointerOverGameObject())
            {
                SetCursor(CursorType.UI);
                return true;
            }
            return false;
        }

        private bool HandleMovement()
        {
            Vector3 target;
            bool hasHit = RaycastNavMesh(out target);
            if (hasHit) 
            {
                if (Input.GetMouseButton(0))
                {
                    GetComponent<Move>().StartMoveAction(target, 1f); 
                }
                SetCursor(CursorType.Movement);
                return true;
            }
            return false;
        }

        private bool RaycastNavMesh(out Vector3 target)
        {
            target = new Vector3();
            // Raycast will hit terrain
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (!hasHit) return false;

            // See if there is a navmesh point nearby
            NavMeshHit navMeshHit;
            bool hasCastToNavMesh = NavMesh.SamplePosition(hit.point, out navMeshHit, maxNavMeshProjDist, NavMesh.AllAreas);
            if (!hasCastToNavMesh) return false;

            // Return true if there's a navmesh point nearby
            target = navMeshHit.position;

            //decide whether or not you want to go that way based on the path
            NavMeshPath path = new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath(transform.position, target, NavMesh.AllAreas, path);
            if (!hasPath) { return false; }
            if (path.status != NavMeshPathStatus.PathComplete) return false;
            if (GetPathLength(path) > maxNavPathLength) return false;

            return true;
        }

        private float GetPathLength(NavMeshPath path)
        {
            float total = 0;
            if (path.corners.Length < 2) return total;
            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                total += Vector3.Distance(path.corners[i], path.corners[i + 1]);
            }

            return total;
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }

        private void SetCursor(CursorType type)
        {
            CursorMapping mapping = GetCursorMapping(type);
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
        }

        private CursorMapping GetCursorMapping(CursorType type)
        {
            foreach (CursorMapping mapping in cursorMappings)
            {
                if(mapping.type == type)
                {
                    return mapping;
                }
            }
            return cursorMappings[0];
        }

    }
}
