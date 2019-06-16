using System.Collections.Generic;

namespace Assets.Scripts.AI.NeuralNetworkTopology
{
    /// <summary>
    /// Class representing single neuron in neural network - it just contains the weights
    /// </summary>
    public class Neuron
    {
        public List<float> Weights { get; private set; }

        /// <summary>
        /// Constructor of neuron assigning passed List of floats which are weights
        /// </summary>
        /// <param name="weights">List of floats serving as weights</param>
        public Neuron(List<float> weights)
        {
            Weights = weights;
        }
    }
}