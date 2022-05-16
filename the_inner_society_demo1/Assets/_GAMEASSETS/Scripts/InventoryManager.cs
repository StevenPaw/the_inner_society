using System;
using System.Collections.Generic;
using InnerSociety.Events;
using UnityEngine;

namespace InnerSociety
{
    public class InventoryManager : MonoBehaviour
    {
        [SerializeField] private Transform itemHolder;
        [SerializeField] private GameObject itemPrefab;
        private List<Item> collectedItems;
        private static InventoryManager instance;

        public static InventoryManager Instance => instance;

        private void OnEnable()
        {
            Message<CollectItemEvent>.Add(OnCollectItemEvent);
            collectedItems = new List<Item>();
            if (instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
            }
        }
        
        private void OnDisable()
        {
            Message<CollectItemEvent>.Remove(OnCollectItemEvent);
        }

        private void OnCollectItemEvent(CollectItemEvent ctx)
        {
            collectedItems.Add(ctx.ItemToCollect);
            UpdateInventory();
        }

        private void UpdateInventory()
        {
            foreach (Transform child in itemHolder)
            {
                Destroy(child.gameObject);
            }
                
            if (collectedItems != null)
            {
                foreach (Item i in collectedItems)
                {
                    InventoryItem ii = Instantiate(itemPrefab, itemHolder).GetComponent<InventoryItem>();
                    ii.SetItem(i);
                }
            }
        }

        public bool HasItem(Item searchedItem)
        {
            foreach (Item i in collectedItems)
            {
                if (i.ItemName == searchedItem.ItemName)
                {
                    return true;
                }
            }

            return false;
        }
    }
}