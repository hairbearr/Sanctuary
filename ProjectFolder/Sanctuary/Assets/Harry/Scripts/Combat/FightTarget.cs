using Sanctuary.Harry.Attributes;
using Sanctuary.Harry.Control;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sanctuary.Harry.Combat
{
    [RequireComponent(typeof(Health))]
    public class FightTarget : MonoBehaviour, IRaycastable
    {
        public CursorType GetCursorType()
        {
            return CursorType.Combat;
        }

        public bool HandleRaycast(PlayerController callingController)
        {
                if (!callingController.GetComponent<Fight>().CanAtk(gameObject)) { return false; }

                if (Input.GetMouseButtonDown(1)) { callingController.GetComponent<Fight>().Attack(gameObject); }

                return true;
            
           
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
