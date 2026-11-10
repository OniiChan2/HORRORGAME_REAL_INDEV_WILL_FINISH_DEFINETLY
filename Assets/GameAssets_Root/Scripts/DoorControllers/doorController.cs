using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.UIElements;


public class doorController : MonoBehaviour
{
    public GameObject door;
    //open close variables for pivot or rotating door 
    [SerializeField] private float openRotation;
    [SerializeField] private float openSpeed;
    private Vector3 closeRotation;

    //Reference to Player Invetoryl 
    [SerializeField] private playerInventory inventory;
    public ItemScriptableObject KeyID;
    

    //General variables 

    [SerializeField] private bool IsDoorOpening;
    [SerializeField] private bool IsLockedDoor;


    private void Start()
    {
        closeRotation = door.transform.localEulerAngles;
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 currentRotation = door.transform.localEulerAngles;

        float closedAngle = IsDoorOpening ? (closeRotation.y + openRotation) : closeRotation.y;
        
            if (IsDoorOpening && !IsLockedDoor)
            {
                if (currentRotation.y < openRotation)
                {
                    door.transform.localEulerAngles = Vector3.Slerp(currentRotation, new Vector3(currentRotation.x, openRotation, currentRotation.z), openSpeed * Time.deltaTime);
                }
            }

            else
            {
                if (currentRotation.y > closedAngle)
                {
                    door.transform.localEulerAngles = Vector3.Slerp(currentRotation, new Vector3(currentRotation.x, closedAngle, currentRotation.z), openSpeed * Time.deltaTime);
                }
            }
    }

    public void OpenDoor()
    {
       
        //checks whether or not the door is locked or not 
        if (IsDoorOpening)
        {
            IsDoorOpening = !IsDoorOpening; 
            
        }
        //Opens the door 
        else
        {
            IsDoorOpening = true;
        }
    }

    //logic used to check whether or not the player has the needed key in his inventory 
    public void UnlockDoor()
    {
        Interactable interactable = door.GetComponent<Interactable>();
        if (!IsLockedDoor)
        {
            interactable.promptMsg = string.Empty;
            OpenDoor();
            
        }

        if(inventory.Inventory.Contains(KeyID))
        {
            OpenDoor();
            inventory.Inventory.Remove(KeyID);
            Debug.Log($"Item: {KeyID.Key_ItemName} Rmoeve from Inventory");
            IsLockedDoor = false;
            interactable.promptMsg = "Door Has Been Unlocked!";

        }
        if(IsLockedDoor)
        {
            interactable.promptMsg = $"Door Is Locked, Find The {KeyID.Key_ItemName}";
        }

         
    }


}

