using UnityEngine;

namespace Assets.Scripts.AI.Utilities
{
    /// <summary>
    /// Class that has a method that randomly generates a number from within given range using Unity Random implementation.
    /// </summary>
    public static class RandomNumberGenerator
    {
        /// <summary>
        /// Randomly generates and returns number from withing given range.
        /// </summary>
        /// <param name="minimum">The minimum value (inclusive) used in number generation.</param>
        /// <param name="maximum">The maximum value (inclusive) used in number generation.</param>
        /// <returns>Randomly generated floating point number.</returns>
        public static float GenerateRandomNumberWithinRange(float minimum, float maximum)
        {
            return Random.Range(minimum, maximum);
        }

        /// <summary>
        /// Randomly generates and returns number from withing given range.
        /// </summary>
        /// <param name="minimum">The minimum value (inclusive) used in number generation.</param>
        /// <param name="maximum">The maximum value (inclusive) used in number generation.</param>
        /// <returns>Randomly generated floating point number.</returns>
        public static int GenerateRandomNumberWithinRange(int minimum, int maximum)
        {
            return Random.Range(minimum, maximum);
        }
    }
}
