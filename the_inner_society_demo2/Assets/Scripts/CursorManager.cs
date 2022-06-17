using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace farmingsim
{
    public class CursorManager : MonoBehaviour
    {
        [SerializeField] private GameObject playerObject;
        [SerializeField] private SpriteRenderer cursorIconRenderer;
        [SerializeField] private SpriteRenderer cursorItemHoldRenderer;
        private IItem itemInCursor;
        private int itemInCursorAmount;
        private Camera mainCamera;
        private Vector2 mousePosition;
        [SerializeField] private float maxCursorDistance;

        public IItem ItemInCursor
        {
            get => itemInCursor;
            set => itemInCursor = value;
        }

        public int ItemInCursorAmount
        {
            get => itemInCursorAmount;
            set => itemInCursorAmount = value;
        }

        public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            mainCamera = Camera.main;
        }

        private void Update()
        {
            UpdatePosition();

            if (itemInCursor != null)
            {
                cursorItemHoldRenderer.sprite = itemInCursor.GetInventoryIcon();
            }
            else
            {
                cursorItemHoldRenderer.sprite = null;
            }
        }

        private void UpdatePosition()
        {
            Vector3 playerPosition = playerObject.transform.position;
            Vector2 newCursorPosition;
        
            if (GameManager.Instance.InInventory)
            {
                cursorItemHoldRenderer.DOFade(1f, 0.5f);
                newCursorPosition = mainCamera.ScreenToWorldPoint(mousePosition);
                gameObject.transform.position =
                    new Vector3(newCursorPosition.x, newCursorPosition.y, -10);
            }
            else
            {
                cursorItemHoldRenderer.DOFade(0f, 0.5f);
                if (Vector2.Distance(mainCamera.ScreenToWorldPoint(mousePosition), playerPosition) < maxCursorDistance)
                {
                    newCursorPosition = mainCamera.ScreenToWorldPoint(mousePosition);
                    gameObject.transform.position =
                        new Vector3(newCursorPosition.x, newCursorPosition.y, -10);
                }
                else
                {
                    Vector2 directionToMove = (mainCamera.ScreenToWorldPoint(mousePosition) - playerPosition);
                    directionToMove.Normalize();

                    newCursorPosition = (Vector2) playerPosition + new Vector2(directionToMove.x * maxCursorDistance,
                        directionToMove.y * maxCursorDistance);
                    gameObject.transform.position = new Vector3(newCursorPosition.x, newCursorPosition.y, -15);
                }
            }
        }

        public void OnMouseMovement(InputAction.CallbackContext ctx)
        {
            mousePosition = ctx.ReadValue<Vector2>();
        }
    }
}