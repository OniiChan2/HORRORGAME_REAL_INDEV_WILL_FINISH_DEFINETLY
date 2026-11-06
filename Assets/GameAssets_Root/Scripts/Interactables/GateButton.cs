using UnityEngine;
public class GateButton : Interactable
{

    [SerializeField] private GameObject door;
    private bool isDoorOpen; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void Interact()
    {
        isDoorOpen = !isDoorOpen;
        door.GetComponent<Animator>().SetBool("isOpen", isDoorOpen);
        Debug.Log($"Player has interacted with {gameObject.name}");
        Debug.Log($"Is Door Open is set to {isDoorOpen}");
    }
}
