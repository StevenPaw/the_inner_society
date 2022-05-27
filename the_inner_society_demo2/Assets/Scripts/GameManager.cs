using System;
using UnityEngine;

namespace farmingsim
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameObject fieldPrefab;
        private IUsable activeUseObject;
        
        private static GameManager instance;
        

        public static GameManager Instance => instance;
        public GameObject FieldPrefab => fieldPrefab;
        public IUsable ActiveUseObject => activeUseObject;

        private void Awake()
        {
            instance = this;
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