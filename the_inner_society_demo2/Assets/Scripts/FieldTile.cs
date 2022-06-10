using System;
using farmingsim.Utils;
using UnityEngine;

namespace farmingsim
{
    public class FieldTile : MonoBehaviour, IUsable
    {
        [SerializeField] private SpriteRenderer farmFieldRenderer;
        [SerializeField] private SpriteRenderer plantRenderer;
        [SerializeField] private SpriteRenderer selectIndicatorRenderer;
        [SerializeField] private Sprite farmFieldReadySprite;
        [SerializeField] private Sprite farmFieldNotReadySprite;
        [SerializeField] private int currentGrowthStep;
        [SerializeField] private bool isReady;
        private IPlantable plantedPlant;
        private float growedTime;

        private bool inUse = false;

        public bool IsReady
        {
            get => isReady;
            set => isReady = value;
        }

        private void Update()
        {
            if (inUse)
            {
                var color = selectIndicatorRenderer.color;
                color = new Color(color.r, color.g, color.b, 1);
                selectIndicatorRenderer.color = color;
            }
            else
            {
                var color = selectIndicatorRenderer.color;
                color = new Color(color.r, color.g, color.b, 0);
                selectIndicatorRenderer.color = color;
            }

            if (plantedPlant == null)
            {
                plantRenderer.gameObject.SetActive(false);
                growedTime = 0;
                currentGrowthStep = 0;
            }
            else
            {
                Debug.Log("Planted Plant: " + plantedPlant.GetName());
                plantRenderer.gameObject.SetActive(true);
                if (plantRenderer.sprite != plantedPlant.GetSprites()[currentGrowthStep])
                {
                    plantRenderer.sprite = plantedPlant.GetSprites()[currentGrowthStep];
                }
                
                growedTime += Time.deltaTime;
                if (growedTime > plantedPlant.GetGrowthDuration() && currentGrowthStep < plantedPlant.GetSprites().Length - 1)
                {
                    growedTime = 0;
                    currentGrowthStep += 1;
                }
            }

            
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(GameTags.CURSOR))
            {
                if (Vector2.Distance(PlayerController.Instance.gameObject.transform.position,
                        gameObject.transform.position) < 30)
                {
                    GameManager.Instance.SetUsedObject(this);
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag(GameTags.CURSOR))
            {
                GameManager.Instance.DeactivateUsedObject(this);
            }
        }

        public void SetUsed(bool isUsed)
        {
            inUse = isUsed;
        }

        public UsableTypes GetUsableType()
        {
            return UsableTypes.FARMFIELD;
        }
        
        public void SetPlantedPlant(IPlantable plant)
        {
            plantedPlant = plant;
        }

        public void DestroyPlantedPlant()
        {
            plantedPlant = null;
        }
    }
        
}
