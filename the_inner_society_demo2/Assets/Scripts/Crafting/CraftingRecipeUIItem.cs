using DG.Tweening;
using farmingsim.EventSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace farmingsim
{
    public class CraftingRecipeUIItem : MonoBehaviour
    {
        [SerializeField] private Image recipeIcon;
        [SerializeField] private TMP_Text recipeName;
        [SerializeField] private Image recipeInfo;
        [SerializeField] private Transform ingredientsHolder;
        [SerializeField] private GameObject ingredientPrefab;
        [SerializeField] private Color standardColor;
        [SerializeField] private Color inactiveColor;
        private CraftingRecipe displayedRecipe;
        private bool isAvailable;
        
        private void OnEnable()
        {
            Message<InventoryChangeEvent>.Add(OnInventoryChangeEvent);
        }

        private void OnDisable()
        {
            Message<InventoryChangeEvent>.Remove(OnInventoryChangeEvent);
        }
        
        public void Populate(CraftingRecipe recipe)
        {
            displayedRecipe = recipe;
            recipeName.text = displayedRecipe.CraftingRecipeName;
            IItem firstResult = displayedRecipe.Result[0] as IItem;
            if (firstResult != null)
            {
                recipeIcon.sprite = firstResult.GetInventoryIcon();
            } else
            {
                Destroy(gameObject);
                return;
            }
            foreach (CraftingIngredient ingredient in displayedRecipe.Ingredients)
            {
                CraftingIngredientUIItem item = Instantiate(ingredientPrefab, ingredientsHolder).GetComponent<CraftingIngredientUIItem>();
                item.Populate(ingredient);
            }
        }

        public void OnHoverEnter()
        {
            recipeInfo.DOFade(1f, 0.3f);
        }
        
        public void OnHoverExit()
        {
            recipeInfo.DOFade(0f, 0.3f);
        }

        private void CheckAvailability()
        {
            isAvailable = true;
            foreach (CraftingIngredient ingredient in displayedRecipe.Ingredients)
            {
                if (Inventory.Instance.GetAmountOfItemInInventory(ingredient.Item as IItem) < ingredient.Amount)
                {
                    isAvailable = false;
                }
            }

            if (isAvailable)
            {
                recipeIcon.color = standardColor;
            }
            else
            {
                recipeIcon.color = inactiveColor;
            }
        }
        
        public void OnCrafting()
        {
            foreach (CraftingIngredient ingredient in displayedRecipe.Ingredients)
            {
                Inventory.Instance.RemoveItemFromInventory(ingredient.Item as IItem, ingredient.Amount);
            }

            foreach (IItem item in displayedRecipe.Result)
            {
                Inventory.Instance.AddItemToInventory(item, 1);
            }
            
            Message.Raise(new InventoryChangeEvent());
        }

        public void OnInventoryChangeEvent(InventoryChangeEvent ctx)
        {
            CheckAvailability();
        }
    }
}