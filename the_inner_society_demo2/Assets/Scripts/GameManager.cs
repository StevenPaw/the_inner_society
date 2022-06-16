using System;
using UnityEngine;

namespace farmingsim
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameObject fieldPrefab;
        private IUsable activeUseObject;
        private bool inInventory;
        
        private static GameManager instance;
        public static GameManager Instance => instance;
        public GameObject FieldPrefab => fieldPrefab;
        public IUsable ActiveUseObject => activeUseObject;

        public bool InInventory
        {
            get => inInventory;
            set => inInventory = value;
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

        public void SetUsedObject(IUsable usedTile)
        {
            if (activeUseObject != null)
            {
                activeUseObject.SetUsed(false);
            }

            activeUseObject = usedTile;
            usedTile.SetUsed(true);
        }

        public void DeactivateUsedObject(IUsable usedTile)
        {
            if (activeUseObject == usedTile)
            {
                usedTile.SetUsed(false);
                activeUseObject = null;
            }
        }
    }
}