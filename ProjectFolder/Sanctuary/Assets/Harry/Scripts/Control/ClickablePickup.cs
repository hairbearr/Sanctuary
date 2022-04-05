using GameDevTV.Inventories;
using Sanctuary.Harry.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sanctuary.Harry.Control
{
    [RequireComponent(typeof(Pickup))]
    public class ClickablePickup : MonoBehaviour, IRaycastable
    {
        Pickup pickup;

        private void Awake()
        {
            pickup = GetComponent<Pickup>();   
        }

        public CursorType GetCursorType()
        {
            if (pickup.CanBePickedUp()) { return CursorType.Pickup; }
            else { return CursorType.FullPickup; }
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            if (Input.GetMouseButtonDown(0))
            {
                callingController.GetComponent<MovementController>().StartMoveAction(transform.position, 1f);
            }
            return true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                other.GetComponent<MovementController>().Cancel();
                pickup.PickupItem();
            }
        }
    }
}
