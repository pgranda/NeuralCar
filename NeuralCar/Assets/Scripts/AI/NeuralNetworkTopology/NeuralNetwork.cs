using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.AI.NeuralNetworkTopology
{
    /// <summary>
    /// Class representing artificial neural network constructed from NeuralLayers
    /// </summary>
    public class NeuralNetwork
    {
        public List<NeuralLayer> Layers { get; private set; }
        
        /// <summary>
        /// Constructor that calls method which initialises the neural network layers with given parameters
        /// </summary>
        /// <param name="numberOfNeuronsInEachLayer">List of numbers defining the amount of neurons in each neural layer</param>
        /// <param name="genotype">Genotype that this neural network is going to utilize</param>
        public NeuralNetwork(List<int> numberOfNeuronsInEachLayer, Genotype genotype)
        {
            InitialiseNeuralLayers(numberOfNeuronsInEachLayer, genotype);
        }

        /// <summary>
        /// Method that initialises neural layers
        /// </summary>
        /// <param name="numberOfNeuronsInEachLayer">List of numbers defining the amount of neurons in each neural layer</param>
        /// <param name="genotype">Genotype that this neural network is going to utilize</param>
        private void InitialiseNeuralLayers(List<int> numberOfNeuronsInEachLayer, Genotype genotype)
        {
            Layers = new List<NeuralLayer>();
            int k = 0;
            for (int i = 0; i < numberOfNeuronsInEachLayer.Count - 1; i++)
            {
                var numberOfNeuronsInCurrentLayer = numberOfNeuronsInEachLayer.ElementAt(i);
                var numberOfNeuronsInNextLayer = numberOfNeuronsInEachLayer.ElementAt(i + 1);
                int genesAmount = (numberOfNeuronsInCurrentLayer + 1) * numberOfNeuronsInNextLayer;
                Layers.Add(new NeuralLayer(numberOfNeuronsInCurrentLayer, numberOfNeuronsInNextLayer , genotype.Genes.GetRange(k, genesAmount)));
                k += genesAmount;
            }
        }
        
        /// <summary>
        /// Method that iterates through all of the layers in neural network and calls ProcessInputs on them in order to produce outputs
        /// </summary>
        /// <param name="rayOutput">Initial inputs - distances calculated from ray casting</param>
        /// <returns>List of values that are calculated output nodes values</returns>
        public List<float> ProcessInputs(List<float> rayOutput)
        {
            var outputs = new List<float>();
            var inputs = rayOutput;

            foreach (var layer in Layers)
            {
                outputs = layer.ProcessInputs(inputs);
                inputs = outputs;
            }

            return outputs;
        }
    }
}