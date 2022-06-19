using System;
using UnityEngine;

namespace farmingsim
{
    [Serializable]
    [CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item", order = 1)]
    public class Item : ScriptableObject, IItem
    {
        [SerializeField] private string plantName;
        [SerializeField] private Sprite sprite;

        public Sprite GetInventoryIcon()
        {
            return sprite;
        }

        public string GetName()
        {
            return plantName;
        }
    }
}