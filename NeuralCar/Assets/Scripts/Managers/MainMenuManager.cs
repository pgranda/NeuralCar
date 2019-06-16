using Assets.Scripts.AI.Utilities;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    /// <summary>
    /// Manager class for handling Main Menu scene logic.
    /// </summary>
    public sealed class MainMenuManager : MonoBehaviour
    {
        /// <summary>
        /// Changes scene to Training upon clicking on Training button.
        /// </summary>
        public void LaunchTrainingScene()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(Constants.TrainingSceneBuildIndex);
        }

        /// <summary>
        /// Changes scene to Exam upon clicking on Exam button.
        /// </summary>
        public void LaunchExamScene()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(Constants.ExamSceneBuildIndex);
        }

        /// <summary>
        /// Depending on current application platform, upon clicking on Quit button,
        /// application process is either killed or Playing state is changed back to Edit mode.
        /// </summary>
        public void QuitApplication()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
