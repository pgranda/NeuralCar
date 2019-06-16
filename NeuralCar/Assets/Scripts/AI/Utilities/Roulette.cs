using Assets.Scripts.AI.NeuralNetworkTopology;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.AI.Utilities
{
    /// <summary>
    /// Class responsible for selecting parents during selection phase of genetic algorithm using roulette method
    /// </summary>
    public static class Roulette
    {
        /// <summary>
        /// Method that selects given amount of parents from population
        /// </summary>
        /// <param name="generationPopulation">List of genotypes to choose parents from</param>
        /// <param name="amountOfParentsToSelect">Amount of parents to select</param>
        /// <returns>List of genotypes which are selected parents</returns>
        public static List<Genotype> SelectParents(List<Genotype> generationPopulation, int amountOfParentsToSelect)
        {
            var rouletteGenotypePlaceholders = PrepareRouletteGenotypePlaceholders(generationPopulation);

            var parents = new List<Genotype>();

            for (int i = 0; i < amountOfParentsToSelect; i++)
            {
                parents.Add(SelectParent(rouletteGenotypePlaceholders));
            }

            return parents;
        }

        /// <summary>
        /// Method that creates assignments to the roulette wheel for each member of population
        /// </summary>
        /// <param name="generationPopulation">List of genotypes that are going to be assigned on roulette wheel</param>
        /// <returns>List of assignments on roulette wheel</returns>
        private static List<RouletteGenotypePlaceholder> PrepareRouletteGenotypePlaceholders(List<Genotype> generationPopulation)
        {
            var rouletteGenotypePlaceholders = new List<RouletteGenotypePlaceholder>();
            float fitnessSum = generationPopulation.Sum(gp => gp.Fitness);
            float tmp = 0;
            foreach (var member in generationPopulation)
            {
                float maxValue = tmp + member.Fitness / fitnessSum;
                rouletteGenotypePlaceholders.Add(new RouletteGenotypePlaceholder(member, tmp, maxValue));
                tmp = maxValue;
            }

            return rouletteGenotypePlaceholders;
        }

        /// <summary>
        /// Method that randomly selects parent from roulette wheel
        /// </summary>
        /// <param name="rouletteGenotypePlaceholders">List of assignments on roulette wheel</param>
        /// <returns>Genotype as the parent selected during roulette wheel spin</returns>
        private static Genotype SelectParent(List<RouletteGenotypePlaceholder> rouletteGenotypePlaceholders)
        {
            float randomNumber = RandomNumberGenerator.GenerateRandomNumberWithinRange(0.0f, 1.0f);
            var chosenParent = rouletteGenotypePlaceholders.First(m => randomNumber >= m.MinValue && randomNumber <= m.MaxValue).Genotype;

            return chosenParent;
        }
    }

    /// <summary>
    /// Class responsible for storing data about genotype and its assignment on roulette wheel
    /// </summary>
    public class RouletteGenotypePlaceholder
    {
        public Genotype Genotype { get; private set; }
        public float MinValue { get; private set; }
        public float MaxValue { get; private set; }

        public RouletteGenotypePlaceholder(Genotype genotype, float minValue, float maxValue)
        {
            Genotype = genotype;
            MinValue = minValue;
            MaxValue = maxValue;
        }
    }
}