using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.AI.Utilities;

namespace Assets.Scripts.AI.NeuralNetworkTopology
{
    /// <summary>
    /// Class representing a single layer of artificial neural network
    /// </summary>
    public class NeuralLayer
    {
        public List<Neuron> Neurons { get; private set; }

        public Neuron BiasNode { get; private set; }

        public int NumberOfNeurons
        {
            get { return Neurons.Count; }
        }

        public int NumberOfNeuronsInNextLayer
        {
            get
            {
                if (Neurons == null || !Neurons.Any())
                {
                    return 0;
                }

                return Neurons.First().Weights.Count;
            }
        }

        /// <summary>
        /// Constructor of Neural Layer class. It calls method that initialises the layer with given number of neurons
        /// </summary>
        /// <param name="numberOfNeurons">The number of neurons in this particular neural network layer</param>
        /// <param name="numberOfNeuronsInNextLayer">The number of neurons in neural network layer that this layer is connected to</param>
        /// <param name="genes">The list of genes (weights) that neurons from this layer are connected to the neurons of the next neural layer</param>
        public NeuralLayer(int numberOfNeurons, int numberOfNeuronsInNextLayer, List<float> genes)
        {
            InitialiseNeurons(genes, numberOfNeurons, numberOfNeuronsInNextLayer);
        }

        /// <summary>
        /// Method that initialises neurons of this particular neural network layer
        /// </summary>
        /// <param name="genes">The list of genes (weights) that neurons from this layer are connected to the neurons of the next neural layer</param>
        /// <param name="numberOfNeurons">The number of neurons in this particular neural network layer</param>
        /// <param name="numberOfNeuronsInNextLayer">The number of neurons in neural network layer that this layer is connected to</param>
        private void InitialiseNeurons(List<float> genes, int numberOfNeurons, int numberOfNeuronsInNextLayer)
        {
            Neurons = new List<Neuron>();

            int k = 0;
            for (int i = 0; i <= numberOfNeurons; i++)
            {
                var weights = new List<float>();
                for (int j = 0; j < numberOfNeuronsInNextLayer; j++)
                {
                    weights.Add(genes[k]);
                    k++;
                }

                if (i == numberOfNeurons)
                {
                    BiasNode = new Neuron(weights);
                }
                else
                {
                    Neurons.Add(new Neuron(weights));
                }
            }
        }

        /// <summary>
        /// Method that calculates result (output values) from given input for this particular neural layer
        /// This is the place where bias node and activation function is applied to the neurons
        /// </summary>
        /// <param name="inputs">List of floats that serves as inputs for this neural layer
        /// (note that distances from rays are serving as inputs only for first [Input layer], hidden layers inputs are
        /// values already modified by previous layer processing method)</param>
        /// <returns>The list of calculated outputs for this particular layer</returns>
        public List<float> ProcessInputs(List<float> inputs)
        {
            var result = new List<float>();

            for (int i = 0; i < NumberOfNeuronsInNextLayer; i++)
            {
                result.Add(0.0f);
                for (int j = 0; j < NumberOfNeurons; j++)
                {
                    result[i] += Neurons.ElementAt(j).Weights.ElementAt(i) * inputs.ElementAt(j);
                }

                result[i] += BiasNode.Weights.ElementAt(i);
                result[i] = ActivationFunction.SoftSignFunction(result[i]);
            }

            return result;
        }
    }
}
