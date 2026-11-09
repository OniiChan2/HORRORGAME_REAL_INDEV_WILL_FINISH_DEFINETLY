using UnityEngine;
using UnityEngine.InputSystem;

public class playerInputManager : MonoBehaviour
{

    private PlayerInput playerInput;
    public PlayerInput.OnFootActions onFoot;
    private playerMovement movement;
    private playerHealth health;
    private playerCameraLook look; 

    void Awake()
    {
        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;
        movement = GetComponent<playerMovement>();
        look = GetComponent<playerCameraLook>();
        health = GetComponent<playerHealth>();

        onFoot.testHeal.performed += ctx => health.healPlayer();
        onFoot.testDamage.performed += ctx => health.takeDamage();
        onFoot.playerJump.performed += ctx => movement.playerJump();
        onFoot.playerSprint.performed += ctx => movement.playerSprint();
        onFoot.playerCrouch.performed += ctx => movement.playerCrouch();
        onFoot.LockCursor.performed += ctx => movement.lockCursor();

    }

    private void Update()
    {
        //Move Character model based on movement actions. 
        movement.movementProcess(onFoot.playerMovement.ReadValue<Vector2>());
        //Move Character model and camera based on mouse
        look.lookProcess(onFoot.playerLook.ReadValue<Vector2>(), onFoot.playerMovement.ReadValue<Vector2>());

    }


    private void OnEnable()
    {
        onFoot.Enable();
    }
    private void OnDisable()
    {
        onFoot.Disable();
    }
}

