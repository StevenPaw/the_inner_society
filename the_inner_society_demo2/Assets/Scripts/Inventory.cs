using System.Collections.Generic;
using DG.Tweening;
using farmingsim.EventSystem;
using UnityEngine;

namespace farmingsim
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private int hotbarSize;
        [SerializeField] private int maxAccessibleSlots;
        [SerializeField] private List<ScriptableObject> itemObjects;
        [SerializeField] private int currentlyActiveSlot;
        [SerializeField] private Transform slotsHolderInventory;
        [SerializeField] private Transform slotsHolderHotbar;
        [SerializeField] private GameObject itemSlotPrefab;
        [SerializeField] private ItemSlot[] slots;
        private static Inventory instance;
        
        private List<IItem> items;
        private List<int> itemAmounts;

        public static Inventory Instance => instance;

        public List<ScriptableObject> ItemObjects
        {
            get => itemObjects;
            set => itemObjects = value;
        }

        public List<IItem> Items
        {
            get => items;
            set => items = value;
        }

        public List<int> ItemAmounts
        {
            get => itemAmounts;
            set => itemAmounts = value;
        }

        public int CurrentlyActiveSlot
        {
            get => currentlyActiveSlot;
            set => currentlyActiveSlot = value;
        }

        public int MaxAccessibleSlots
        {
            get => maxAccessibleSlots;
            set => maxAccessibleSlots = value;
        }

        private void OnValidate()
        {
            for (int i = 0; i < maxAccessibleSlots; i++)
            {
                if (itemObjects.Count > i)
                {
                    if (itemObjects[i] != null)
                    {
                        if (itemObjects[i] is IItem)
                        {
                        }
                        else
                        {
                            itemObjects[i] = null;
                        }
                    }
                }
                else
                {
                    itemObjects.Add(null);
                }
            }

            if (itemObjects.Count > maxAccessibleSlots)
            {
                for (int i = itemObjects.Count - 1; i >= maxAccessibleSlots; i--)
                {
                    itemObjects.Remove(itemObjects[i]);
                }
            }
        }
        
        private void Awake()
        {
            if (Instance == null || Instance == this)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

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
            slots = new ItemSlot[maxAccessibleSlots];
            for (int i = 0; i < maxAccessibleSlots; i++)
            {
                if (i < hotbarSize)
                {
                    ItemSlot slot = Instantiate(itemSlotPrefab, slotsHolderHotbar).GetComponent<ItemSlot>();
                    slot.HoldedItem = itemObjects[i] as IItem;
                    slot.SlotID = i;
                    slots[i] = slot;
                }
                else
                {
                    ItemSlot slot = Instantiate(itemSlotPrefab, slotsHolderInventory).GetComponent<ItemSlot>();
                    slot.HoldedItem = itemObjects[i] as IItem;
                    slot.SlotID = i;
                    slots[i] = slot;
                }
            }

            items = new List<IItem>();
            itemAmounts = new List<int>();

            foreach (ScriptableObject obj in itemObjects)
            {
                if (obj != null)
                {
                    items.Add(obj as IItem);
                    itemAmounts.Add(1);
                }
                else
                {
                    items.Add(null);
                    itemAmounts.Add(0);
                }
            }
            Message.Raise(new InventoryChangeEvent());
        }

        private void OnToggleInventoryEvent(ToggleInventoryEvent ctx)
        {
            if (GameManager.Instance.InInventory)
            {
                slotsHolderInventory.gameObject.SetActive(true);
            }
            else
            {
                slotsHolderInventory.gameObject.SetActive(false);
            }
        }

        public void ChangeActiveSlot(int direction)
        {
            currentlyActiveSlot -= direction;
            if (currentlyActiveSlot >= hotbarSize)
            {
                currentlyActiveSlot -= hotbarSize;
            }
            else if(currentlyActiveSlot < 0)
            {
                currentlyActiveSlot += hotbarSize;
            }

            if (items[CurrentlyActiveSlot] is ITool)
            {
                PlayerController.Instance.ToolRenderer.sprite = items[CurrentlyActiveSlot].GetInventoryIcon();
                PlayerController.Instance.ToolRenderer.DOFade(1f, 0.2f);
            }
            else
            {
                PlayerController.Instance.ToolRenderer.DOFade(0f, 0.2f);
            }
            
            Message.Raise(new InventoryChangeEvent());
        }

        public bool AddItemToInventory(IItem collectedItem, int collectedAmount)
        {
            for (int i = 0; i < maxAccessibleSlots; i++)
            {
                if (items[i] != null)
                {
                    if (items[i].GetName() == collectedItem.GetName())
                    {
                        itemAmounts[i] += collectedAmount;
                        slots[i].HoldedItemAmount = itemAmounts[i];
                        slots[i].HoldedItem = items[i];
                        Message.Raise(new InventoryChangeEvent());
                        return true;
                    }
                }
            }
            
            for (int i = 0; i < maxAccessibleSlots; i++)
            {
                if (items[i] == null)
                {
                    items[i] = collectedItem;
                    itemAmounts[i] = collectedAmount;
                    slots[i].HoldedItemAmount = itemAmounts[i];
                    slots[i].HoldedItem = items[i];
                    Message.Raise(new InventoryChangeEvent());
                    return true;
                }
            }

            Message.Raise(new InventoryChangeEvent());
            return false;
        }

        public void RemoveItemFromInventory(int slotID, int usedAmount)
        {
            itemAmounts[slotID] -= usedAmount;
            if (itemAmounts[slotID] <= 0)
            {
                items[slotID] = null;
            }
            slots[slotID].HoldedItemAmount = itemAmounts[slotID];
            slots[slotID].HoldedItem = items[slotID];
            
            Message.Raise(new InventoryChangeEvent());
        }
        
        public void RemoveItemFromInventory(IItem itemType, int usedAmount)
        {
            int removedAmount = 0;
            for (int i = 0; i < maxAccessibleSlots; i++)
            {
                if (removedAmount < usedAmount)
                {
                    if (items[i] == itemType)
                    {
                        if (itemAmounts[i] - (usedAmount - removedAmount) <= 0)
                        {
                            removedAmount += itemAmounts[i];
                            items[i] = null;
                            itemAmounts[i] = 0;
                            slots[i].HoldedItemAmount = itemAmounts[i];
                            slots[i].HoldedItem = items[i];
                        }
                        else
                        {
                            int oldRemovedAmount = removedAmount;
                            removedAmount += (itemAmounts[i] - removedAmount);
                            itemAmounts[i] -= (usedAmount - oldRemovedAmount);
                            slots[i].HoldedItemAmount = itemAmounts[i];
                        }
                    }
                }
                else
                {
                    break;
                }
            }
            
            Message.Raise(new InventoryChangeEvent());
        }

        public int GetAmountOfItemInInventory(IItem item)
        {
            
            int amount = 0;
            for(int i = 0; i < maxAccessibleSlots; i++)
            {
                if (items[i] != null && items[i] == item)
                {
                    amount += ItemAmounts[i];
                }
            }
            return amount;
        }
    }
}