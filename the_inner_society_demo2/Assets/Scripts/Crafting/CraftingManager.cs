using System;
using System.Collections;
using System.Collections.Generic;
using farmingsim.EventSystem;
using UnityEngine;

namespace farmingsim
{
    public class CraftingManager : MonoBehaviour
    {
        [SerializeField] private Canvas craftingMenu;
        [SerializeField] private Transform craftingRecipesHolder;
        [SerializeField] private GameObject craftingRecipePrefab;
        [SerializeField] private List<CraftingRecipe> recipes;

        private void OnEnable()
        {
            Message<ToggleInventoryEvent>.Add(OnToggleInventoryEvent);
        }

        private void OnDisable()
        {
            Message<ToggleInventoryEvent>.Remove(OnToggleInventoryEvent);
        }
        
        private void Start()
        {
            foreach (CraftingRecipe recipe in recipes)
            {
                CraftingRecipeUIItem item = Instantiate(craftingRecipePrefab, craftingRecipesHolder).GetComponent<CraftingRecipeUIItem>();
                item.Populate(recipe);
            }
        }

        private void OnToggleInventoryEvent(ToggleInventoryEvent ctx)
        {
            if (GameManager.Instance.InInventory)
            {
                craftingMenu.enabled = true;
            }
            else
            {
                craftingMenu.enabled = false;
            }
        }
    }
}