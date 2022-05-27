using UnityEngine;

namespace farmingsim
{
    public interface IPlantable
    {
        public Sprite[] GetSprites();
        public float GetGrowthDuration();
    }
}