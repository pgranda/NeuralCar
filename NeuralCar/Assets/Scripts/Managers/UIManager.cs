using Assets.Scripts.AI.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Managers
{
    /// <summary>
    /// Manager class for handling User Interface (changing and updating text on UI).
    /// </summary>
    public sealed class UIManager : MonoBehaviour
    {
        /// <summary>
        /// Instance of the class used in simple Singleton design pattern,
        /// thanks to which its elements are available for modification in classes that need to change values on UI.
        /// </summary>
        public static UIManager Instance { get; private set; }

        [Space]
        [Header("Car")]
        [Space]

        [SerializeField]
        private Text inputValueText;
        [SerializeField]
        private Text speedText;

        [Space]
        [Header("Simulation")]
        [Space]

        [SerializeField]
        private Text generationText;
        [SerializeField]
        private Text distanceTravelledText;
        [SerializeField]
        private Text maxDistanceTravelledText;
        [SerializeField]
        private Text timeText;

        [Space]
        [Header("Rays")]
        [Space]

        [SerializeField]
        private Text leftRayText;
        [SerializeField]
        private Text leftForwardRayText;
        [SerializeField]
        private Text forwardRayText;
        [SerializeField]
        private Text rightForwardRayText;
        [SerializeField]
        private Text rightRayText;

        [Space]
        [Header("Utilities")]
        [Space]

        [SerializeField]
        private GameObject trainingProcessPanel;

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

        /// <summary>
        /// Method used to update Left Ray distance text on UI.
        /// </summary>
        /// <param name="newText">Formatted string containing new text displayed on UI. </param>
        public void UpdateLeftRayText(string newText)
        {
            leftRayText.text = newText;
        }

        /// <summary>
        /// Method used to update Left-Forward Ray distance text on UI.
        /// </summary>
        /// <param name="newText">Formatted string containing new text displayed on UI. </param>
        public void UpdateLeftForwardRayText(string newText)
        {
            leftForwardRayText.text = newText;
        }

        /// <summary>
        /// Method used to update Forward Ray distance text on UI.
        /// </summary>
        /// <param name="newText">Formatted string containing new text displayed on UI. </param>
        public void UpdateForwardRayText(string newText)
        {
            forwardRayText.text = newText;
        }

        /// <summary>
        /// Method used to update Right-Forward Ray distance text on UI.
        /// </summary>
        /// <param name="newText">Formatted string containing new text displayed on UI. </param>
        public void UpdateRightForwardRayText(string newText)
        {
            rightForwardRayText.text = newText;
        }

        /// <summary>
        /// Method used to update Right Ray distance text on UI.
        /// </summary>
        /// <param name="newText">Formatted string containing new text displayed on UI. </param>
        public void UpdateRightRayText(string newText)
        {
            rightRayText.text = newText;
        }

        /// <summary>
        /// Method used to update Input value text on UI.
        /// </summary>
        /// <param name="newText">Formatted string containing new text displayed on UI. </param>
        public void UpdateInputValueText(string newText)
        {
            inputValueText.text = newText;
        }

        /// <summary>
        /// Method used to update Car speed text on UI.
        /// </summary>
        /// <param name="newText">Formatted string containing new text displayed on UI. </param>
        public void UpdateSpeedText(string newText)
        {
            speedText.text = newText;
        }

        /// <summary>
        /// Method used to update Generation count text on UI.
        /// </summary>
        /// <param name="newText">Formatted string containing new text displayed on UI. </param>
        public void UpdateGenerationText(string newText)
        {
            generationText.text = newText;
        }

        /// <summary>
        /// Method used to update current Leader distance travelled text on UI.
        /// </summary>
        /// <param name="newText">Formatted string containing new text displayed on UI. </param>
        public void UpdateDistanceTravelledText(string newText)
        {
            distanceTravelledText.text = newText;
        }

        /// <summary>
        /// Method used to update max distance travelled in this learning process text on UI.
        /// </summary>
        /// <param name="newText">Formatted string containing new text displayed on UI. </param>
        public void UpdateMaxDistanceTravelledText(string newText)
        {
            maxDistanceTravelledText.text = newText;
        }

        /// <summary>
        /// Method used to update Time text on UI.
        /// </summary>
        /// <param name="newText">Formatted string containing new text displayed on UI. </param>
        public void UpdateTimeText(string newText)
        {
            timeText.text = newText;
        }

        /// <summary>
        /// Method used to Show/Hide Training Process Panel.
        /// </summary>
        public void UpdateShowHideTrainingPanel()
        {
            trainingProcessPanel.SetActive(!trainingProcessPanel.activeInHierarchy);
        }

        /// <summary>
        /// Changes scene to Exam upon clicking on OK button.
        /// </summary>
        public void LaunchExamScene()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(Constants.ExamSceneBuildIndex);
        }
    }
}
