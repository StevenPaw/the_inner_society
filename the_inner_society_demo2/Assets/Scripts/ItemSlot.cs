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
        [SerializeField] private int slotID;
        [SerializeField] private GameObject activeIndicator;
        private IItem holdedItem;
        private int itemAmount = 1;

        public int SlotID
        {
            get => slotID;
            set => slotID = value;
        }

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

            if (Inventory.Instance.CurrentlyActiveSlot == slotID)
            {
                activeIndicator.SetActive(true);
            }
            else
            {
                activeIndicator.SetActive(false);
            }
        }
    }
}