using System;

namespace Assets.Scripts.AI.Utilities
{
    /// <summary>
    /// Class that contains Activation Function used in Neural Network
    /// </summary>
    public static class ActivationFunction
    {
        /// <summary>
        /// Method that "squishes" the values.
        /// Firstly Hyperbolic (TanH) function was chosen, but then SoftSign function was found
        /// as better replacement, it increased the performance of neural network learning drastically
        /// </summary>
        /// <param name="x">Floating point value passed to the function</param>
        /// <returns>Value that has been squished using SoftSign function</returns>
        public static float SoftSignFunction(float x)
        {
            return x / (1 + Math.Abs(x));
        }
    }
}