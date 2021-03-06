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
using GameDevTV.Inventories;

namespace Sanctuary.Harry.Control
{
    public class PlayerController : MonoBehaviour
    {
        Health health;
        ActionStore actionStore;

        [System.Serializable] struct CursorMapping { public CursorType type; public Texture2D texture; public Vector2 hotspot; }

        [SerializeField] CursorMapping[] cursorMappings = null;
        [SerializeField] float maxNavMeshProjDist = 1f, raycastRadius =1f;
        [SerializeField] ParticleSystem clickFeedback;
        [SerializeField] int numberOfAbilities = 6;

        bool isDraggingUI = false;

        private void Awake()
        {
            health = GetComponent<Health>();
            actionStore = GetComponent<ActionStore>();
        }

        private void Update()
        {
            if (InteractWithUI()) return;

            if (health.IsDead())
            {
                SetCursor(CursorType.None);
                return;
            }

            UseAbilities();

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
            RaycastHit[] hits = Physics.SphereCastAll(GetMouseRay(), raycastRadius);
            
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
            if (Input.GetMouseButtonUp(0)) { isDraggingUI = false; }

            if(EventSystem.current.IsPointerOverGameObject())
            {
                if (Input.GetMouseButtonDown(0))
                {
                    isDraggingUI = true;
                }
                SetCursor(CursorType.UI);
                return true;
            }

            if (isDraggingUI)
            {
                return true;
            }
            return false;
        }

        private void UseAbilities()
        {
            for (int i = 0; i < numberOfAbilities; i++)
            {
                if(Input.GetKeyDown(KeyCode.Alpha1 + i))
                {
                    actionStore.Use(i, gameObject);
                }
            }
        }

        private bool HandleMovement()
        {
            Vector3 target;
            bool hasHit = RaycastNavMesh(out target);
            if (hasHit) 
            {
                if (!GetComponent<MovementController>().CanMoveTo(target)) return false;

                if (Input.GetMouseButton(0))
                {
                    GetComponent<MovementController>().StartMoveAction(target, 1f);
                }
                if (Input.GetMouseButtonDown(0)) { Instantiate(clickFeedback, target, Quaternion.identity); }
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
            

            return true;
        }

        

        public static Ray GetMouseRay()
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
