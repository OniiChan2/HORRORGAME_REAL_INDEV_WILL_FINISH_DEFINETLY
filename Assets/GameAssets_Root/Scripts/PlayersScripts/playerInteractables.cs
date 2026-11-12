using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.LowLevel;

public class playerInteractables : MonoBehaviour
{
    public Camera cam;
    [SerializeField] private LayerMask mask;
    [SerializeField] GameObject UI_playerImage; //Player's crosshair 
    [SerializeField] private float playerReach = 3f;    //Ray cast Variable
    [SerializeField] private playerInventory inventory;
    [SerializeField] private ItemScriptableObject KeyID;

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
    [SerializeField] private float ThrowForceFwd;
    [SerializeField] private float ThrowForceUp;
    [SerializeField] private float TextShowDuration = 1f; 
    private GameObject HeldInteratable;
    public bool IsCurrentlyHolding = false;
    private Rigidbody HeldRb; 

    void Start()
    {
        cam = GetComponent<playerCameraLook>().cam;
        playerUI = GetComponent<playerUI>();
        inputManager = GetComponent<playerInputManager>();
        inventory  = GetComponent<playerInventory>();   
        UI_playerImage.gameObject.SetActive(false);
        ClearScreen();
    }
    void Update()
    {
        bool HitSomethingToInteractWith = false;
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
                if (inputManager.onFoot.playerInteract.triggered)
                {
                    if (hitInfo.collider.GetComponent<Rigidbody>() != null && hitInfo.collider.CompareTag("Holdable")) // checks first if the item that has been interacted with is holdable 
                    {   
                        if (IsCurrentlyHolding)
                        {
                            Debug.Log("Tried to pick something up while ");
                            playerUI.UpdateText("Player is Already Holding an Item");
                            Invoke(nameof(ClearScreen), TextShowDuration);
                            return;
                        }

                        //Holdable Item Logic
                        KeyScript keyScript = hitInfo.collider.GetComponent<KeyScript>();
                        Rigidbody Itemrb = hitInfo.collider.GetComponent<Rigidbody>();
                        HeldInteratable = hitInfo.collider.gameObject;
                        HeldRb = Itemrb;

                        if (!IsCurrentlyHolding)
                        {
                            KeyID = keyScript.Key_Data;
                            //if so, call the function to pick it up 
                            PickUpItem();
                        }
                    }
                    else
                    {
                        interactable.baseInteract();
                    }
                }
            }
        }
        //Checks whether or not the player is looking at an interactable object, if so changes the crosshair 
        if (isLookingAtInteractable != HitSomethingToInteractWith)
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
        HeldRb.isKinematic = true;
        HeldRb.useGravity = false;
        HeldInteratable.transform.SetParent(ItemHolder); //sets its parented game object to the Item Holder game object on the camera 
        HeldInteratable.transform.localPosition = Vector3.zero; //resets the items local position relative to the parent 
        HeldInteratable.transform.localRotation = Quaternion.identity; //Sets the Item's rotation to 0,0,0 
        Collider HolddableCollider = HeldInteratable.GetComponent<Collider>();
        if (HolddableCollider != null) HolddableCollider.enabled = false; //turns off the collider when equipped by the player 
        inventory.Inventory.Add(KeyID); //Adds the key to the player's inventory 
        IsCurrentlyHolding = true;
    }

    //Item Drop System 
    public void DropItem()
    {
        if (IsCurrentlyHolding)
        {
            HeldRb.isKinematic = false;
            HeldRb.useGravity = true;

            HeldInteratable.transform.SetParent(null); //unsets its parent so it can move freely when dropped 
            Collider HolddableCollider = HeldInteratable.GetComponent<Collider>();
            if (HolddableCollider != null) HolddableCollider.enabled = true; //switches it back on so it doesn't fall through the floor 
            HeldRb.AddForce(cam.transform.forward * ThrowForceFwd, ForceMode.Impulse); //Add's a force directed forward relative to the player's camera to throw the object 
            HeldRb.AddForce(cam.transform.up * ThrowForceUp, ForceMode.Impulse); //Add's a force upward relative to the player's camera 
            float RndTorque = Random.Range(-1f, 1f); // creates a random value for to add as torque to the item thrown 
            HeldRb.AddTorque(new Vector3(RndTorque, RndTorque, RndTorque)); //applies the torque
            HeldInteratable = null;
            HeldRb = null;
            inventory.Inventory.Remove(KeyID); //removes the item from inventory list when player has dropped the item 
            IsCurrentlyHolding = false;
            
        }
        else
        {
            playerUI.UpdateText("You are not holding anything.");
            Invoke(nameof(ClearScreen), TextShowDuration);
        }
    }

    private void ClearScreen()
    {
        playerUI.UpdateText(string.Empty);
    }
}
