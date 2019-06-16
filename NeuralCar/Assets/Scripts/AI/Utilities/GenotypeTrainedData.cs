using System;
using System.Collections.Generic;

namespace Assets.Scripts.AI.Utilities
{
    /// <summary>
    /// Simple class that allows to assign Weights(Genes) passed from Genotype and save it to JSON file
    /// </summary>
    [Serializable]
    public class GenotypeTrainedData
    {
        public List<float> Weights;
    }
}
