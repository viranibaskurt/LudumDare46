using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [SerializeField] AudioClip clip;
    AudioSource soundPlayer;


    // Start is called before the first frame update
    void Start()
    {
        soundPlayer = gameObject.AddComponent<AudioSource>();
        soundPlayer.loop = false;
        soundPlayer.playOnAwake = false;
        soundPlayer.clip = clip;
    }

    public void PlaySound()
    {
        soundPlayer.Play();
    }
}
