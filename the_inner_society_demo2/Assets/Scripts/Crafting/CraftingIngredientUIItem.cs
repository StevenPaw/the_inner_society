using farmingsim.EventSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace farmingsim
{
    public class CraftingIngredientUIItem : MonoBehaviour
    {
        [SerializeField] private Image ingredientsIcon;
        [SerializeField] private TMP_Text ingredientsAmountText;
        [SerializeField] private Color standardColor;
        [SerializeField] private Color unavailableColor;
        private CraftingIngredient displayedIngredient;
        
        private void OnEnable()
        {
            Message<InventoryChangeEvent>.Add(OnInventoryChangeEvent);
        }

        private void OnDisable()
        {
            Message<InventoryChangeEvent>.Remove(OnInventoryChangeEvent);
        }
        
        public void Populate(CraftingIngredient ingredient)
        {
            displayedIngredient = ingredient;
            IItem item = displayedIngredient.Item as IItem;
            if (item != null)
            {
                ingredientsIcon.sprite = item.GetInventoryIcon();
                ingredientsAmountText.text = displayedIngredient.Amount.ToString();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnInventoryChangeEvent(InventoryChangeEvent ctx)
        {
            IItem item = displayedIngredient.Item as IItem;
            int amount = Inventory.Instance.GetAmountOfItemInInventory(item);
            if (amount >= displayedIngredient.Amount)
            {
                ingredientsIcon.color = standardColor;
            }
            else
            {
                ingredientsIcon.color = unavailableColor;
            }
        }
    }
}