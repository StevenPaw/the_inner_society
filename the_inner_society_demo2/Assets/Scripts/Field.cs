using UnityEngine;

namespace farmingsim
{
    public class Field : MonoBehaviour
    {
        [SerializeField] private bool farmActive;
        [SerializeField] private IPlantable plantedPlant;
        [SerializeField] private Vector2 fieldSize;

        private void Start()
        {
            PopulateFarm();
        }

        private void PopulateFarm()
        {
            if (farmActive)
            {
                for (int x = 0; x < fieldSize.x; x++)
                {
                    for (int y = 0; y < fieldSize.y; y++)
                    {
                        Vector2 transformPosition = transform.position;
                        Vector2 transformOffset = new Vector2(x, y);
                        Vector2 transformAdd = transformPosition + transformOffset;
                        Instantiate(GameManager.Instance.FieldPrefab, transformAdd, Quaternion.identity, transform);
                    }
                }
            }
        }
    }
}
