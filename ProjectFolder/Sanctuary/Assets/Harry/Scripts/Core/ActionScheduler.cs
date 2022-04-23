using System.Collections;
using UnityEngine;

namespace Sanctuary.Harry.Core
{
    public class ActionScheduler : MonoBehaviour
    {
        IAction currentAction = null, previousAction=null;

        public void StartAction(IAction action)
        {
            if(currentAction == action) { return; }

            if(currentAction != null)
            {
                currentAction.Cancel();
            }
            currentAction = action;
        }

        public void CancelCurrentAction()
        {
            previousAction = currentAction;
            StartAction(null);
        }

        public void ResumePreviousAction()
        {
            StartAction(previousAction);
            previousAction = null;
        }
    }
}