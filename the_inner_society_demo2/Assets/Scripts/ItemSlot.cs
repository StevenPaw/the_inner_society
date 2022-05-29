using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace farmingsim
{
    
    public class ItemSlot : MonoBehaviour
    {
        [SerializeField] private Image itemImage;
        [SerializeField] private TMP_Text itemAmountText;
        private IItem holdedItem;
        private int itemAmount = 1;

        public IItem HoldedItem
        {
            get => holdedItem;
            set => holdedItem = value;
        }

        private void Update()
        {
            if (holdedItem != null)
            {
                itemImage.sprite = holdedItem.GetInventoryIcon();
                itemAmountText.text = itemAmount.ToString();
            }
            else
            {
                itemImage.gameObject.SetActive(false);
            }
        }
    }
}