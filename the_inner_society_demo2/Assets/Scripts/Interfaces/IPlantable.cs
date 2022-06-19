using UnityEngine;

namespace farmingsim
{
    public interface IPlantable
    {
        public string GetName();
        public Sprite[] GetSprites();
        public float GetGrowthDuration();
        public IItem[] GetHarvestedItems();
    }
}