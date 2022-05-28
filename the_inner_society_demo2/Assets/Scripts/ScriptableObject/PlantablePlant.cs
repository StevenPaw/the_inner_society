using System;
using UnityEngine;

namespace farmingsim
{
    [Serializable]
    [CreateAssetMenu(fileName = "New Plantable Plant", menuName = "Farming/PlantablePlant", order = 1)]
    public class PlantablePlant : ScriptableObject, IPlantable, IItem
    {
        [SerializeField] private string plantName;
        [SerializeField] private Sprite[] sprites;
        [SerializeField] private float growthDurationPerStep;

        public Sprite[] GetSprites()
        {
            return sprites;
        }

        public float GetGrowthDuration()
        {
            return growthDurationPerStep;
        }

        public Sprite GetInventoryIcon()
        {
            return sprites[sprites.Length];
        }

        public string GetName()
        {
            return plantName;
        }
    }
}