using System;
using System.Collections.Generic;
using UnityEngine;

namespace farmingsim
{
    [Serializable]
    [CreateAssetMenu(fileName = "New CraftingRecipe", menuName = "Crafting/Recipe", order = 1)]
    public class CraftingRecipe : ScriptableObject
    {
        [SerializeField] private List<CraftingIngredient> ingredients;
        [SerializeField] private List<ScriptableObject> result;
    }
}