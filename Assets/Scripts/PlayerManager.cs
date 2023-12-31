using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public int gearCount = 0;

    public UIManager UIManager;

    public float maxHealth = 100f;
    public float currentHealth;

    public Image healthSlider;

    public float damageAmount = 10f;

    void Start()
    {
        currentHealth = maxHealth;

        // Sa�l�k �ubu�unu ba�lang��ta g�ncelle
        UpdateHealthUI();
    }

    void UpdateHealthUI()
    {
        healthSlider.fillAmount = currentHealth / maxHealth;
    }

    public void TakeDamage(float damageAmount)
    {
        // Hasar al�nd���nda sa�l��� azalt
        currentHealth -= damageAmount;

        // Sa�l�k �ubu�unu g�ncelle
        UpdateHealthUI();

        // Karakter �ld���nde iste�e ba�l� olarak ba�ka i�lemler ekleyebilirsiniz
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        
        //gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Gear"))
        {
            gearCount++;
            Destroy(other.gameObject);
        }

        if (other.CompareTag("Damage"))
        {
            TakeDamage(damageAmount);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Car"))
        {
            UIManager.NeedGear = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            UIManager.NeedGear = false;
        }
    }
}
