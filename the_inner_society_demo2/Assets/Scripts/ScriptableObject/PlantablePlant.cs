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
        [SerializeField] private Sprite inventorySprite;
        [SerializeField] private ScriptableObject[] harvestedItems;

        private void OnValidate()
        {
            for (int i = 0; i < harvestedItems.Length; i++)
            {
                if (harvestedItems[i] != null)
                {
                    if (harvestedItems[i] is IItem)
                    {
                    }
                    else
                    {
                        harvestedItems[i] = null;
                    }
                }
            }
        }

        public Sprite[] GetSprites()
        {
            return sprites;
        }

        public float GetGrowthDuration()
        {
            return growthDurationPerStep;
        }

        public IItem[] GetHarvestedItems()
        {
            IItem[] items = new IItem[harvestedItems.Length];
            for(int i = 0; i < harvestedItems.Length; i++)
            {
                items[i] = harvestedItems[i] as IItem;
            }
            return items;
        }

        public Sprite GetInventoryIcon()
        {
            return inventorySprite;
        }

        public string GetName()
        {
            return plantName;
        }
    }
}