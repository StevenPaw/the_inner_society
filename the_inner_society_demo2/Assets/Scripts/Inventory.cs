using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using DG.Tweening;
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
        }

        private void Update()
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
        }

        public void AddItemToInventory(IItem collectedItem, int collectedAmount)
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
                        return;
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
                    return;
                }
            }
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
        }
    }
}