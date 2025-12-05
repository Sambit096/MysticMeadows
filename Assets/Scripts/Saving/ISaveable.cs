namespace RPG.Saving
{
    /// <summary>
    /// Interface for components that can save and restore their state.
    /// </summary>
    public interface ISaveable
    {
        /// <summary>
        /// Captures the current state of this component.
        /// </summary>
        object CaptureState();

        /// <summary>
        /// Restores this component to a previously captured state.
        /// </summary>
        void RestoreState(object state);
    }
}