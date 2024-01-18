using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject NeedGearUI, FixingUI;

    public TextMeshProUGUI needGearCount, canFixText, WranchCount;

    public PlayerManager _playerManager;

    public bool NeedGear = false, canFixing = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int Wranch = _playerManager.wranchCount;
        int gearCount = _playerManager.gearCount;

        int remainingGearCount = 20 - gearCount;

        WranchCount.text = Wranch.ToString();
        needGearCount.text = remainingGearCount.ToString();

        if(NeedGear && !canFixing)
        {
            NeedGearUI.SetActive(true);
        }else
        {
            NeedGearUI.SetActive(false);
        }

        if(_playerManager.gearCount >= 20)
        {
            canFixText.gameObject.SetActive(true);  
        }
    }

    
}
