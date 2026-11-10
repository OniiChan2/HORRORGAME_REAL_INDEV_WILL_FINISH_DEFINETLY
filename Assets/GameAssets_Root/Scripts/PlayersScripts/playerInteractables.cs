using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.LowLevel;

public class playerInteractables : MonoBehaviour
{
    public Camera cam;
    [SerializeField] private LayerMask mask;
    [SerializeField] GameObject UI_playerImage; //Player's crosshair 
    [SerializeField] private float playerReach = 3f;    //Ray cast Variable

    //Sphere cast / Sphere-Overlap Variables 
    [SerializeField] private float sphereInteractableRad = 3f;
    [SerializeField] private LayerMask sphereCastMask;
    private bool InteracatableWasInFrame = false;
    private Interactable savedInteratable;
    private bool isLookingAtInteractable;

    //UI Elements 
    private playerUI playerUI;
    private playerInputManager inputManager;


    //Pick Up System
    [SerializeField] private Transform ItemHolder;
    private GameObject HeldInteratable;
    private bool IsCurrentlyHolding = false;
    private Rigidbody HeldRb; 

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
               

                //Player UI Interactable Variables 
                Interactable interactable = hitInfo.collider.GetComponent<Interactable>(); //calls of the component or script named "Interactables" 
              
                HitSomethingToInteractWith = true; //Updates player UI if the raycast hits something interactable 
                playerUI.UpdateText(interactable.promptMsg);




                if (inputManager.onFoot.playerInteract.triggered)
                {
                    if (hitInfo.collider.GetComponent<Rigidbody>() != null && hitInfo.collider.CompareTag("Holdable")) // checks first if the item that has been interacted with is holdable 
                    {
                        //Holdable Item Logic 
                        Rigidbody Itemrb = hitInfo.collider.GetComponent<Rigidbody>();
                        HeldInteratable = hitInfo.collider.gameObject;
                        HeldRb = Itemrb;

                        //if so, call the function to pick it up 
                        Debug.Log("Interacted with something Holdable");
                        PickUpItem();
                    }
                   
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




    //Item Pick-Up System 

    private void PickUpItem()
    {
        
        if (!IsCurrentlyHolding)
        {
            HeldRb.isKinematic = true;
            HeldRb.useGravity = false;

            HeldInteratable.transform.SetParent(ItemHolder); //sets its parented game object to the Item Holder game object on the camera 
            HeldInteratable.transform.localPosition = Vector3.zero; //resets the items local position relative to the parent 
            HeldInteratable.transform.localRotation = Quaternion.identity; //Sets the Item's rotation to 0,0,0 

            Collider HolddableCollider = HeldInteratable.GetComponent<Collider>();
            if (HolddableCollider != null) HolddableCollider.enabled = false; //turns off the collider when equipped by the player 
            IsCurrentlyHolding = true;
        }
        else
        {
    
            playerUI.UpdateText("Player is Already Holding an Item");

        }
    }

    //Item Drop System 
    public void DropItem()
    {
        IsCurrentlyHolding = false; 
        HeldRb.isKinematic = false;
        HeldRb.useGravity = true;

        HeldInteratable.transform.SetParent(null); //unsets its parent so it can move freely when dropped 
        Collider HolddableCollider = HeldInteratable.GetComponent<Collider>();
        if (HolddableCollider != null) HolddableCollider.enabled = true; //switches it back on so it doesn't fall through the floor 

        HeldInteratable = null;
        HeldRb = null;
    }
}
