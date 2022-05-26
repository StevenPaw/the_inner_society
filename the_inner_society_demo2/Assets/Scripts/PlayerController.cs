using System.Collections;
using System.Collections.Generic;
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
    
    public void Movement()
    {
        Vector2 currentPos = rbody.position;
 
        Vector2 adjustedMovement = movement * movementSpeed;
 
        Vector2 newPos = currentPos + adjustedMovement * Time.fixedDeltaTime;
 
        rbody.MovePosition(newPos);
    }
}
