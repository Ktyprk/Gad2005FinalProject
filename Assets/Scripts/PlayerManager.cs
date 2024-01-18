using System.Collections;
using System.Collections.Generic;
using Climbing;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    [Header("Variables")]
    public int gearCount = 0, wranchCount;
    public float maxHealth = 100f;
    public float currentHealth;
    public float damageAmount = 10f;
    
    [Header("Objects")]
    public UIManager UIManager;
    public Image healthSlider;
    public GameObject PlayerCamera, PlayerMesh;
    public InputCharacterController playerCont;

    public bool hiding = true, inhide;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void UpdateHealthUI()
    {
        healthSlider.fillAmount = currentHealth / maxHealth;
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;

        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Buraya ölme iþlemleri eklenebilir.
        // Örneðin: gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Gear"))
        {
            gearCount++;
            Destroy(other.gameObject);
        }

        if (other.CompareTag("Wranch"))
        {
            wranchCount++;
            Destroy(other.gameObject);
        }

        if (other.CompareTag("HidingBox"))
        {
            // Eðer gizleme kutusuna giriþ yapýldýðýnda bir þey yapmak istiyorsanýz
            // buraya ekleyebilirsiniz.
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            if (gearCount < 20)
            {
                UIManager.NeedGear = true;
            }
            else
            {
                UIManager.NeedGear = false;
                UIManager.canFixing = true;
                UIManager.FixingUI.SetActive(true);
            }
        }

        if (other.CompareTag("HidingBox"))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                /*HideArea hideArea = other.GetComponent<HideArea>();
                if (hideArea != null && !hideArea.hideable)
                {
                    wranchCount--;
                    Transform lockTransform = other.transform.GetChild(1);
                    lockTransform.gameObject.SetActive(false);
                    hideArea.hideable = true;
                }*/

                hiding = !hiding;

                Transform hidingBoxTransform = other.transform;
                Transform boxCameraTransform = hidingBoxTransform.GetChild(0);

                if (!hiding)
                {
                    playerCont.enabled = false;
                    PlayerCamera.SetActive(false);
                    PlayerMesh.SetActive(false);
                    boxCameraTransform.gameObject.SetActive(true);
                }

                if (hiding)
                {
                    playerCont.enabled = true;
                    PlayerCamera.SetActive(true);
                    PlayerMesh.SetActive(true);
                    boxCameraTransform.gameObject.SetActive(false);
                }
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
