using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectPlayer : MonoBehaviour
{
    // Start is called before the first frame update

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
}
