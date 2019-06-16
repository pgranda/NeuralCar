using Assets.Scripts.AI.NeuralNetworkTopology;
using System.Collections.Generic;

namespace Assets.Scripts.Car
{
    /// <summary>
    /// Class that contains both Cars genotype and neural network
    /// </summary>
    public class CarBrain
    {
        public Genotype Genotype { get; private set; }

        private NeuralNetwork neuralNetwork;

        /// <summary>
        /// Method that initialises car with given genotype and creates new neural network for the car
        /// </summary>
        /// <param name="genotype">Genotype that this car is going to use</param>
        /// <param name="numberOfNeuronsInEachLayer">List of amount of neurons that each layer of this car neural network is going to have</param>
        public CarBrain(Genotype genotype, List<int> numberOfNeuronsInEachLayer)
        {
            Genotype = genotype;
            neuralNetwork = new NeuralNetwork(numberOfNeuronsInEachLayer, genotype);
        }

        /// <summary>
        /// Method that calls ProcessInputs method on neural network
        /// </summary>
        /// <param name="rayOutput">List of floats that serve as neural network input neurons with calculated ray distances as input values</param>
        /// <returns>List of calculated output values that are going to control the car (2 output nodes [1 for steering, 1 for accelerating])</returns>
        public List<float> ProcessInputs(List<float> rayOutput)
        {
            return neuralNetwork.ProcessInputs(rayOutput);
        }
    }
}