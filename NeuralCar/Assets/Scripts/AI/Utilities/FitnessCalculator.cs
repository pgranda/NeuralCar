using Assets.Scripts.AI.NeuralNetworkTopology;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.AI.Utilities
{
    /// <summary>
    /// Class containing method that calculates fitness for genotypes in given generation population
    /// </summary>
    public static class FitnessCalculator
    {
        /// <summary>
        /// Method that calculates fitness value for all genotypes in given generation population
        /// Each Genotype's distance is compared to the average distance travelled by this generation population
        /// </summary>
        /// <param name="generationPopulation">List of genotypes that form given generation population</param>
        public static void CalculateFitness(List<Genotype> generationPopulation)
        {
            float overallDistanceTravelled = generationPopulation.Sum(g => g.Distance);
            float averageDistanceTravelled = overallDistanceTravelled / generationPopulation.Count;

            foreach (var genotype in generationPopulation)
            {
                genotype.Fitness = genotype.Distance / averageDistanceTravelled;
            }
        }
    }
}
