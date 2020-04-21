using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningControl : MonoBehaviour
{

    AudioSource audioSource;
    Animator anim;
    Coroutine Routine;


    private void OnDisable()
    {
        StopAllCoroutines();
    }
    public void PlayLightning()
    {
        StopAllCoroutines();

        if (audioSource == null || anim == null)
            Init();

        Routine = StartCoroutine(LightningRoutine());
    }

    public void StopLightning()
    {
        StopAllCoroutines();

        if (audioSource == null || anim == null)
            Init();

        audioSource.Stop();
        anim.SetTrigger("Reset");
    }

    IEnumerator LightningRoutine()
    {


        anim.SetTrigger("Lightning");
        yield return new WaitForSeconds(1f);
        audioSource.Play();

    }

    void Init()
    {
        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
    }
}
