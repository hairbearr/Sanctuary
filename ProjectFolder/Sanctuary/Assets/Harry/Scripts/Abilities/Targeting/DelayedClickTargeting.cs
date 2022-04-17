using UnityEngine;
using Sanctuary.Harry.Control;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Sanctuary.Harry.Abilities.Targeting
{
    [CreateAssetMenu(fileName = "Delayed Click Targeting", menuName = "Sanctuary/Abilities/Targeting/Delayed Click", order = 0)]
    public class DelayedClickTargeting : TargetingStrategy
    {
        [SerializeField] Texture2D cursorTexture;
        [SerializeField] Vector2 cursorHotspot;
        [SerializeField] float areaAffectRadius;
        [SerializeField] LayerMask layerMask;
        [SerializeField] Transform targetingPrefab;
        
        Transform targetingPrefabInstance = null;


        public override void StartTargeting(GameObject user, Action<IEnumerable<GameObject>> finished)
        {
            PlayerController playerController = user.GetComponent<PlayerController>();
            playerController.StartCoroutine(Targeting(user, playerController, finished));
        }
        private IEnumerator Targeting(GameObject user, PlayerController playerController, Action<IEnumerable<GameObject>> finished)
        {
            playerController.enabled = false;
            if(targetingPrefabInstance == null) { targetingPrefabInstance = Instantiate(targetingPrefab); }
            else { targetingPrefabInstance.gameObject.SetActive(true); }

            targetingPrefabInstance.localScale = new Vector3 (areaAffectRadius*2, 1, areaAffectRadius*2);

            while (true)
            {
                Cursor.SetCursor(cursorTexture, cursorHotspot, CursorMode.Auto);

                RaycastHit raycastHit;
                if (Physics.Raycast(PlayerController.GetMouseRay(), out raycastHit, 1000, layerMask))
                {
                    targetingPrefabInstance.position = raycastHit.point;

                    if(Input.GetMouseButtonDown(0))
                    {
                        // Use the whole mouse click, so you don't move immediately after you use the ability
                        yield return new WaitWhile(() => Input.GetMouseButton(0));
                        playerController.enabled = true;
                        targetingPrefabInstance.gameObject.SetActive(false);
                        finished(GetGameObjectsInRadius(raycastHit.point));
                        yield break;
                    }
                }
                yield return null;
            }
        }

        private IEnumerable<GameObject> GetGameObjectsInRadius(Vector3 point)
        {
            
            RaycastHit[] hits = Physics.SphereCastAll(point, areaAffectRadius, Vector3.up, 0);
            foreach (var hit in hits)
            {
                yield return hit.collider.gameObject;
            }
            
        }
    }
}