using Sanctuary.Harry.Core;
using Sanctuary.Harry.Movement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Sanctuary.Harry.Dialogue
{
    public class PlayerConversant : MonoBehaviour, IAction
    {
        [SerializeField] string playerName;

        Dialogue currentDialogue;
        DialogueNode currentNode = null;
        AIConversant currentConversant = null, targetConversant;
        bool isChoosing = false;
        


        public event Action onConversationUpdated;


        public void StartDialogueAction(AIConversant newConversant, Dialogue newDialogue)
        {
            //Debug.Log($"StartDialogueAction {newConversant}/{newDialogue}");
            if (currentConversant != null && currentConversant == newConversant) return;
            if (currentDialogue != null) Quit();
            if(newConversant == null) { return; }

            GetComponent<ActionScheduler>().StartAction(this);

            
            currentDialogue = newDialogue;
            targetConversant = newConversant;
            
        }

        private void Update()
        {
           if (targetConversant)
            {
                if(Vector3.Distance(targetConversant.transform.position, transform.position) > 3)
                {
                    GetComponent<MovementController>().MoveTo(targetConversant.transform.position, 1); ;
                }
                else
                {
                   GetComponent<MovementController>().Cancel();
                    StartDialogue();
                }
            }    
        }

        private void StartDialogue()
        {
            currentConversant = targetConversant;
            targetConversant = null;
            currentNode = currentDialogue.GetRootNode();
            onConversationUpdated?.Invoke();
            
        }

        public void Quit()
        {
            currentDialogue = null;
            TriggerExitAction();
            currentConversant = null;
            currentNode = null;
            isChoosing = false;
            onConversationUpdated();
        }

        public bool IsActive()
        {
            return currentDialogue != null;
        }

        public string GetText()
        {
            if(currentNode == null) return ""; 

            return currentNode.GetText();
        }

        public bool IsChoosing()
        {
            return isChoosing;
        }

        public IEnumerable<DialogueNode> GetChoices()
        {
            return FilterOnCondition(currentDialogue.GetPlayerChildren(currentNode));
        }

        public void SelectChoice(DialogueNode chosenNode)
        {
            currentNode = chosenNode;

            TriggerEnterAction();
            isChoosing = false;
            Next();
        }

        public void Next()
        {
            int numPlayerResponses = FilterOnCondition(currentDialogue.GetPlayerChildren(currentNode)).Count();
            if (numPlayerResponses > 0)
            {
                isChoosing = true;
                TriggerExitAction();
                onConversationUpdated();
                return;
            }

            DialogueNode[] children = FilterOnCondition(currentDialogue.GetAIChildren(currentNode)).ToArray();
            int randomIndex = UnityEngine.Random.Range(0, children.Count());
            TriggerExitAction();
            currentNode = children[randomIndex];
            TriggerEnterAction();
            onConversationUpdated();
        }

        public bool HasNext()
        {
            return FilterOnCondition(currentDialogue.GetAllChildren(currentNode)).Count() > 0;
        }

        public string GetCurrentConversantName()
        {
            if (isChoosing)
            {
                return playerName;
            }
            else
            {
                return currentConversant.GetName();
            }
        }

        private IEnumerable<DialogueNode> FilterOnCondition(IEnumerable<DialogueNode> inputNode)
        {
            foreach (var node in inputNode)
            {
                if (node.CheckConditon(GetEvaluators())) { yield return node; }
            }
        }


        private IEnumerable<IPredicateEvaluator> GetEvaluators()
        {
            return GetComponents<IPredicateEvaluator>();
        }

        private void TriggerEnterAction()
        {
            if(currentNode!=null)
            {
                TriggerAction(currentNode.GetOnEnterAction());
            }
        }

        

        private void TriggerExitAction()
        {
            if (currentNode != null)
            {
                TriggerAction(currentNode.GetOnExitAction());
            }
        }

        private void TriggerAction(string action)
        {
            if (action == "") return;

            foreach (DialogueTrigger trigger in currentConversant.GetComponents<DialogueTrigger>())
            {
                trigger.Trigger(action);
            }
        }

        public void Cancel()
        {
            Quit();
        }
    }
}
