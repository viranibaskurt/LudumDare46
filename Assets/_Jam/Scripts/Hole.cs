using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
    private Transform child;

    Vector3 startScale;

    public event Action OnFixed = delegate { };

    // Start is called before the first frame update
    void Awake()
    {
        child = transform.GetChild(0);

        startScale = child.localScale;

    }



    public void Fix()
    {
        child.localScale = new Vector3(child.localScale.x, child.localScale.y - 0.05f, child.localScale.z);

        if( child.localScale.y<=0.01f)
        {
            OnFixed();
            gameObject.SetActive(false);

        }
    }

    public void ResetHole()
    {
        if(child==null)
            child = transform.GetChild(0);


        child.localScale = startScale;
        ParticleSystem part = child.GetComponent<ParticleSystem>();
        if (!part.isPlaying)
            part.Play();

    }
}
