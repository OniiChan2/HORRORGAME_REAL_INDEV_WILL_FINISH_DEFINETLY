using System.Collections;
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
