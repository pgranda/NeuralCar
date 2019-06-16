using Assets.Scripts.AI.NeuralNetworkTopology;
using Assets.Scripts.AI.Utilities;
using Assets.Scripts.Car;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    /// <summary>
    /// Manager class for handling things that happens in the scene, for instance changing Leader depending on distance travelled
    /// changing camera after getting input from user, updating values and texts displayed on UI by calling methods from UIManager
    /// </summary>
    public sealed class SceneManager : MonoBehaviour
    {
        /// <summary>
        /// Instance of the class used in simple Singleton design pattern.
        /// </summary>
        public static SceneManager Instance { get; private set; }

        [Space]
        [Header("Car")]
        [Space]
        public GameObject CarPrefab;
        public GameObject StartingPoint;

        [Space]
        [Header("Simulation")]
        [Space]
        public CarController Leader;
        public float MaxDistanceTravelled;
        public Camera MainCamera;

        public List<CarController> CarControllers { get; private set; }

        public Vector3 CarStartingPosition;
        public Quaternion CarStartingRotation;

        private float timer;
        private Camera leaderCarCamera;
        private bool useCarCamera;

        public bool TrainingScene
        {
            get
            {
                return UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex ==
                       Constants.TrainingSceneBuildIndex;
            }
        }

        public SimulationManager SimulationManager
        {
            get
            {
                if (TrainingScene)
                {
                    return TrainingSimulationManager.Instance;
                }

                return ExaminationSimulationManager.Instance;
            }
        }

        /// <summary>
        /// Simple Singleton pattern creation. As well as initialization of some important configurations such as:
        /// Timescale and setting starting position and rotation for CarControllers
        /// </summary>
        private void Awake()
        {
            CarControllers = new List<CarController>();

            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            Time.timeScale = 1;

            CarStartingPosition = StartingPoint.transform.position;
            CarStartingRotation = StartingPoint.transform.rotation;
        }

        /// <summary>
        /// Method called after pressing Play, it just starts simulation
        /// </summary>
        private void Start()
        {
            SimulationManager.StartSimulation();

            UIManager.Instance.UpdateMaxDistanceTravelledText(string.Format("Max Distance: {0:F}", MaxDistanceTravelled));
        }

        /// <summary>
        /// Method used to update and handle things that needed to be dealt with each frame
        /// </summary>
        private void Update()
        {
            HandleLeaderChange();
            HandleCameraChange();
            UpdateValuesAndTexts();
        }

        /// <summary>
        /// Method that chooses new Leader from all the CarControllers
        /// </summary>
        public void HandleLeaderChange()
        {
            var candidateForLeader = CarControllers.OrderByDescending(c => c.DistanceTravelled).First();

            if (candidateForLeader == Leader)
            {
                return;
            }

            ChangeLeader(candidateForLeader);
        }

        /// <summary>
        /// Method that changes simulation timeScale
        /// </summary>
        /// <param name="newTimeScaleValue"></param>
        public void ChangeTimeScale(float newTimeScaleValue)
        {
            Time.timeScale = newTimeScaleValue;
        }

        /// <summary>
        /// Method that initialises car controllers
        /// </summary>
        /// <param name="numberOfCars">Number of cars to be created</param>
        public void InitialiseCarControllers(int numberOfCars)
        {
            ClearCarControllers();
            CreateCarControllers(numberOfCars);
        }

        /// <summary>
        /// Method that clears Leader gameobject assignment
        /// </summary>
        public void Restart()
        {
            Leader = null;
        }

        /// <summary>
        /// Method that saves training data to JSON file
        /// </summary>
        public void SaveTrainingDataToFile()
        {
            GenotypeTrainedData data = new GenotypeTrainedData();
            data.Weights = Leader.CarBrain.Genotype.Genes;

            string dataAsJson = JsonUtility.ToJson(data);

            File.WriteAllText(Constants.TrainingDataFilePath, dataAsJson);
        }

        /// <summary>
        /// Method that reads training data from JSON file and return Genotype with genes from file
        /// </summary>
        /// <returns>Genotype and its weights that were read from JSON file, or null if file was not found</returns>
        public Genotype ReadTrainingDataFromFile()
        {
            if (File.Exists(Constants.TrainingDataFilePath))
            {
                GenotypeTrainedData data = new GenotypeTrainedData();
                string dataAsJson = File.ReadAllText(Constants.TrainingDataFilePath);
                data = JsonUtility.FromJson<GenotypeTrainedData>(dataAsJson);

                return new Genotype(data.Weights);
            }
            else
            {
                Debug.LogError("Cannot read data from file or file does not exist! " + Constants.TrainingDataFilePath);
                return null;
            }
        }

        /// <summary>
        /// Method that calls ChangeTimeScale method with 1 - default timeScale value,
        /// after clicking Cancel when Training Process window shows.
        /// </summary>
        public void OnCancelClickedOnTrainingProcess()
        {
            ChangeTimeScale(1);
        }
        
        /// <summary>
        /// Method that handles camera change after getting input from user
        /// </summary>
        private void HandleCameraChange()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                MainCamera.enabled = useCarCamera;
                leaderCarCamera.enabled = !useCarCamera;
                useCarCamera = !useCarCamera;
            }
        }

        /// <summary>
        /// Method that changes material on cars from default to leader and the other way around
        /// It also changes car cameras.
        /// </summary>
        /// <param name="candidateForLeader">Car controller that is going to be new leader in given simulation</param>
        private void ChangeLeader(CarController candidateForLeader)
        {
            if (Leader != null)
            {
                Leader.ChangeCarMaterials(Leader.DefaultMaterial);
                Leader.CarCamera.enabled = false;
            }

            Leader = candidateForLeader;
            Leader.ChangeCarMaterials(Leader.LeaderMaterial);
            if (useCarCamera)
            {
                Leader.CarCamera.enabled = true;
            }

            leaderCarCamera = Leader.CarCamera;
        }

        /// <summary>
        /// Method that calls UIManager instance to update texts on the UI
        /// </summary>
        private void UpdateValuesAndTexts()
        {
            UIManager.Instance.UpdateInputValueText(string.Format("Input Value: {0}", Leader.CurrentSteeringValue));
            UIManager.Instance.UpdateSpeedText(string.Format("Speed: {0} KM/H", Mathf.RoundToInt(Leader.CurrentVelocity)));

            UpdateGenerationText();

            UIManager.Instance.UpdateDistanceTravelledText(string.Format("Current Distance: {0:F}", Leader.DistanceTravelled));

            UpdateMaxDistanceTravelledText();

            if (Time.timeScale != 0)
            {
                UpdateTime();
            }
        }

        /// <summary>
        /// Method that creates Cars, instantiates prefab and assigns controller to car models
        /// </summary>
        /// <param name="numberOfCars">Number of cars to create</param>
        private void CreateCarControllers(int numberOfCars)
        {
            for (int i = 0; i < numberOfCars; i++)
            {
                GameObject car = Instantiate(CarPrefab);
                car.name = string.Format("Car ({0})", i);
                car.transform.position = CarStartingPosition;
                car.transform.rotation = CarStartingRotation;
                CarController carController = car.GetComponent<CarController>();
                CarControllers.Add(carController);
            }
        }

        /// <summary>
        /// Method that destroys cars and clear their reference in CarControllers list
        /// </summary>
        private void ClearCarControllers()
        {
            var carControllersGameObjects = CarControllers.Select(cc => cc.gameObject);

            for (int i = 0; i < carControllersGameObjects.Count(); i++)
            {
                Destroy(carControllersGameObjects.ElementAt(i));
            }

            CarControllers.Clear();
        }

        /// <summary>
        /// Method that simply calls UIManager to update generation text
        /// </summary>
        private void UpdateGenerationText()
        {
            if (TrainingScene)
            {
                UIManager.Instance.UpdateGenerationText(string.Format("Generation: {0}",
                    TrainingSimulationManager.Instance.Generation));
            }
        }

        /// <summary>
        /// Method that updates max distance travelled text on the UI
        /// </summary>
        private void UpdateMaxDistanceTravelledText()
        {
            if (Leader.DistanceTravelled > MaxDistanceTravelled)
            {
                MaxDistanceTravelled = Leader.DistanceTravelled;
                UIManager.Instance.UpdateMaxDistanceTravelledText(string.Format("Max Distance: {0:F}", MaxDistanceTravelled));
            }
        }

        /// <summary>
        /// Method that calculates and updates Time in the scene
        /// </summary>
        private void UpdateTime()
        {
            timer += Time.fixedDeltaTime;
            UIManager.Instance.UpdateTimeText(string.Format("Time: {0:F}", timer));
        }
    }
}
