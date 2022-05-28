using System;
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
        private int currentGrowthStep;
        private bool isReady;
        private IPlantable plantedPlant;

        private bool inUse = false;

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
            }
            else
            {
                plantRenderer.gameObject.SetActive(true);
                if (plantRenderer.sprite == plantedPlant.GetSprites()[currentGrowthStep])
                {
                    plantRenderer.sprite = plantedPlant.GetSprites()[currentGrowthStep];
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("Area entered!");
            if (Vector2.Distance(PlayerController.Instance.gameObject.transform.position,
                gameObject.transform.position) < 30)
            {
                GameManager.Instance.SetUsedObject(this);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            Debug.Log("Area leaved!");
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
        
        public void SetPlantedPlant(IPlantable plant)
        {
            plantedPlant = plant;
        }
    }
        
}
