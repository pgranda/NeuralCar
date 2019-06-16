using Assets.Scripts.Car;
using Assets.Scripts.Managers;
using UnityEngine;

namespace Assets.Scripts.AI.Utilities
{
    /// <summary>
    /// Simple class that saves training data after Leader car triggers Finish Line Trigger
    /// </summary>
    public class FinishLineLogic : MonoBehaviour
    {
        /// <summary>
        /// Method that is called when car collides with Finish Line collider
        /// </summary>
        /// <param name="other">Car collider</param>
        private void OnTriggerEnter(Collider other)
        {
            var car = other.gameObject.GetComponent<CarController>();
            car.FinishedLaps++;
            SaveTrainingDataIfTrainingWasSuccessful(car);
        }

        /// <summary>
        /// Method that calls SceneManager Instance to save training data to file, stops time and asks UIManager Instance to show panel
        /// </summary>
        /// <param name="car">Car that hit the Finish Line collider</param>
        private void SaveTrainingDataIfTrainingWasSuccessful(CarController car)
        {
            if (IsTrainingCompletedSuccessfully(car))
            {
                SceneManager.Instance.SaveTrainingDataToFile();
                SceneManager.Instance.ChangeTimeScale(0);
                UIManager.Instance.UpdateShowHideTrainingPanel();
            }
        }

        /// <summary>
        /// Method that returns true or false depending if the car passed in as parameter fulfilled given requirements
        /// </summary>
        /// <param name="car">Car controller that hit the Finish Line collider</param>
        /// <returns>True or false depending on the requirements fulfillment</returns>
        private bool IsTrainingCompletedSuccessfully(CarController car)
        {
            return car == SceneManager.Instance.Leader && car.FinishedLaps >= Constants.MinimumAmountOfLaps;
        }
    }
}
