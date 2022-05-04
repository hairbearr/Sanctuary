using Sanctuary.Harry.Attributes;
using Sanctuary.Harry.Control;
using Sanctuary.Harry.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sanctuary.Harry.Dialogue
{
    public class AIConversant : MonoBehaviour, IRaycastable
    {
        [SerializeField] string conversantName;
        [SerializeField] Dialogue dialogue = null;

        public CursorType GetCursorType()
        {
            return CursorType.Dialogue;
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            if (dialogue == null)
            {
                return false;
            }
            
            Health health = GetComponent<Health>();
            if (health && health.IsDead()) return false;

            if (Input.GetMouseButtonDown(0))
            {
                callingController.GetComponent<PlayerConversant>().StartDialogueAction(this, dialogue);
            }

            return true;
        }

        public string GetName()
        {
            return conversantName;
        }

    }
}
