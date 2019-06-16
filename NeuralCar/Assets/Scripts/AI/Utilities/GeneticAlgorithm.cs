using Assets.Scripts.AI.NeuralNetworkTopology;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.AI.Utilities
{
    /// <summary>
    /// Class responsible for operations on genotypes
    /// </summary>
    public class GeneticAlgorithm
    {
        public List<Genotype> GenerationPopulation { get; private set; }

        /// <summary>
        /// Constructor which calls method which initialises starting generation population
        /// </summary>
        /// <param name="populationSize">Number defining how big the population will be</param>
        /// <param name="genesAmount">Amount of genes that will be describing every member of the population</param>
        public GeneticAlgorithm(int populationSize, int genesAmount)
        {
            InitialiseGenerationPopulation(populationSize, genesAmount);
        }

        /// <summary>
        /// Method that performs all actions required on each generation end
        /// </summary>
        public void HandleGenerationSimulationFinish()
        {
            FitnessCalculator.CalculateFitness(GenerationPopulation);

            List<Genotype> elite = SelectElitistPopulationMembers();

            var newPopulation = Selection(GenerationPopulation.Count() - elite.Count());

            MutatePopulation(newPopulation);

            newPopulation.AddRange(elite);

            GenerationPopulation = newPopulation;
        }

        /// <summary>
        /// Method that creates and assigns genotypes to each member of population
        /// </summary>
        /// <param name="populationSize">Number defining how big the population will be</param>
        /// <param name="genesAmount">Amount of genes that will be describing every member of the population</param>
        private void InitialiseGenerationPopulation(int populationSize, int genesAmount)
        {
            GenerationPopulation = new List<Genotype>();
            for (int i = 0; i < populationSize; i++)
            {
                GenerationPopulation.Add(new Genotype(genesAmount));
            }
        }

        /// <summary>
        /// Method that creates new population of genotypes by selecting and interbreeding parents
        /// </summary>
        /// <param name="amountOfParentsToSelect">The number of parents to be selected during selection</param>
        /// <returns>List of child genotypes that will be included in next generation</returns>
        private List<Genotype> Selection(int amountOfParentsToSelect)
        {
            var parentsPopulation = Roulette.SelectParents(GenerationPopulation, amountOfParentsToSelect);

            List<Genotype> newPopulation = Interbreeding(parentsPopulation);
            return newPopulation;
        }

        /// <summary>
        /// Method responsible for selecting two fittest members of given population
        /// </summary>
        /// <returns>List of elite members of population</returns>
        private List<Genotype> SelectElitistPopulationMembers()
        {
            var elite = new List<Genotype>();
            GenerationPopulation = GenerationPopulation.OrderByDescending(p => p.Fitness).ToList();
            elite.Add(GenerationPopulation.ElementAt(0));
            elite.Add(GenerationPopulation.ElementAt(1));
            return elite;
        }

        /// <summary>
        /// Method responsible for random pairing of parents and interbreeding them in order to create offsprings
        /// </summary>
        /// <param name="parentsPopulation">List of genotypes serving as parents in interbreeding</param>
        /// <returns>Freshly created population</returns>
        private List<Genotype> Interbreeding(List<Genotype> parentsPopulation)
        {
            List<Genotype> newPopulation = new List<Genotype>();
            var notUsedIndexes = new List<int>();
            for (int i = 0; i < parentsPopulation.Count; i++)
            {
                notUsedIndexes.Add(i);
            }

            while (!IsNewPopulationGenerated(parentsPopulation, newPopulation))
            {
                int randomMemberIndex1 = GenerateNotUsedRandomMemberIndex(notUsedIndexes);
                int randomMemberIndex2 = GenerateNotUsedRandomMemberIndex(notUsedIndexes);

                var offsprings = ExecuteUniformCrossoverOperator(parentsPopulation[randomMemberIndex1], parentsPopulation[randomMemberIndex2]);

                AddOffspringsToNewPopulation(parentsPopulation.Count, newPopulation, offsprings);
            }

            return newPopulation;
        }

        /// <summary>
        /// Method that checks if population of expected size was already generated
        /// </summary>
        /// <param name="parentsPopulation">List of genotypes serving as parents</param>
        /// <param name="newPopulation">List of freshly created genotypes</param>
        /// <returns>True if population was generated, otherwise false</returns>
        private bool IsNewPopulationGenerated(List<Genotype> parentsPopulation, List<Genotype> newPopulation)
        {
            return newPopulation.Count == parentsPopulation.Count;
        }

        /// <summary>
        /// Method that adds created offspring during interbreeding to the new population if the population did not meet required size
        /// </summary>
        /// <param name="populationSize">Expected population size</param>
        /// <param name="newPopulation">Newly generated population</param>
        /// <param name="offsprings">Children offsprings created during interbreeding</param>
        private void AddOffspringsToNewPopulation(int populationSize, List<Genotype> newPopulation, List<Genotype> offsprings)
        {
            foreach (var offspring in offsprings)
            {
                if (newPopulation.Count < populationSize)
                {
                    newPopulation.Add(offspring);
                }
            }
        }

        /// <summary>
        /// Method that checks if crossover operation should be performed, depending on CrossoverProbability value
        /// </summary>
        /// <returns>True if randomly generated number is lesser than CrossoverProbability, false otherwise</returns>
        private bool ShouldPerformCrossoverOperation()
        {
            return RandomNumberGenerator.GenerateRandomNumberWithinRange(0.0f, 1.0f) < Constants.CrossoverProbability;
        }

        /// <summary>
        /// Method that returns random not used population member index
        /// </summary>
        /// <param name="notUsedIndexes">List of not used members indexes</param>
        /// <returns>Random not used member index</returns>
        private int GenerateNotUsedRandomMemberIndex(List<int> notUsedIndexes)
        {
            int randomIndex = RandomNumberGenerator.GenerateRandomNumberWithinRange(0, notUsedIndexes.Count);
            int randomMemberIndex = notUsedIndexes.ElementAt(randomIndex);

            notUsedIndexes.RemoveAt(randomIndex);
            return randomMemberIndex;
        }

        /// <summary>
        /// Method that executes uniform crossover operator on parents
        /// </summary>
        /// <param name="parent1">First parent that will be used during crossover operation</param>
        /// <param name="parent2">Second parent that will be used during crossover operation</param>
        /// <returns>List of offsprings created during crossover operation</returns>
        private List<Genotype> ExecuteUniformCrossoverOperator(Genotype parent1, Genotype parent2)
        {
            var offsprings = new List<Genotype>();
            var offspring1Genes = new List<float>();
            var offspring2Genes = new List<float>();

            for (int i = 0; i < parent1.Genes.Count; i++)
            {
                if (!ShouldPerformCrossoverOperation())
                {
                    offspring1Genes.Add(parent1.Genes.ElementAt(i));
                    offspring2Genes.Add(parent2.Genes.ElementAt(i));
                }
                else
                {
                    offspring1Genes.Add(parent2.Genes.ElementAt(i));
                    offspring2Genes.Add(parent1.Genes.ElementAt(i));
                }
            }

            offsprings.Add(new Genotype(offspring1Genes));
            offsprings.Add(new Genotype(offspring2Genes));

            return offsprings;
        }

        //public void ExecuteOnePointCrossoverOperator(Genotype parent1, Genotype parent2, out Genotype offspring1, out Genotype offspring2)
        //{
        //    //Initialise new parameter vectors
        //    int amountOfGenes = parent1.Genes.Count;
        //    int crossoverPoint = RandomNumberGenerator.GenerateRandomNumberWithinRange(0, amountOfGenes);

        //    float[] off1Parameters = new float[amountOfGenes], off2Parameters = new float[amountOfGenes];

        //    //Iterate over all parameters randomly swapping
        //    for (int i = 0; i < amountOfGenes; i++)
        //    {
        //        if (i > crossoverPoint)
        //        {
        //            off1Parameters[i] = parent2.Genes[i];
        //            off2Parameters[i] = parent1.Genes[i];
        //        }
        //        else
        //        {
        //            off1Parameters[i] = parent1.Genes[i];
        //            off2Parameters[i] = parent2.Genes[i];
        //        }
        //    }

        //    offspring1 = new Genotype(off1Parameters.ToList());
        //    offspring2 = new Genotype(off2Parameters.ToList());
        //}

        /// <summary>
        /// Method responsible for mutating all members of newly created population depending on mutation probability
        /// </summary>
        /// <param name="newPopulation">Population on which mutation should be executed</param>
        private void MutatePopulation(List<Genotype> newPopulation)
        {
            foreach (var member in newPopulation)
            {
                if (RandomNumberGenerator.GenerateRandomNumberWithinRange(0.0f, 1.0f) < Constants.MutationProbability)
                {
                    MutateGenotype(member);
                }
            }
        }

        /// <summary>
        /// Method responsible for mutating given genotype by replacing gene with randomly generated number
        /// </summary>
        /// <param name="genotype">Genotype which randomly chosen gene will be mutated</param>
        private void MutateGenotype(Genotype genotype)
        {
            var geneIndex = RandomNumberGenerator.GenerateRandomNumberWithinRange(0, genotype.Genes.Count - 1);

            genotype.Genes[geneIndex] =
                RandomNumberGenerator.GenerateRandomNumberWithinRange(Constants.MinGeneValue, Constants.MaxGeneValue);
        }
    }
}