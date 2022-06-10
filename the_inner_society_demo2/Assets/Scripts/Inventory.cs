using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;

namespace farmingsim
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private List<ScriptableObject> items;
        [SerializeField] private int currentlyActiveSlot;
        [SerializeField] private int maxAccessibleSlots;
        [SerializeField] private Transform slotsHolderInventory;
        [SerializeField] private Transform slotsHolderHotbar;
        [SerializeField] private GameObject itemSlotPrefab;
        [SerializeField] private ItemSlot[] slots;
        private static Inventory instance;

        public static Inventory Instance => instance;

        public List<ScriptableObject> Items
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
                if (items.Count >= i)
                {
                    if (items[i] != null)
                    {
                        if (items[i] is IItem)
                        {
                        }
                        else
                        {
                            items[i] = null;
                        }
                    }
                }
                else
                {
                    items.Add(null);
                }
            }
            
            for (int i = 0; i < maxAccessibleSlots; i++)
            {
                if (items[i] != null)
                {
                    slots[i].HoldedItem = items[i] as IItem;
                }
                else
                {
                    slots[i].HoldedItem = null;
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
                return;
            }
            
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            slots = new ItemSlot[maxAccessibleSlots];
            for (int i = 0; i < maxAccessibleSlots; i++)
            {
                if (i < 9)
                {
                    ItemSlot slot = Instantiate(itemSlotPrefab, slotsHolderHotbar).GetComponent<ItemSlot>();
                    slot.HoldedItem = items[i] as IItem;
                    slot.SlotID = i;
                    slots[i] = slot;
                }
                else
                {
                    ItemSlot slot = Instantiate(itemSlotPrefab, slotsHolderInventory).GetComponent<ItemSlot>();
                    slot.HoldedItem = items[i] as IItem;
                    slot.SlotID = i;
                    slots[i] = slot;
                }
            }
        }

        public void ChangeActiveSlot(int direction)
        {
            currentlyActiveSlot -= direction;
            if (currentlyActiveSlot > maxAccessibleSlots)
            {
                currentlyActiveSlot -= maxAccessibleSlots;
            }
            else if(currentlyActiveSlot < 0)
            {
                currentlyActiveSlot += maxAccessibleSlots;
            }
        }
    }
}