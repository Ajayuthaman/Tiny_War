using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public float health;
    public float maxHealth;
   
    public Text healthText;
    public Slider slider;
    public GameObject healthBarUI;

    private void Start()
    {
        health = maxHealth;
        slider.value = CalculateHealth();
    }

    private void Update()
    {
        slider.value = CalculateHealth();

        if (health < maxHealth)
        {
            healthBarUI.SetActive(true);
        }

        if (health <= 0)
        {
            Animator death =  GetComponent<Animator>();
            death.SetBool("Dead", true);
            Destroy(gameObject,2.4f);
        }

        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }

    private float CalculateHealth()
    {
        return health / maxHealth;
    }

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;

        if(health >= 0)
            healthText.text = health.ToString();
    }
}
