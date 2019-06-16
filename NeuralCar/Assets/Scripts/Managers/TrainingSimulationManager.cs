using Assets.Scripts.AI.Utilities;
using System.Linq;

namespace Assets.Scripts.Managers
{
    /// <summary>
    /// Class which handles Training simulation
    /// </summary>
    public class TrainingSimulationManager : SimulationManager
    {
        /// <summary>
        /// Instance of the class used in simple Singleton design pattern.
        /// </summary>
        public static TrainingSimulationManager Instance { get; private set; }

        public int Generation = 1;

        private GeneticAlgorithm geneticAlgorithm;

        /// <summary>
        /// Simple Singleton pattern creation.
        /// </summary>
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        protected override void Update()
        {
            RestartSimulationIfAllCarsFailed();
            RestartTrainingIfCurrentOneWasUnsuccessful();
        }

        /// <summary>
        /// Method that Starts Simulation process
        /// </summary>
        public override void StartSimulation()
        {
            geneticAlgorithm = new GeneticAlgorithm(PopulationSize, CalculateAmountOfWeights());
            PrepareGenerationForSimulation(geneticAlgorithm.GenerationPopulation);
        }

        /// <summary>
        /// Method that reloads scene terminating current simulation process.
        /// It happens when simulation couldn't create satisfactory progress over given time.
        /// </summary>
        private void RestartTrainingIfCurrentOneWasUnsuccessful()
        {
            if (!SceneManager.Instance.CarControllers.Any(cc => cc.FinishedLaps > 0) && Generation > Constants.GenerationsStopCondition)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(Constants.TrainingSceneBuildIndex);
            }
        }

        /// <summary>
        /// Method that restarts simulation process after all cars fail to proceed in given generation
        /// </summary>
        private void RestartSimulationIfAllCarsFailed()
        {
            if (!SceneManager.Instance.CarControllers.Any(cc => cc.IsCarInOperation))
            {
                Generation++;
                geneticAlgorithm.HandleGenerationSimulationFinish();
                PrepareGenerationForSimulation(geneticAlgorithm.GenerationPopulation);
            }
        }

        /// <summary>
        /// Method that calculates amount of weights
        /// </summary>
        /// <returns>Calculated amount of weights</returns>
        private int CalculateAmountOfWeights()
        {
            int amountOfWeights = 0;
            for (int i = 0; i < NumberOfNeuronsInEachLayer.Count - 1; i++)
            {
                amountOfWeights += (NumberOfNeuronsInEachLayer[i] + 1) * NumberOfNeuronsInEachLayer[i + 1];
            }

            return amountOfWeights;
        }
    }
}
