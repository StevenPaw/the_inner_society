using UnityEngine;

namespace farmingsim
{
    public class Field : MonoBehaviour
    {
        [SerializeField] private bool farmActive;
        private void Start()
        {
            for (int x = 0; x < 5; x++)
            {
                for (int y = 0; y < 5; y++)
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
