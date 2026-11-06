using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class doorControllerSliding : MonoBehaviour
{
    public GameObject door;
    //open close variables for sliding or linear door 
    [SerializeField] private float openPositionX;
    [SerializeField] private float closedPositionX;

    //General variables 
    [SerializeField] private float openSpeed;
    [SerializeField] private bool IsDoorOpening;
    
    // Update is called once per frame
    void Update()
    {
        Vector3 currentPosition = door.transform.localPosition;

        if (IsDoorOpening)
        {
            //defines the target position for the door to be considered "open" 
            Vector3 targetedOpenPosition = new Vector3(openPositionX, currentPosition.y, currentPosition.z);
            door.transform.localPosition = Vector3.MoveTowards(currentPosition, targetedOpenPosition, openSpeed * Time.deltaTime);
        }

        else
        {
            //defines the target position for the door to be considered "closed" 
            Vector3 targetedOpenPosition = new Vector3(closedPositionX, currentPosition.y, currentPosition.z);
            door.transform.localPosition = Vector3.MoveTowards(currentPosition, targetedOpenPosition, openSpeed * Time.deltaTime);
        }

    }
    public void OpenDoor()
    {
        if (IsDoorOpening)
        {
            IsDoorOpening = !IsDoorOpening;
        }
        else
        {
            IsDoorOpening = true;
        }
    }

   
}
