using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;

namespace farmingsim
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private int hotbarSize;
        [SerializeField] private int maxAccessibleSlots;
        [SerializeField] private List<ScriptableObject> itemObjects;
        [SerializeField] private List<IItem> items;
        [SerializeField] private int currentlyActiveSlot;
        [SerializeField] private Transform slotsHolderInventory;
        [SerializeField] private Transform slotsHolderHotbar;
        [SerializeField] private GameObject itemSlotPrefab;
        [SerializeField] private ItemSlot[] slots;
        private static Inventory instance;

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

            foreach (ScriptableObject obj in itemObjects)
            {
                if (obj != null)
                {
                    items.Add(obj as IItem);
                }
                else
                {
                    items.Add(null);
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
        }
    }
}