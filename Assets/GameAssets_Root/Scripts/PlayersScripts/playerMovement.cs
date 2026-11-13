using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;


public class playerMovement : MonoBehaviour
{

    private CharacterController Controller;
    private playerCameraLook playerCameraLook;
    private Vector3 playerSpeed;
    public Transform playerCam;
    private playerHealth playerHealth;
    [SerializeField] private Animator animator;
    [SerializeField] private float playerAccelerationRate;
    [SerializeField] private float playerStaminaGainRate;
    [SerializeField] private float playerStaminaDrainRate;
    private playerInputManager playerInputManager;
   

    //player controller values
    private int crouchHeight = 1;
    private int standingHeight = 2;
    private float velocity;
    private float WalkVelocity = 5f; 
    private float RunVelocity = 8f;
    private float jumpForce = 1f;
    private float crouchTimer = 1;
    private float standingCamY = 0f;
    private float playerTargetSpeed; 
    private bool isGrounded;
    public bool isSprinting = false;
    private bool isCrouching;
    private bool lerpCrouch;
    private float moveSpeed;
    private bool isCursorLocked; //defaults the cursor to be locked when the game loads

    public float cameraHeightDelta = 0.7f;
    public float gravityValue = -9.81f;


    



    void Start()
    {
        Controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        playerHealth = GetComponent<playerHealth>();
        playerInputManager = GetComponent<playerInputManager>();
        //sets to cursor to be locked when the game starts 
        isCursorLocked = true;
        Cursor.lockState = CursorLockMode.Locked;

    }
  
    void Update()
    {
        
        //ensures that the player is grounded during the crouch animation
        isGrounded = Controller.isGrounded;
        //Crouch Animation and/or mechanic    
        if (lerpCrouch)
        {
            crouchTimer += Time.deltaTime;
            float crouchProgress = crouchTimer / 1;
            crouchProgress *= crouchProgress;
            if (isCrouching)
            {
                Controller.height = Mathf.Lerp(Controller.height, crouchHeight, crouchProgress);
                //changes the camera height to match the crouch height 
                playerCam.transform.localPosition = Vector3.Lerp(
                    playerCam.transform.localPosition, new Vector3(0f, standingCamY - cameraHeightDelta, 0f),
                    crouchProgress
                    );
            }
            else
            {
                Controller.height = Mathf.Lerp(Controller.height, standingHeight, crouchProgress);
                //reverts camera height change to match standing height
                playerCam.transform.localPosition = Vector3.Lerp(
                    playerCam.transform.localPosition, new Vector3(0f, standingCamY, 0f),
                    crouchProgress
                    );
            }

            //Smooths camera interpolation to become smooth instead of jittery  
            Controller.center = new Vector3(0f, Controller.height / 2f, 0f);

            if (crouchProgress > 1)
            {
                lerpCrouch = false;
                crouchTimer = 0f;
            }
        }
        playerSprint();
    }

   
    //Take Input from PlayerInputManager.cs and apply to character controller

    public void movementProcess(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;
        Controller.Move(transform.TransformDirection(moveDirection) * velocity * Time.deltaTime);
        playerSpeed.y += gravityValue * Time.deltaTime;

        if (isGrounded && playerSpeed.y < 0)
        {
            playerSpeed.y = -2;
        }

        Controller.Move(playerSpeed * Time.deltaTime);

        playerTargetSpeed = input.magnitude * velocity;

        moveSpeed = Mathf.MoveTowards(moveSpeed, playerTargetSpeed, playerAccelerationRate * Time.deltaTime);

        animator.SetFloat("Velocity", moveSpeed);
        

    }
    
    public void playerJump()
    {
        //checks if the player is grounded, if true allows the player to jump 
        if (isGrounded)
        {
            playerSpeed.y = Mathf.Sqrt(jumpForce * gravityValue * -2.0f);
        }
    }

    public void setSprint(bool isSprint)
    {
        isSprinting = isSprint;
    }

    public void playerSprint()
    {
        //Player's sprint function, changes player speed if sprinting = true 
        if (isSprinting)
        {
            velocity = RunVelocity;
            playerHealth.StaminaDrain(playerStaminaDrainRate);
            isSprinting = (Mathf.Round(playerHealth.stamina) <= 0) ? false : true;
        }
        else
        {
            velocity = WalkVelocity;
            playerHealth.StaminaGain(playerStaminaGainRate);
        }

    }

    //Lets the player crouch 
    public void playerCrouch()
    {
        isCrouching = !isCrouching;
        crouchTimer = 0;
        lerpCrouch = true;

    }    
    //Locks the cursor 
    public void lockCursor()
    {
        if (!isCursorLocked && Cursor.lockState == CursorLockMode.None) {

            Cursor.lockState = CursorLockMode.Locked;
            isCursorLocked = !isCursorLocked;
            Debug.Log(isCursorLocked);
        }
        else if (isCursorLocked && Cursor.lockState == CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.None;
            isCursorLocked = false;
            Debug.Log(isCursorLocked);
        }
    }
}
