using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip bgmusic;
    // Start is called before the first frame update
    void Start()
    {
        audioSource.clip = bgmusic;
        audioSource.volume = 0.5f;  //±Í¾ÆÇÄ;;
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
