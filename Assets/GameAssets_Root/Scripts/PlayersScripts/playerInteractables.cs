using System.Runtime.CompilerServices;
using UnityEngine;

public class playerInteractables : MonoBehaviour
{
    private Camera cam;
    [SerializeField] private LayerMask mask;
    [SerializeField] GameObject UI_playerImage; //Player's crosshair 
    //Ray cast Variables 
    [SerializeField] private float playerReach = 3f;

    //Sphere cast / Sphere-Overlap Variables 
    [SerializeField] private float sphereInteractableRad = 3f;
    [SerializeField] private LayerMask sphereCastMask;
    private bool InteracatableWasInFrame = false;
    private Interactable savedInteratable; 


    private playerUI playerUI;
    private playerInputManager inputManager;
    private bool isLookingAtInteractable;
    

    void Start()
    {
        cam = GetComponent<playerCameraLook>().cam;
        playerUI = GetComponent<playerUI>();
        inputManager = GetComponent<playerInputManager>();
        UI_playerImage.gameObject.SetActive(false);
    }
    void Update()
    {
        bool HitSomethingToInteractWith = false; 

        //clears screen of prompt message 
        playerUI.UpdateText(string.Empty);


        //Ray Cast Interactions 
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * playerReach);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, playerReach, mask))
        {
            if (hitInfo.collider.GetComponent<Interactable>() != null)
            {
            
                Interactable interactable = hitInfo.collider.GetComponent<Interactable>();
                HitSomethingToInteractWith = true; 
                playerUI.UpdateText(interactable.promptMsg);
                if (inputManager.onFoot.playerInteract.triggered)
                {
                    interactable.baseInteract();
                }
            }
        }


        //Checks whether or not the player is looking at an interactable object, if so changes the crosshair 
        if(isLookingAtInteractable != HitSomethingToInteractWith)
        {
            UI_playerImage.gameObject.SetActive(HitSomethingToInteractWith);
            isLookingAtInteractable = HitSomethingToInteractWith;
        }

      

        //Sphere Cast or Overlap Sphere Interactions
        Collider[] collisionDetection = Physics.OverlapSphere(transform.position, sphereInteractableRad, sphereCastMask);
        bool collisionDetectedByPlayer = false;

        //Detects if the object in range is in fact interactable 
        foreach (Collider objCollided in collisionDetection)
        {
            Interactable Sphereinteractable = objCollided.GetComponent<Interactable>();

            if (Sphereinteractable != null)
            {
                collisionDetectedByPlayer = true;
                savedInteratable = Sphereinteractable;
                break;
            }
        }

        //tells the game it is in range of the object 
        if (collisionDetectedByPlayer && !InteracatableWasInFrame)
        {
            collisionDetectedByPlayer = true;
            if (savedInteratable != null)
            {
                savedInteratable.baseInteract();
            }


        }
        //Tells the game that the player is no longer in rage of the object 
        else if (!collisionDetectedByPlayer && InteracatableWasInFrame)
        {
            collisionDetectedByPlayer = false;

            if (savedInteratable != null)
            {
                savedInteratable.baseInteract();
                savedInteratable = null;
            }
        }

        //Updates the state of the collision detection tracker for the next frames to be generated 
        InteracatableWasInFrame = collisionDetectedByPlayer;
    }
}
