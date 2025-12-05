using System.Collections.Generic;

namespace RPG.Stats
{
    /// <summary>
    /// Interface for components that provide stat modifiers.
    /// </summary>
    public interface IModifierProvder
    {
        /// <summary>
        /// Returns additive modifiers for the specified stat.
        /// </summary>
        IEnumerable<float> GetAdditiveModifiers(Stat stat);

        /// <summary>
        /// Returns percentage modifiers for the specified stat.
        /// </summary>
        IEnumerable<float> GetPercentageModifiers(Stat stat);
    }
}