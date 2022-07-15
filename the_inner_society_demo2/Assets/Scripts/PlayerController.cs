using System;
using DG.Tweening;
using farmingsim;
using farmingsim.EventSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject playerObject;
    [SerializeField] private CursorManager cursorManager;
    [SerializeField] float movementSpeed;
    [SerializeField] private Image blackPanel;
    [SerializeField] private SpriteRenderer toolRenderer;
    [SerializeField] private Animator playerAnimator;
    private Vector2 movement;
    private Rigidbody2D rbody;
    private Vector2 mousePosition;

    private static PlayerController instance;

    public static PlayerController Instance => instance;
    public CursorManager CursorManager => cursorManager;
    public SpriteRenderer ToolRenderer => toolRenderer;

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
        cursorManager.OnSceneLoaded(scene, mode);
        blackPanel.DOFade(0f, 0.5f);
    }

    private void Start()
    {
        rbody = playerObject.GetComponent<Rigidbody2D>();
        Message.Raise(new ToggleInventoryEvent());
    }

    void FixedUpdate()
    {
        Movement();
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
        if (!GameManager.Instance.InInventory)
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
                            if (plantable.IsReady && plantable.IsFree)
                            {
                                plantable.SetPlantedPlant(
                                    Inventory.Instance.Items[Inventory.Instance.CurrentlyActiveSlot] as IPlantable);
                                Inventory.Instance.RemoveItemFromInventory(Inventory.Instance.CurrentlyActiveSlot, 1);
                            }
                        }
                        else if (Inventory.Instance.Items[Inventory.Instance.CurrentlyActiveSlot] is ITool)
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
        if (!GameManager.Instance.InInventory)
        {
            if (ctx.started)
            {
                if (GameManager.Instance.ActiveUseObject != null)
                {
                    if (GameManager.Instance.ActiveUseObject.GetUsableType() == UsableTypes.FARMFIELD)
                    {
                        FieldTile fieldTile = (FieldTile) GameManager.Instance.ActiveUseObject;
                        if (!fieldTile.IsFree)
                        {
                            if (fieldTile.CanBeHarvested)
                            {
                                IItem[] collectedItems = fieldTile.HarvestPlantedPlant();
                                if (collectedItems != null)
                                {
                                    foreach (IItem item in collectedItems)
                                    {
                                        Inventory.Instance.AddItemToInventory(item, 1);
                                    }
                                }
                            }
                            else
                            {
                                IItem[] collectedItems = fieldTile.DestroyPlantedPlant();
                                if (collectedItems != null)
                                {
                                    foreach (IItem item in collectedItems)
                                    {
                                        Inventory.Instance.AddItemToInventory(item, 1);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public void OnMouseMovement(InputAction.CallbackContext ctx)
    {
        cursorManager.OnMouseMovement(ctx);
    }

    public void OnScroll(InputAction.CallbackContext ctx)
    {
        if (!GameManager.Instance.InInventory)
        {
            if (ctx.started)
            {
                float scrollDirection = 0;
                scrollDirection += ctx.ReadValue<Vector2>().normalized.y;
                Inventory.Instance.ChangeActiveSlot((int) scrollDirection);
            }
        }
    }

    public void OnInventory(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            GameManager.Instance.InInventory = !GameManager.Instance.InInventory;
            Message.Raise(new ToggleInventoryEvent());
        }
    }

    public void Movement()
    {
        Vector2 currentPos = rbody.position;
 
        playerAnimator.SetFloat("MovementX", movement.x);
        playerAnimator.SetFloat("MovementY", -movement.y);
        
        Vector2 adjustedMovement = movement * movementSpeed;

        Vector2 newPos = currentPos + adjustedMovement * Time.fixedDeltaTime;
 
        rbody.MovePosition(newPos);
    }

    public void FadeToBlack(float duration = 0.5f)
    {
        blackPanel.DOFade(1f, duration);
    }
}
