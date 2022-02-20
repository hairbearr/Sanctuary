using Sanctuary.Harry.Control;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sanctuary.Harry.Combat
{
    public class WeaponPickUp : MonoBehaviour, IRaycastable
    {
        [SerializeField] Weapon weapon = null;
        [SerializeField] float respawnTime = 5f;

        private void OnTriggerEnter(Collider other)
        {

            if (other.gameObject.tag == "Player")
            {
                Pickup(other.GetComponent<Fight>());
            }
        }

        private void Pickup(Fight fighter)
        {
            fighter.EquipWeapon(weapon);
            StartCoroutine(HideForSeconds(respawnTime));
        }

        private IEnumerator HideForSeconds(float seconds)
        {
            ShowPickup(false);
            yield return new WaitForSeconds(seconds);
            ShowPickup(true);
        }

        private void ShowPickup(bool shouldShow)
        {
            GetComponent<SphereCollider>().enabled = shouldShow;
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(shouldShow);
            }
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Pickup(callingController.GetComponent<Fight>());
            }
            return true;
        }

        public CursorType GetCursorType()
        {
            return CursorType.Pickup;
        }
    }
}
