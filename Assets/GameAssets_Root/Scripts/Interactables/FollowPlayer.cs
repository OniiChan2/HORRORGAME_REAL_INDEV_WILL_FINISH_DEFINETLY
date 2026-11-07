using UnityEngine;

public class FollowPlayer : MonoBehaviour
{

    private bool PlayerInRange;
    [SerializeField] private string PlayerTag; 
    [SerializeField] private float rotationRate;
    [SerializeField] private float turnSpeed = 3f;
    private Transform playerTransform;

    private void Update()
    {
        //default state 
        if (!PlayerInRange)
        {
            transform.Rotate(0f, rotationRate * Time.deltaTime, 0f);
           
        }
        // will follow the player once it is in range 
        else if (PlayerInRange && playerTransform != null) {
            Vector3 playerDirection = playerTransform.position - transform.position; // takes the difference between the current rotation of the object and the position of the player 
            playerDirection.y = 0f; // locks the object so that it only rotates from left to right 
            Quaternion rotationTarget = Quaternion.LookRotation(playerDirection); 
            transform.rotation = Quaternion.Slerp(transform.rotation, rotationTarget, turnSpeed * Time.deltaTime);
        }
    }

    //checks if the player is in rage of the 
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PlayerTag))
        {
            PlayerInRange = true;
            playerTransform = other.transform;
        }
        
    }

    //checks if the player is out of range 
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(PlayerTag))
        {
            PlayerInRange = !PlayerInRange;
            playerTransform = null;
        }

    }

}
