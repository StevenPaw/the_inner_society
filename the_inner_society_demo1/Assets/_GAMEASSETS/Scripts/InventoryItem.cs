using UnityEngine;
using UnityEngine.UI;

namespace InnerSociety
{
    public class InventoryItem : MonoBehaviour
    {
        [SerializeField] private Item holdedItem;
        [SerializeField] private Image itemImage;

        public void SetItem(Item newItem)
        {
            holdedItem = newItem;
            itemImage.sprite = newItem.ItemSprite;
        }
    }
}