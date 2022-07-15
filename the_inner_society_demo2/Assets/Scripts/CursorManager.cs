using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace farmingsim
{
    public class CursorManager : MonoBehaviour
    {
        [SerializeField] private GameObject playerObject;
        [SerializeField] private SpriteRenderer cursorIconRenderer;
        [SerializeField] private RectTransform cursorItemHolder;
        [SerializeField] private Sprite cursorItemHolderEmpty;
        private Image cursorItemImage;
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

        private void Start()
        {
            cursorItemImage = cursorItemHolder.GetComponent<Image>();
        }

        private void Update()
        {
            UpdatePosition();
            RotateToPlayer();

            if (itemInCursor != null)
            {
                //cursorItemHoldRenderer.sprite = itemInCursor.GetInventoryIcon();
                cursorItemImage.sprite = itemInCursor.GetInventoryIcon();
            }
            else
            {
                cursorItemHoldRenderer.sprite = null;
                cursorItemImage.sprite = cursorItemHolderEmpty;
            }
        }

        private void RotateToPlayer()
        {
            if (!GameManager.Instance.InInventory)
            {
                Vector3 targ = playerObject.transform.position;
                Vector3 objectPos = cursorIconRenderer.gameObject.transform.position;
                targ.x -= objectPos.x;
                targ.y -= objectPos.y;
                
                float angle = Mathf.Atan2(targ.y, targ.x) * Mathf.Rad2Deg + 45;
                cursorIconRenderer.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            }
            else
            {
                cursorIconRenderer.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0,0,0));
            }
        }

        private void UpdatePosition()
        {
            Vector3 playerPosition = playerObject.transform.position;
            Vector2 newCursorPosition;
        
            if (GameManager.Instance.InInventory)
            {
                cursorIconRenderer.DOFade(0f, 0.5f);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                cursorItemHoldRenderer.DOFade(1f, 0.5f);
                newCursorPosition = mainCamera.ScreenToWorldPoint(mousePosition);
                gameObject.transform.position =
                    new Vector3(newCursorPosition.x, newCursorPosition.y, -10);
            }
            else
            {
                cursorIconRenderer.DOFade(1f, 0.5f);
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = false;
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

            cursorItemHolder.position = mousePosition;
        }

        public void OnMouseMovement(InputAction.CallbackContext ctx)
        {
            mousePosition = ctx.ReadValue<Vector2>();
        }
    }
}