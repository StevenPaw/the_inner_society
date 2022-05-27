using UnityEngine;

namespace farmingsim
{
    public interface IItem
    {
        public Sprite GetInventoryIcon();
        public string GetName();
    }
}