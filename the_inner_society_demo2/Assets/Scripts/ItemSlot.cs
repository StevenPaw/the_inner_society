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
        [SerializeField] private Button interactButton;
        [SerializeField] private PlayerController playerController;
        [SerializeField] private Sprite freeSlotImage;
        private IItem holdedItem;
        private int holdedItemAmount = 1;

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

        public int HoldedItemAmount
        {
            get => holdedItemAmount;
            set => holdedItemAmount = value;
        }

        public PlayerController PlayerController
        {
            get => playerController;
            set => playerController = value;
        }

        private void Start()
        {
            playerController = PlayerController.Instance;
        }

        private void Update()
        {
            if (holdedItem != null)
            {
                itemImage.sprite = holdedItem.GetInventoryIcon();
                itemAmountText.text = holdedItemAmount.ToString();
            }
            else
            {
                itemImage.sprite = freeSlotImage;
            }

            if (Inventory.Instance.CurrentlyActiveSlot == slotID)
            {
                activeIndicator.SetActive(true);
            }
            else
            {
                activeIndicator.SetActive(false);
            }

            if (GameManager.Instance.InInventory)
            {
                interactButton.interactable = true;
            }
            else
            {
                interactButton.interactable = false;
            }
        }

        public void OnButtonleftPress()
        {
            if (playerController.CursorManager.ItemInCursor != null){
                if (holdedItem == null)
                {
                    holdedItem = playerController.CursorManager.ItemInCursor;
                    holdedItemAmount = playerController.CursorManager.ItemInCursorAmount;
                    playerController.CursorManager.ItemInCursor = null;
                    playerController.CursorManager.ItemInCursorAmount = 0;
                }  
                else if (playerController.CursorManager.ItemInCursor.GetName() == holdedItem.GetName())
                {
                    playerController.CursorManager.ItemInCursorAmount += 1;
                    holdedItemAmount -= 1;
                }
                else
                {
                    IItem inventoryItem = holdedItem;
                    holdedItem = playerController.CursorManager.ItemInCursor;
                    playerController.CursorManager.ItemInCursor = inventoryItem;
                }
            }
            else
            {
                if (holdedItem != null)
                {
                    playerController.CursorManager.ItemInCursorAmount += 1;
                    playerController.CursorManager.ItemInCursor = holdedItem;
                    holdedItemAmount -= 1;
                }
            }

            if (holdedItemAmount <= 0)
            {
                holdedItem = null;
            }

            Inventory.Instance.Items[slotID] = holdedItem;
            Inventory.Instance.ItemAmounts[slotID] = holdedItemAmount;
        }
    }
}