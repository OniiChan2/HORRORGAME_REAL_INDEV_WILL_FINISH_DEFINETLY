using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class AOE : MonoBehaviour
{
    [SerializeField] private string PlayerTag;
    [SerializeField] private float effectTimer;
    //Ticked in Editor to toggle between healing and damaging 
    [SerializeField] private bool isHealing; 

    //Declaring delay or timer for Damage over Time 
    private Coroutine HoTCoroutine; 
   
 
    //checks if the character is within AOE 
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PlayerTag))
        {
            if (HoTCoroutine == null) 
            { 
                HoTCoroutine = StartCoroutine(HealOverTime(other));
                Debug.Log("DOT Started, Player is In Range");
            }
        }
    }

    //checks if player if outside of AOE
    private void OnTriggerExit(Collider other)
    {
   
        if (other.CompareTag(PlayerTag))
        {
            if (HoTCoroutine != null)
            {
                StopCoroutine(HoTCoroutine);
                HoTCoroutine = null;
            }
            Debug.Log("No longer in range of AOE");
        }
    }

    //Function which delays and loops the damage 
    private IEnumerator HealOverTime(Collider player)
    {
        
        playerHealth health = player.GetComponent<playerHealth>();
        while (true)
        {
            if (health != null)
            {
                if(isHealing)
                {
                    health.healPlayer();
                }
                else
                {
                    health.takeDamage();
                }
            }

           yield return new WaitForSeconds(effectTimer);
        }

      
    }
}
