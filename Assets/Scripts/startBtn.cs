using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class startBtn : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startB()
    {
        PlayerPrefs.SetInt("mode", 0);
        SceneManager.LoadScene("MainScene");
    }

    public void startHardB()
    {
        PlayerPrefs.SetInt("mode", 1);
        SceneManager.LoadScene("MainScene");
    }
}
