using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinWaver : MonoBehaviour
{
    public Vector3 Direction = Vector3.right;

    private Vector3 InitialPos;
    private float rand;
    private bool moving;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void Init()
    {
        InitialPos = transform.position;
        rand = Random.value + 0.5f;
        moving = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (moving)
            transform.position = InitialPos + Direction * Mathf.Sin(Time.time * rand);
    }

    private void OnDisable()
    {
        moving = false;
    }
}
