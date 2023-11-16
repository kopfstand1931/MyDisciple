using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectPlayer : MonoBehaviour
{
    public AudioSource src;
    public AudioClip[] sfxs;

    public  void PlaySfx1()
    {
        src.clip = sfxs[0];
        src.Play();
    }
    public void PlaySfx2()
    {
        src.clip = sfxs[1];
        src.Play();
    }

    public void PlaySfx3()
    {
        src.clip = sfxs[2];
        src.Play();
    }
}
