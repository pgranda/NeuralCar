using UnityEngine;

namespace Assets.Scripts.AI.Utilities
{
    /// <summary>
    /// Class containing constant fields used throughout whole project, it has been created
    /// for greater visibility and better code readability.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// LayerMask that all car components are assigned to, used to properly calculate collision of rays
        /// </summary>
        public static readonly LayerMask CarLayer = 1 << 9;

        /// <summary>
        /// LayerMask of Finish Line gameobject
        /// </summary>
        public static readonly LayerMask FinishLine = 1 << 10;

        /// <summary>
        /// LayerMask RayCasts should ignore
        /// </summary>
        public static readonly LayerMask RayCastIgnore = CarLayer | FinishLine;

        /// <summary>
        /// The maximum distance of ray being shot from car
        /// </summary>
        public static readonly int MaxRayDistance = 5;

        /// <summary>
        /// The angle of Right-Forward and Left-Forward rays being shot from car
        /// </summary>
        public static readonly int RayAngle = 45;

        /// <summary>
        /// String containing Trainind Data file path
        /// </summary>
        public static readonly string TrainingDataFilePath = Application.streamingAssetsPath + "/" + "TrainingData.json";

        /// <summary>
        /// Training scene build index
        /// </summary>
        public static readonly int TrainingSceneBuildIndex = 1;

        /// <summary>
        /// Exam scene build index
        /// </summary>
        public static readonly int ExamSceneBuildIndex = 2;

        /// <summary>
        /// Minimum value of gene in starting population
        /// </summary>
        public const float MinGeneValue = -3;

        /// <summary>
        /// Maximum value of gene in starting population
        /// </summary>
        public const float MaxGeneValue =3;

        /// <summary>
        /// Probability of crossover operation being performed
        /// </summary>
        public const float CrossoverProbability = 0.6f;

        /// <summary>
        /// Probability of mutation operation being performed
        /// </summary>
        public const float MutationProbability = 0.1f;

        /// <summary>
        /// Number of generations that serves as condition of current simulation being terminated unsuccessfully
        /// </summary>
        public const int GenerationsStopCondition = 20;

        /// <summary>
        /// Number of laps being completed by a car that serves as condition of current simulation being terminated successfully
        /// </summary>
        public const int MinimumAmountOfLaps = 2;
    }
}
