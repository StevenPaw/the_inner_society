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
        private IPlantable plantedSeed;
        private float growedTime;
        private bool isFree = true;
        private bool canBeHarvested;

        private bool inUse = false;
        public bool IsFree => isFree;

        public bool IsReady
        {
            get => isReady;
            set => isReady = value;
        }

        public bool CanBeHarvested => canBeHarvested;

        private void Update()
        {
            if (inUse && !GameManager.Instance.InInventory)
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

            if (isReady)
            {
                farmFieldRenderer.sprite = farmFieldReadySprite;
            }
            else
            {
                farmFieldRenderer.sprite = farmFieldNotReadySprite;
            }

            if (plantedSeed == null)
            {
                plantRenderer.gameObject.SetActive(false);
                growedTime = 0;
                currentGrowthStep = 0;
                canBeHarvested = false;
                isFree = true;
            }
            else
            {
                isFree = false;
                plantRenderer.gameObject.SetActive(true);
                if (plantRenderer.sprite != plantedSeed.GetSprites()[currentGrowthStep])
                {
                    plantRenderer.sprite = plantedSeed.GetSprites()[currentGrowthStep];
                }
                
                growedTime += Time.deltaTime;
                if (growedTime > plantedSeed.GetGrowthDuration() && currentGrowthStep < plantedSeed.GetSprites().Length - 1)
                {
                    growedTime = 0;
                    currentGrowthStep += 1;
                    canBeHarvested = false;
                } else if (currentGrowthStep < plantedSeed.GetSprites().Length - 1)
                {
                    canBeHarvested = false;
                }
                else if(currentGrowthStep >= plantedSeed.GetSprites().Length - 1)
                {
                    canBeHarvested = true;
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
            plantedSeed = plant;
        }

        public IItem[] DestroyPlantedPlant()
        {
            Debug.Log("Destroying!");
            IItem[] items = new IItem[1];
            items[0] = plantedSeed as IItem;
            plantedSeed = null;
            return items;
        }
        
        public IItem[] HarvestPlantedPlant()
        {
            Debug.Log("Harvesting!");
            IItem[] items = plantedSeed.GetHarvestedItems();
            plantedSeed = null;
            return items;
        }
    }
        
}
