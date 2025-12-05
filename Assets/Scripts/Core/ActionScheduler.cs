using UnityEngine;

namespace RPG.Core
{
    /// <summary>
    /// Manages action scheduling to ensure only one action runs at a time.
    /// </summary>
    public class ActionScheduler : MonoBehaviour
    {
        private IAction currentAction;

        /// <summary>
        /// Starts a new action, cancelling any currently running action.
        /// </summary>
        public void StartAction(IAction action)
        {
            if (currentAction == action) return;

            if (currentAction != null)
            {
                currentAction.Cancel();
                Debug.Log("Cancelling " + currentAction);
            }

            currentAction = action;
        }

        /// <summary>
        /// Cancels the current action.
        /// </summary>
        public void CancelCurrentAction()
        {
            StartAction(null);
        }
    }
}
