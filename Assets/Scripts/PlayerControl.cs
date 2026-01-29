using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class PlayerControl : MonoBehaviour
{
    public bool isAlive;
    public float jumpForce;
    public float probeLen;
    public bool grounded; // only public for debugging purposes
    public LayerMask WhatIsGround;
    public float walkSpeed;
    public float maxWalk;
    public float turnSpeed;
    private Vector2 moveInput;
    private Vector2 rotateInput;
    private Rigidbody rigi;
    private IA_PlayerInputs ctrl;
    private int keyCount;
    void jump(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            if (grounded)
            {
                rigi.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
    

    void Awake()
    {
        rigi = GetComponent<Rigidbody>();
        ctrl = new IA_PlayerInputs();
        ctrl.Enable();
        ctrl.Player.Jump.started += jump;
        isAlive = true;
        
    }






    private void FixedUpdate()
    {
       if(isAlive)
       {

       
        grounded = Physics.Raycast(this.transform.position, Vector3.down, probeLen, WhatIsGround);
        
        moveInput = ctrl.Player.Move.ReadValue<Vector2>();
        rotateInput = ctrl.Player.Rotate.ReadValue<Vector2>();


        if (rotateInput.magnitude > 0.1f)
        {
            Vector3 angleVelocity = new Vector3(0f, rotateInput.x * turnSpeed, 0f);
            Quaternion deltaRot = Quaternion.Euler(angleVelocity * Time.deltaTime);
            rigi.MoveRotation(rigi.rotation * deltaRot);
         
        }


        if (moveInput.magnitude > 0.1f)
        {
            Vector3 moveForward = moveInput.y * this.transform.forward;
            Vector3 moveRight = moveInput.x * this.transform.right;
            Vector3 moveVector = moveForward + moveRight;
            rigi.AddForce(moveVector * walkSpeed * Time.deltaTime);

            rigi.linearVelocity = Vector3.ClampMagnitude(rigi.linearVelocity, maxWalk);
        
        }
        }
        
       
    }
   
}
 