using System;
using farmingsim;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        mainCamera = Camera.main;
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
        if (Vector2.Distance(mainCamera.ScreenToWorldPoint(mousePosition), playerPosition) < maxCursorDistance)
        {
            newCursorPosition = mainCamera.ScreenToWorldPoint(mousePosition);
            cursorObject.transform.position =
                new Vector3(newCursorPosition.x, newCursorPosition.y, -10);
        }
        else
        {
            Vector2 directionToMove = (mainCamera.ScreenToWorldPoint(mousePosition) - playerPosition);
            directionToMove.Normalize();
            
            newCursorPosition = (Vector2)playerPosition + new Vector2(directionToMove.x * maxCursorDistance, directionToMove.y * maxCursorDistance);
            cursorObject.transform.position = new Vector3(newCursorPosition.x, newCursorPosition.y, -15);
        }
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            movement = ctx.ReadValue<Vector2>();
        }

        if (ctx.canceled)
        {
            movement = Vector2.zero;
        }
    }

    public void OnBuildAction(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            if (GameManager.Instance.ActiveUseObject != null && Inventory.Instance.Items != null)
            {
                if (GameManager.Instance.ActiveUseObject.GetUsableType() == UsableTypes.FARMFIELD)
                {
                    if (Inventory.Instance.Items[Inventory.Instance.CurrentlyActiveSlot] is IPlantable)
                    {
                        FieldTile plantable = (FieldTile) GameManager.Instance.ActiveUseObject;
                        plantable.SetPlantedPlant(Inventory.Instance.Items[Inventory.Instance.CurrentlyActiveSlot] as IPlantable);
                    } 
                    else if (Inventory.Instance.Items[Inventory.Instance.CurrentlyActiveSlot] is IUsable)
                    {
                        if (Inventory.Instance.Items[Inventory.Instance.CurrentlyActiveSlot] is ITool)
                        {
                            ITool tool = Inventory.Instance.Items[Inventory.Instance.CurrentlyActiveSlot] as ITool;
                            tool.Use(GameManager.Instance.ActiveUseObject);
                        }
                    }
                }
            }
        }
    }

    public void OnDestroyAction(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            if (GameManager.Instance.ActiveUseObject != null)
            {
                if (GameManager.Instance.ActiveUseObject.GetUsableType() == UsableTypes.FARMFIELD)
                {
                    FieldTile plantable = (FieldTile) GameManager.Instance.ActiveUseObject;
                    plantable.DestroyPlantedPlant();
                }
            }
        }
    }

    public void OnMouseMovement(InputAction.CallbackContext ctx)
    {
        mousePosition = ctx.ReadValue<Vector2>();
    }

    public void OnScroll(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            float scrollDirection = 0;
            scrollDirection += ctx.ReadValue<Vector2>().normalized.y;
            Inventory.Instance.ChangeActiveSlot((int) scrollDirection);
        }
    }

    public void Movement()
    {
        Vector2 currentPos = rbody.position;
 
        Vector2 adjustedMovement = movement * movementSpeed;
 
        Vector2 newPos = currentPos + adjustedMovement * Time.fixedDeltaTime;
 
        rbody.MovePosition(newPos);
    }
}
