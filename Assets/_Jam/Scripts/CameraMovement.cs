using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Vector3 Direction;

    public float shakeDuration = 0.5f;
    public float shakeMag = 1;

    Vector3 StartPos;
    // Start is called before the first frame update
    void Start()
    {
        StartPos = transform.position;
        //InvokeRepeating("RandomDirection", 1f,1f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = StartPos + Direction * Mathf.Sin(Time.timeSinceLevelLoad);
    }

    public void ShakeCamera()
    {
        StartCoroutine(CameraShakeRoutine());
    }


    void RandomDirection()
    {
        Direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
    }


    IEnumerator CameraShakeRoutine()
    {
        Vector3 posBeforeShake = transform.position;
        float shakeTime = 0;

        while (shakeTime < shakeDuration)
        {
            shakeTime += Time.deltaTime;
            transform.position = posBeforeShake+ new Vector3(
                (Random.value - 0.5f) * shakeMag,
                (Random.value - 0.5f) * shakeMag,
                0);

            yield return null;
        }

        transform.position = posBeforeShake;
    }
}
