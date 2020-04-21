using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum MonsterState
{
    Inactive,
    Idle,
    Attacking
}

public class TentacleController : MonoBehaviour
{
    public Transform Target;
    public Transform Pole;
    public Transform AttackTarget;

    public AudioSource hitSource;
    public AudioSource beforeAttackSource;

    Coroutine IdleStateRoutine;
    Coroutine AttackingStateRoutine;


    Vector3 TargetInitialPos;
    Vector3 PoleInitialPos;

    Vector3 TargetBeginPos;
    Vector3 PoleBeginPos;

    Vector3 TargetPongPos;
    Vector3 PolePongPos;


    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public void Idle()
    {
        TargetInitialPos = Target.position;
        PoleBeginPos = Pole.position;
        IdleStateRoutine = StartCoroutine(IdleRoutine());
    }

    public void Attack()
    {
        StopAllCoroutines();
        AttackingStateRoutine = StartCoroutine(AttackingRoutine());
    }


    IEnumerator IdleRoutine()
    {
        TargetBeginPos = Target.position;
        PoleBeginPos = Pole.position;

        TargetPongPos = new Vector3(Random.value * 10, Random.value, Random.value * 3);
        PolePongPos = new Vector3(Random.value * 3, Random.value, Random.value * 2);

        float RandomTargetTime = Random.value;
        float RandomPoleTime = Random.value * 2;


        while (true)
        {
            Target.position = Vector3.Lerp(Target.position, TargetBeginPos + Mathf.Sin(Time.time * RandomTargetTime) * TargetPongPos, 0.01f);
            //Pole.position = Vector3.Lerp(Pole.position, PoleBeginPos + Mathf.Cos(Time.time * RandomPoleTime) * PolePongPos, 0.01f);


            yield return null;
        }


    }
    IEnumerator AttackingRoutine()
    {
        TargetBeginPos = Target.position;
        //AttackTarget.position = new Vector3(TargetBeginPos.x, 2.5f, -11);
        Vector3 attackPoint = new Vector3(0, 2, -14f);

        beforeAttackSource.Play();

        yield return new WaitForSeconds(0.5f);

        hitSource.Play();

        while ((Target.position - AttackTarget.position).sqrMagnitude > 1f)
        {
            Target.position = Vector3.SlerpUnclamped(Target.position, AttackTarget.position, 0.3f);

            yield return null;

        }


        while ((Target.position - TargetInitialPos).sqrMagnitude > 1f)
        {
            Target.position = Vector3.SlerpUnclamped(Target.position, TargetInitialPos, 0.1f);

            yield return null;

        }
        IdleStateRoutine = StartCoroutine(IdleRoutine());

    }

}
