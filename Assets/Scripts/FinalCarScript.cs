using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalCarScript : MonoBehaviour
{
    public GameManager gm;
    public PlayerManager playermanager;
    public GameObject fire;

    public bool finish = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playermanager.gearCount >= 10)
        {
            fire.SetActive(false);
            finish = true;
        }
        
    }
    
}
