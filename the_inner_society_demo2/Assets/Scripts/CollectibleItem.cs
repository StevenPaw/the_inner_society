using System;
using System.Diagnostics;
using farmingsim.EventSystem;
using farmingsim.Utils;
using UnityEngine;

namespace farmingsim
{
    public class CollectibleItem : MonoBehaviour
    {
        [SerializeField] private ScriptableObject itemScriptableObject;
        [SerializeField] private int itemAmount;
        [SerializeField] private SpriteRenderer spriteRenderer;
        private IItem item;

        private void OnValidate()
        {
            IItem test = item as IItem;
            if (test == null)
            {
                item = null;
            }
        }

        private void OnEnable()
        {
            item = itemScriptableObject as IItem; 
            spriteRenderer.sprite = item.GetInventoryIcon();
        }
        
        private void Start()
        {
            item = itemScriptableObject as IItem;
            spriteRenderer.sprite = item.GetInventoryIcon();
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag(GameTags.PLAYER))
            {
                if (Inventory.Instance.AddItemToInventory(item, itemAmount))
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}