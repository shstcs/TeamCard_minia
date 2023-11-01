using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class endText : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void retryEasy()
    {
        PlayerPrefs.SetInt("mode", 0);
        SceneManager.LoadScene("MainScene");
    }

    public void retryHard()
    {
        PlayerPrefs.SetInt("mode", 1);
        SceneManager.LoadScene("MainScene");
    }

    public void returnStart() 
    {
        SceneManager.LoadScene("StartScene");
    }
}
