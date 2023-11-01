using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class seletHard : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void selectMode(bool isOn)
    {
        if (isOn)
        {
            PlayerPrefs.SetInt("mode", 1);
        }
        else
        {
            PlayerPrefs.SetInt("mode", 0);
        }
    }
}
