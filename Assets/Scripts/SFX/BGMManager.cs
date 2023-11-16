using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public AudioSource src;
    public AudioClip bgm;

    void Start()
    {
        if (src != null && bgm != null)
        {
            src.clip = bgm;
            src.Play();
        }
        else
        {
            Debug.LogError("AudioSource component not found!");
        }
        
    }
    
}
