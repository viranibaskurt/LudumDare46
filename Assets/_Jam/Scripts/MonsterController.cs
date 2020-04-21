using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;
public class MonsterController : MonoBehaviour
{
    public Transform head;
    public Transform tentacleRoot;

    public TentacleController ten1;
    public TentacleController ten2;

    public List<SinWaver> sinWavers = new List<SinWaver>();

    public float moveSpeed = 1f;

    public AudioSource showHeadSound;
    public AudioSource horrorSound;


    public event Action OnAttack = delegate { };

    private Vector3 headBeginPos;
    private Vector3 tentacleBeginPos;

    private Vector3 headDownPos;
    private Vector3 tentacleDownPos;

    Coroutine ShowCoroutine;
    Coroutine AttackCoroutine;


    public void Show()
    {
        ShowCoroutine = StartCoroutine(ShowRoutine());
    }

    IEnumerator ShowRoutine()
    {

        headBeginPos = head.position;
        tentacleBeginPos = tentacleRoot.position;

        headDownPos = head.position + Vector3.down * 25;
        tentacleDownPos = tentacleRoot.position + Vector3.down * 25;

        head.position = headDownPos;
        tentacleRoot.position = tentacleDownPos;

        showHeadSound.Play();
        yield return null;

        while ((head.position - headBeginPos).sqrMagnitude > 0.5f)
        {
            head.position = Vector3.Lerp(head.position, headBeginPos, Time.deltaTime * moveSpeed);
            yield return null;
        }

        horrorSound.Play();

        yield return new WaitForSeconds(4f);

        while ((tentacleRoot.position - tentacleBeginPos).sqrMagnitude > 0.5f)
        {
            tentacleRoot.position = Vector3.Lerp(tentacleRoot.position, tentacleBeginPos, Time.deltaTime * moveSpeed);
            yield return null;
        }

        ten1.Idle();
        ten2.Idle();

        foreach (SinWaver item in sinWavers)
        {
            item.Init();
        }

        AttackCoroutine = StartCoroutine(AttackRoutine());
    }

    IEnumerator AttackRoutine()
    {
        while (true)
        {
            float wait = Random.Range(1.5f, 4.5f);
            yield return new WaitForSeconds(wait);

            if (Random.Range(0, 2) == 0)
            {
                ten1.Attack();
            }
            else
            {
                ten2.Attack();
            }

            yield return new WaitForSeconds(0.5f);
            OnAttack();
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public void Sink()
    {
        StartCoroutine(SinkRoutine());
    }

    IEnumerator SinkRoutine()
    {
        if (AttackCoroutine!=null) StopCoroutine(AttackCoroutine);
        yield return new WaitForSeconds(1f);
        showHeadSound.Play();
        while ((head.position - headDownPos).sqrMagnitude > 0.5f)
        {
            head.position = Vector3.Lerp(head.position, headDownPos, Time.deltaTime * moveSpeed);
            tentacleRoot.position = Vector3.Lerp(tentacleRoot.position, tentacleDownPos, Time.deltaTime * moveSpeed);
            yield return null;
        }

        yield return new WaitForSeconds(2f);

        gameObject.SetActive(false);

    }
}
