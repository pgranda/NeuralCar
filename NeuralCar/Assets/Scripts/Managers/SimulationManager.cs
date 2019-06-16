using Assets.Scripts.AI.NeuralNetworkTopology;
using Assets.Scripts.Car;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    /// <summary>
    /// Parent class for both Examination and Training managers containing methods and fields used in both classes
    /// </summary>
    public abstract class SimulationManager : MonoBehaviour
    {
        public int PopulationSize;

        public List<int> NumberOfNeuronsInEachLayer;

        protected abstract void Update();

        /// <summary>
        /// Method that will start simulation
        /// </summary>
        public abstract void StartSimulation();

        /// <summary>
        /// Method that prepares given population for next simulation
        /// </summary>
        /// <param name="currentPopulation">List of Genotypes (population specimen)</param>
        public void PrepareGenerationForSimulation(List<Genotype> currentPopulation)
        {
            SceneManager.Instance.InitialiseCarControllers(currentPopulation.Count);

            int i = 0;
            foreach (var carController in SceneManager.Instance.CarControllers)
            {
                ResetCarControllerVariables(currentPopulation[i], carController);
                i++;
            }

            SceneManager.Instance.Restart();
        }

        /// <summary>
        /// Method that prepares given population for next simulation
        /// </summary>
        /// <param name="genotype">Genotype that is going to be reseted </param>
        /// <param name="carController">Car controller which parameters are going to be reseted</param> 
        protected void ResetCarControllerVariables(Genotype genotype, CarController carController)
        {
            genotype.Reset();
            carController.CarBrain = new CarBrain(genotype, NumberOfNeuronsInEachLayer);
            carController.CarRigidbody.constraints = RigidbodyConstraints.None;
            carController.DistanceTravelled = 0;
            carController.LastCarPosition = SceneManager.Instance.StartingPoint.transform.position;
            carController.ChangeCarMaterials(carController.DefaultMaterial);
            carController.IsCarInOperation = true;
            carController.transform.position = SceneManager.Instance.CarStartingPosition;
            carController.transform.rotation = SceneManager.Instance.CarStartingRotation;
            carController.FinishedLaps = 0;
        }
    }
}