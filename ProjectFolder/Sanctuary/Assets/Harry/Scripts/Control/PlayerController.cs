using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sanctuary.Harry.Movement;
using Sanctuary.Harry.Combat;
using Sanctuary.Harry.Core;

namespace Sanctuary.Harry.Control
{
    public class PlayerController : MonoBehaviour
    {
        Health health;

        private void Start()
        {
            health = GetComponent<Health>();
        }

        private void Update()
        {
            if (health.IsDead()) return;
            if (HandleCombat()) return;
            if (HandleMovement()) return;
        }

        private bool HandleCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach (RaycastHit hit in hits)
            {
                FightTarget target = hit.transform.GetComponent<FightTarget>();
                if (target == null) continue;

                if (!GetComponent<Fight>().CanAtk(target.gameObject)) { continue; }

                if (Input.GetMouseButtonDown(1)) { GetComponent<Fight>().Attack(target.gameObject); }
                
                return true;
            }
            return false;
        }

        private bool HandleMovement()
        {
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (hasHit) 
            {
                if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
                {
                    GetComponent<Move>().StartMoveAction(hit.point, 1f); 
                }
                return true;
            }
            return false;
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }

    }
}
