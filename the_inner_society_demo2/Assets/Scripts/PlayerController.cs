using System.Collections;
using System.Collections.Generic;
using farmingsim;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float movementSpeed;
    private Vector2 movement;
    private Rigidbody2D rbody;
 
    private void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
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
    
    public void Movement()
    {
        Vector2 currentPos = rbody.position;
 
        Vector2 adjustedMovement = movement * movementSpeed;
 
        Vector2 newPos = currentPos + adjustedMovement * Time.fixedDeltaTime;
 
        rbody.MovePosition(newPos);
    }
}
