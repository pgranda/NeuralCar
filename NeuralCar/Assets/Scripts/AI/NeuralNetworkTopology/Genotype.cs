using Assets.Scripts.AI.Utilities;
using System.Collections.Generic;

namespace Assets.Scripts.AI.NeuralNetworkTopology
{
    /// <summary>
    /// Single representative of given Generation population.
    /// Contains information about travelled distance,
    /// overall fitness of this Genotype across whole population
    /// and List of Genes (Neural Network weights).
    /// </summary>
    public class Genotype
    {
        public List<float> Genes { get; private set; }

        public float Distance { get; set; }

        public float Fitness { get; set; }
        
        /// <summary>
        /// Constructor of Genotype class which takes list of floats as its parameter
        /// </summary>
        /// <param name="genes">List of floats (weights of neural network).</param>
        public Genotype(List<float> genes)
        {
            Genes = genes;
            Fitness = 0;
        }

        /// <summary>
        /// Constructor of Genotype class which takes integer as its parameter
        /// </summary>
        /// <param name="genesAmount">Amount of weights that this neural network should use.</param>
        public Genotype(int genesAmount)
        {
            Genes = new List<float>();
            InitialiseGenesWithRandomValues(genesAmount);
            Fitness = 0;
        }
        /// <summary>
        /// Method that resets Distance and Fitness variables setting them both to 0
        /// </summary>
        public void Reset()
        {
            Distance = 0;
            Fitness = 0;
        }

        /// <summary>
        /// Method that initialises Genes with random values.
        /// </summary>
        /// <param name="genesAmount">The amount of genes to initalise with random numbers.</param>
        private void InitialiseGenesWithRandomValues(int genesAmount)
        {
            for (int i = 0; i < genesAmount; i++)
            {
                Genes.Add(RandomNumberGenerator.GenerateRandomNumberWithinRange(Constants.MinGeneValue, Constants.MaxGeneValue));
            }
        }
    }
}