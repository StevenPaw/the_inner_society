using DG.Tweening;
using farmingsim.EventSystem;
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
        [SerializeField] private Sprite hoverSprite;
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

        private void OnEnable()
        {
            Message<InventoryChangeEvent>.Add(OnInventoryChangeEvent);
        }

        private void OnDisable()
        {
            Message<InventoryChangeEvent>.Remove(OnInventoryChangeEvent);
        }

        private void Start()
        {
            playerController = PlayerController.Instance;
        }

        public void OnButtonRightPress()
        {
            if (GameManager.Instance.InInventory)
            {
                if (playerController.CursorManager.ItemInCursor != null)
                {
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
                        int hItemAmount = holdedItemAmount;
                        holdedItem = playerController.CursorManager.ItemInCursor;
                        holdedItemAmount = playerController.CursorManager.ItemInCursorAmount;
                        playerController.CursorManager.ItemInCursor = inventoryItem;
                        playerController.CursorManager.ItemInCursorAmount = hItemAmount;
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
            
            Message.Raise(new InventoryChangeEvent());
        }

        public void OnButtonLeftPress()
        {
            if (GameManager.Instance.InInventory)
            {
                if (playerController.CursorManager.ItemInCursor != null)
                {
                    if (holdedItem == null)
                    {
                        holdedItem = playerController.CursorManager.ItemInCursor;
                        holdedItemAmount = playerController.CursorManager.ItemInCursorAmount;
                        playerController.CursorManager.ItemInCursor = null;
                        playerController.CursorManager.ItemInCursorAmount = 0;
                    }
                    else if (playerController.CursorManager.ItemInCursor.GetName() == holdedItem.GetName())
                    {
                        playerController.CursorManager.ItemInCursorAmount += holdedItemAmount;
                        holdedItemAmount = 0;
                    }
                    else
                    {
                        IItem inventoryItem = holdedItem;
                        int hItemAmount = holdedItemAmount;
                        holdedItem = playerController.CursorManager.ItemInCursor;
                        holdedItemAmount = playerController.CursorManager.ItemInCursorAmount;
                        playerController.CursorManager.ItemInCursor = inventoryItem;
                        playerController.CursorManager.ItemInCursorAmount = hItemAmount;
                    }
                }
                else
                {
                    if (holdedItem != null)
                    {
                        playerController.CursorManager.ItemInCursorAmount = holdedItemAmount;
                        playerController.CursorManager.ItemInCursor = holdedItem;
                        holdedItemAmount = 0;
                    }
                }

                if (holdedItemAmount <= 0)
                {
                    holdedItem = null;
                }

                Inventory.Instance.Items[slotID] = holdedItem;
                Inventory.Instance.ItemAmounts[slotID] = holdedItemAmount;
            }
            
            Message.Raise(new InventoryChangeEvent());
        }

        public void OnButtonEnter()
        {
            if (GameManager.Instance.InInventory)
            {
                itemImage.DOFade(0.5f, 0.3f);
                if (holdedItemAmount <= 0 || holdedItem == null)
                {
                    itemImage.sprite = hoverSprite;
                }
            }
        }

        public void OnButtonExit()
        {
            if (GameManager.Instance.InInventory)
            {
                itemImage.DOFade(1f, 0.3f);
                if (holdedItemAmount <= 0 || holdedItem == null)
                {
                    itemImage.sprite = freeSlotImage;
                }
            }
        }

        private void OnInventoryChangeEvent(InventoryChangeEvent ctx)
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

            if (holdedItemAmount <= 0 || holdedItem == null)
            {
                itemAmountText.text = "";
            }
        }
    }
}