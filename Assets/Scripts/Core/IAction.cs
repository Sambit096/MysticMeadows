namespace RPG.Core
{
    /// <summary>
    /// Interface for actions that can be scheduled and cancelled.
    /// </summary>
    public interface IAction
    {
        /// <summary>
        /// Cancels this action.
        /// </summary>
        void Cancel();
    }
}
