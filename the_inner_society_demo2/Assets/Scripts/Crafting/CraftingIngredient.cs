using System;
using UnityEngine;

namespace farmingsim
{
    [Serializable]
    public class CraftingIngredient
    {
        [SerializeField] private ScriptableObject item;
        [SerializeField] private int amount = 1;
        [SerializeField] private bool isConsumedOnCrafting = true;

        public ScriptableObject Item => item;
        public int Amount => amount;
        public bool IsConsumedOnCrafting => isConsumedOnCrafting;
    }
}