using System;
using System.ComponentModel.Design.Serialization;
using UnityEngine;

namespace farmingsim
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private ScriptableObject[] items;
        [SerializeField] private int currentlyActiveSlot;
        [SerializeField] private int maxAccessibleSlots;
        [SerializeField] private Transform slotsHolderInventory;
        [SerializeField] private Transform slotsHolderHotbar;
        [SerializeField] private GameObject itemSlotPrefab;
        [SerializeField] private ItemSlot[] slots;
        [SerializeField] private PlantablePlant placeHolderPlant;
        private static Inventory instance;

        public static Inventory Instance => instance;

        public ScriptableObject[] Items
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
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] is IItem)
                { }
                else
                {
                    items[i] = null;
                }
            }
            
            /*for (int i = 0; i < maxAccessibleSlots; i++)
            {
                slots[i].HoldedItem = items[i] as IItem;
            }*/
        }

        private void Start()
        {
            for (int i = 0; i < maxAccessibleSlots; i++)
            {
                if (i < 9)
                {
                    ItemSlot slot = Instantiate(itemSlotPrefab, slotsHolderHotbar).GetComponent<ItemSlot>();
                    slot.HoldedItem = items[i] as IItem;
                    slots[i] = slot;
                }
                else
                {
                    ItemSlot slot = Instantiate(itemSlotPrefab, slotsHolderInventory).GetComponent<ItemSlot>();
                    slot.HoldedItem = items[i] as IItem;
                    slots[i] = slot;
                }
            }
            
            if (placeHolderPlant != null)
            {
                Items[currentlyActiveSlot] = placeHolderPlant;
                slots[currentlyActiveSlot].HoldedItem = placeHolderPlant;
            }
        }

        private void Awake()
        {
            instance = this;
            slots = new ItemSlot[45];
            items = new ScriptableObject[45];
        }
    }
}