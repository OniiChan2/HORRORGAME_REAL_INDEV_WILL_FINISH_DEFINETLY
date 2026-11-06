using UnityEngine;

public class playerMovement : MonoBehaviour
{

    private CharacterController Controller;
    private Vector3 playerSpeed;
    public Camera playerCam; 

    //player controller values 
    private int crouchHeight = 1;
    private int standingHeight = 2; 
    private float velocity = 5f;
    private float jumpForce = 1f;
    private float crouchTimer = 1;
    private float standingCamY = 1.7f;
    private bool isGrounded;
    private bool isSprinting;
    private bool isCrouching ;
    private bool lerpCrouch;
    private bool isCursorLocked; //defaults the cursor to be locked when the game loads

    public float cameraHeightDelta = 0.7f;
    public float gravityValue = -9.81f;



    void Start()
    {
        Controller = GetComponent<CharacterController>();

        //sets to cursor to be locked when the game starts 
        isCursorLocked = true;
        Cursor.lockState = CursorLockMode.Locked;

    }
  
    void Update()
    {
        isGrounded = Controller.isGrounded;
        //Crouch Animation and/or mechanic 
        isGrounded = Controller.isGrounded;   //ensures that the player is grounded during the crouch animation
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
    }
    
    public void playerJump()
    {
        //checks if the player is grounded, if true allows the player to jump 
        if (isGrounded)
        {
            playerSpeed.y = Mathf.Sqrt(jumpForce * gravityValue * -2.0f);
        }
    }

    public void playerSprint()
    {
        //Player's sprint function, changes player speed if sprinting = true 
        isSprinting = !isSprinting;
        if (isSprinting)
        {
            velocity = 8f;
        }
        else
        {
            velocity = 5f;
        }
    }

    public void playerCrouch()
    {
        isCrouching = !isCrouching;
        crouchTimer = 0;
        lerpCrouch = true;

    }

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
