using System;
using farmingsim;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject playerObject;
    [SerializeField] private GameObject cursorObject;
    [SerializeField] float movementSpeed;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float maxCursorDistance;
    private Vector2 movement;
    private Rigidbody2D rbody;
    private Vector2 mousePosition;
    

    private static PlayerController instance;

    public static PlayerController Instance => instance;
 
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        rbody = playerObject.GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Movement();
    }

    private void Update()
    {
        Vector3 playerPosition = playerObject.transform.position;
        Vector2 newCursorPosition;
        Debug.Log("Mouse: " + mainCamera.ScreenToWorldPoint(mousePosition) + " | Player: " + playerPosition);
        if (Vector2.Distance(mainCamera.ScreenToWorldPoint(mousePosition), playerPosition) < maxCursorDistance)
        {
            newCursorPosition = mainCamera.ScreenToWorldPoint(mousePosition);
            cursorObject.transform.position =
                new Vector3(newCursorPosition.x, newCursorPosition.y, -10);
        }
        else
        {
            //(playerpos - cursorpos) : (cursorpos - maxdis)
            Vector2 val1 = playerPosition - mainCamera.ScreenToWorldPoint(mousePosition);
            Vector2 val2 = mainCamera.ScreenToWorldPoint(mousePosition) -
                           new Vector3(maxCursorDistance, maxCursorDistance, 1);
            newCursorPosition = val1 / val2;
            
            //Vector2 directionToMove = (mainCamera.ScreenToWorldPoint(mousePosition) - playerPosition);
            //newCursorPosition = (Vector2)playerPosition + (directionToMove * maxCursorDistance);
            cursorObject.transform.position = new Vector3(newCursorPosition.x, newCursorPosition.y, -10);
        }
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            movement = ctx.ReadValue<Vector2>();
            Debug.Log("Test");
        }

        if (ctx.canceled)
        {
            movement = Vector2.zero;
        }
    }

    public void OnInteract(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            if (GameManager.Instance.ActiveUseObject != null && Inventory.Instance.Item != null)
            {
                if (GameManager.Instance.ActiveUseObject.GetUsableType() == UsableTypes.FARMFIELD)
                {
                    if (Inventory.Instance.Item is IPlantable)
                    {
                        FieldTile plantable = (FieldTile) GameManager.Instance.ActiveUseObject;
                        plantable.SetPlantedPlant(Inventory.Instance.Item as IPlantable);
                    }
                }
            }
        }
    }

    public void OnMouseMovement(InputAction.CallbackContext ctx)
    {
        mousePosition = ctx.ReadValue<Vector2>();
    }
    
    public void Movement()
    {
        Vector2 currentPos = rbody.position;
 
        Vector2 adjustedMovement = movement * movementSpeed;
 
        Vector2 newPos = currentPos + adjustedMovement * Time.fixedDeltaTime;
 
        rbody.MovePosition(newPos);
    }
}
