using System;
using UnityEngine;

namespace farmingsim
{
    public class FieldTile : MonoBehaviour, IUsable
    {
        [SerializeField] private Collider2D collider;
        [SerializeField] private SpriteRenderer renderer;
        private bool inUse = false;

        private void Start()
        {
            collider = GetComponent<Collider2D>();
            renderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            if (inUse)
            {
                renderer.color = Color.black;
            }
            else
            {
                renderer.color = Color.red;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            GameManager.Instance.SetUsedObject(this);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            GameManager.Instance.DeactivateUsedObject(this);
        }

        public void SetUsed(bool isUsed)
        {
            inUse = isUsed;
        }

        public UsableTypes GetUsableType()
        {
            return UsableTypes.FARMFIELD;
        }
    }
        
}
