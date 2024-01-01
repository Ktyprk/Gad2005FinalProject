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

        // Saðlýk çubuðunu baþlangýçta güncelle
        UpdateHealthUI();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void UpdateHealthUI()
    {
        healthSlider.fillAmount = currentHealth / maxHealth;
    }

    public void TakeDamage(float damageAmount)
    {
        // Hasar alýndýðýnda saðlýðý azalt
        currentHealth -= damageAmount;

        // Saðlýk çubuðunu güncelle
        UpdateHealthUI();

        // Karakter öldüðünde isteðe baðlý olarak baþka iþlemler ekleyebilirsiniz
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

        
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Car"))
        {
            if(gearCount < 20) 
            {
               UIManager.NeedGear = true;
            }
            if(gearCount >= 20)
            {
                UIManager.NeedGear = false;
                UIManager.canFixing = true;
                UIManager.FixingUI.SetActive(true);
            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            
                UIManager.NeedGear = false;
            UIManager.FixingUI.SetActive(false);
        }

        if (other.CompareTag("Damage"))
        {
            TakeDamage(damageAmount);
        }
    }
}
