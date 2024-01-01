using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fixing : MonoBehaviour
{
    public Canvas ProjectBarCanvas;
    public Image progressBar;
    public UIManager UIManager;
    public PlayerManager playerManager;

    public float fillSpeed = 0.5f;
    public float fillAmount = 0f;

    private bool isFilling = false;

    void Update()
    {
        if (isFilling)
        {
            FillProgressBar();
        }

        if (Input.GetKeyDown(KeyCode.E) && isPlayerInside && UIManager.canFixing)
        {

            ProjectBarCanvas.gameObject.SetActive(true);
            StartFilling();
            
        }
        if (fillAmount >= 1f)
        {
            CompleteTask();
        }

        if (Input.GetKeyUp(KeyCode.E) )
        {
            if (fillAmount >= 1f)
            {
                CompleteTask();
            }
            else
            {
                ResetProgressBar();
            }


        }
    }

    void FillProgressBar()
    {
        fillAmount += fillSpeed * Time.deltaTime;
        fillAmount = Mathf.Clamp01(fillAmount);
        progressBar.fillAmount = fillAmount;
    }

    void StartFilling()
    {
        isFilling = true;
    }

    void ResetProgressBar()
    {
        isFilling = false;
        fillAmount = 0f;
        progressBar.fillAmount = 0f;
    }

    void CompleteTask()
    {
        ProjectBarCanvas.gameObject.SetActive(false);
        isFilling = false;
        fillAmount = 1f;
        progressBar.fillAmount = 1f;
        
        playerManager.gearCount -= 20;

    }

    private bool isPlayerInside = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
            progressBar.gameObject.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            progressBar.gameObject.SetActive(false);
        }
    }
}
