using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class playerHealth : MonoBehaviour
{
    //player health variables 
    private float health;
    private float LerpTimer; 
    private float maxPlayerHealth = 100f;
    private float minPlayerHealth = 0f;
    public float chipSpeed = 2f;
    public Image frontHealthBar;
    [SerializeField] private TextMeshProUGUI healthText;


    //player stamina variables 
    private float stamina;
    public Image frontStaminaBar;
    [SerializeField] private TextMeshProUGUI staminaText; 
   

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxPlayerHealth;
        stamina = maxPlayerHealth;

        
    }

    // Update is called once per frame
    void Update()
    {
        health = Mathf.Clamp(health, minPlayerHealth, maxPlayerHealth);
        stamina = Mathf.Clamp(stamina, minPlayerHealth, maxPlayerHealth);
        UpdateHealthUI();    
        UpdateStaminaUI();
    
    }

    public void UpdateHealthUI()
    {
        healthText.text = health.ToString();
        float fillFrontHealth = frontHealthBar.fillAmount;
        float HealthDelta = health / maxPlayerHealth;

        if(fillFrontHealth > HealthDelta)
        {
            LerpTimer += Time.deltaTime;
            float percentComplete = LerpTimer / chipSpeed;
            percentComplete *= percentComplete;
            frontHealthBar.fillAmount = Mathf.Lerp(fillFrontHealth, HealthDelta, percentComplete);
        }
        if(fillFrontHealth < HealthDelta)
        {
            LerpTimer += Time.deltaTime;
            float percentComplete = LerpTimer/ chipSpeed;
            percentComplete *= percentComplete;
            frontHealthBar.fillAmount = Mathf.Lerp(fillFrontHealth, HealthDelta, percentComplete);
        }
    }

    public void UpdateStaminaUI()
    {
        staminaText.text = Mathf.Round(stamina).ToString();
        float fillStamina = frontStaminaBar.fillAmount;
        float staminaDelta = stamina / maxPlayerHealth;
        
        if( fillStamina > staminaDelta)
        {
            LerpTimer += Time.deltaTime;
            float percentComplete = LerpTimer / chipSpeed;
            percentComplete *= percentComplete;
            frontStaminaBar.fillAmount = Mathf.Lerp(fillStamina, staminaDelta, percentComplete);
        }
        if (fillStamina < staminaDelta)
        {
            LerpTimer += Time.deltaTime;
            float percentComplete = LerpTimer / chipSpeed;
            percentComplete *= percentComplete;
            frontStaminaBar.fillAmount = Mathf.Lerp(fillStamina, staminaDelta, percentComplete);
        }
    }


    public void damageTaken(float dmg)
    {
        health -= dmg;
        LerpTimer = 0f;
    }

    public void takeDamage()
    {
        damageTaken(Random.Range(5, 10));
    }

    public void HealAmount(float healAmount)
    {
        health += healAmount;
        LerpTimer = 0f;
    }

    public void healPlayer()
    {
        HealAmount(Random.Range(5, 10));
    }

    public void StaminaDrain(float drainAmount)
    {
        Debug.Log("Drain Function Called");

        stamina -= drainAmount;
        LerpTimer = 0f;
            }
}
