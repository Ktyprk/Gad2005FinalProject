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
     
    private bool isSpeedBoosted = false;
    
    [Header("Objects")]
    public UIManager UIManager;
    public Image healthSlider;
    public GameObject PlayerCamera, PlayerMesh, diedCanvas, finalcanvas;
    public InputCharacterController playerCont;
    public MovementCharacterController MCC;
    public EnemyAI EnemyAI;
    public bool hiding = true, inhide;
    public LevelManager lm;
    public FinalCarScript car;
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
    
    public void AddHealth(float damageAmount)
    {
        currentHealth += damageAmount;
        
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        UpdateHealthUI();
    }

    void Die()
    {
        gameObject.SetActive(false);
        diedCanvas.SetActive(true);
        
        Cursor.lockState = CursorLockMode.None;
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

        if (other.CompareTag("PowerUp"))
        {
            // Apply speed boost when colliding with the power-up
            StartCoroutine(ApplySpeedBoost());
            Destroy(other.gameObject); // PowerUp nesnesini yok et
        }
        
        if (other.CompareTag("Hearth"))
        {
            AddHealth(10f);
            Destroy(other.gameObject); // PowerUp nesnesini yok et
        }
        
        if (other.CompareTag("LevelUp"))
        {
            StartCoroutine(NextLevel());
        }
        
        if (other.CompareTag("FinishCar"))
        {
            if (car.finish == true)
            {
                finalcanvas.SetActive(true);
            }
        }
    }


    IEnumerator NextLevel()
    {
        yield return new WaitForSeconds(2.0f);
        lm.LoadNextLevel();
    }
    
    private IEnumerator ApplySpeedBoost()
    {
        if (!isSpeedBoosted)
        {
            MCC.RunSpeed = 10; 
            isSpeedBoosted = true;
            
            yield return new WaitForSeconds(10.0f);
           
            MCC.RunSpeed = 8; 
            isSpeedBoosted = false;
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

                EnemyAI.playerHidden = !EnemyAI.playerHidden;
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
