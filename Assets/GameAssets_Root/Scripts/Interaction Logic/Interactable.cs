using UnityEngine;
using UnityEngine.UIElements;

public abstract class Interactable : MonoBehaviour
{
    //add or remove interaction component 
    public bool useEvents; 

    //Messaged Displayed to show an item is interactable 
    public string promptMsg; 

    //function to be called by the player 
    public void baseInteract()
    {
        if (useEvents)
            GetComponent<InteractionEvents>().onInteraction.Invoke();
        Interact();
    }
    protected virtual void Interact()
    {
        //template for interactable items to override.

    }


}
