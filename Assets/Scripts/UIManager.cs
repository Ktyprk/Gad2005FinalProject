using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject NeedGearUI, FixingUI;

    public TextMeshProUGUI needGearCount;

    public PlayerManager _playerManager;

    public bool NeedGear = false, canFixing = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int gearCount = _playerManager.gearCount;

        int remainingGearCount = 20 - gearCount;

        needGearCount.text = remainingGearCount.ToString();

        if(NeedGear && !canFixing)
        {
            NeedGearUI.SetActive(true);
        }else
        {
            NeedGearUI.SetActive(false);
        }
    }

    
}
