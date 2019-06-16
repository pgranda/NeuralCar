using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.AI.NeuralNetworkTopology;

namespace Assets.Scripts.Managers
{
    /// <summary>
    /// Class which handles Examination simulation
    /// </summary>
    public class ExaminationSimulationManager : SimulationManager
    {
        /// <summary>
        /// Instance of the class used in simple Singleton design pattern.
        /// </summary>
        public static ExaminationSimulationManager Instance { get; private set; }

        public Genotype ExaminationGenotype { get; private set; }

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
            if (!SceneManager.Instance.CarControllers.Any(cc => cc.IsCarInOperation))
            {
                StartSimulation();
            }
        }

        /// <summary>
        /// Method that Starts Simulation process
        /// </summary>
        public override void StartSimulation()
        {
            ExaminationGenotype = SceneManager.Instance.ReadTrainingDataFromFile();

            if (ExaminationGenotype == null)
            {
                return;
            }

            PrepareGenerationForSimulation(new List<Genotype>() { ExaminationGenotype });
        }
    }
}