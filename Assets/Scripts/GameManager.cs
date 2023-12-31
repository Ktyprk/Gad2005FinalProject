using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public PlayerManager _playerManager;
    
    public TextMeshProUGUI GearCount;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGearCount();
    }

    public void UpdateGearCount()
    {
        int gearCount = _playerManager.gearCount;

       GearCount.text = gearCount.ToString();
    }
}
