using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IK_System : MonoBehaviour
{
    public List<Transform> Joints;

    public Transform Target;
    public Transform Pole;

    //public float lerpSpeed = 1f;
    //private List<float> BoneLengths = new List<float>();
    //private List<Vector3> Positions = new List<Vector3>();

    Transform Root;

    private float[] BoneLengths;
    private Vector3[] Positions;
    private Quaternion[] Rotations;
    private float TotalLength;


    void Awake()
    {
        Init();
    }

    void LateUpdate()
    {
        if (Target == null) return;

        SolveInverse();
        SolveForward();
        if (Pole)
            SolveForPole();
        UpdateJoints();
    }
    
    void SolveInverse()
    {

        for (int i = 0; i < Joints.Count; i++)
        {
            Positions[i] = Joints[i].position;
        }

        Positions[Joints.Count - 1] = Target.position;

        for (int i = Joints.Count - 2; i >= 0; i--)
        {
            Positions[i] = Positions[i + 1] - (Positions[i + 1] - Positions[i]).normalized * BoneLengths[i];
        }

    }

    void SolveForward()
    {

        for (int i = 0; i < Joints.Count; i++)
        {
            if (i > 0)
            {
                Positions[i] = Positions[i - 1] + (Positions[i] - Positions[i - 1]).normalized * BoneLengths[i - 1];
            }
            else
            {
                //first position is always attached the root
                Positions[0] = Root.position;
            }

            //dont set rotation for the effector
            if (i != Joints.Count - 1)
            {
                Rotations[i] = Quaternion.FromToRotation(Vector3.up, (Positions[i + 1] - Positions[i]));
            }
        }

    }

    private void SolveForPole()
    {
        Vector3 polePosition = Pole.position;

        for (int i = Positions.Length - 2; i >= 1; i--)
        {
            var plane = new Plane(Positions[i + 1] - Positions[i - 1], Positions[i - 1]);
            var projectedPole = plane.ClosestPointOnPlane(polePosition);
            var projectedBone = plane.ClosestPointOnPlane(Positions[i]);
            var angle = Vector3.SignedAngle(projectedBone - Positions[i - 1], projectedPole - Positions[i - 1], plane.normal);
            Positions[i] = Quaternion.AngleAxis(angle, plane.normal) * (Positions[i] - Positions[i - 1]) + Positions[i - 1];

        }


        for (int i = 0; i < Joints.Count; i++)
        {

            //dont set rotation for the effector
            if (i != Joints.Count - 1)
            {
                Rotations[i] = Quaternion.FromToRotation(Vector3.up, (Positions[i + 1] - Positions[i]));
            }
        }
    }

    void UpdateJoints()
    {
        for (int i = 0; i < Joints.Count; i++)
        {
            Joints[i].rotation = Rotations[i];
            Joints[i].position = Positions[i];

            //Joints[i].rotation = Quaternion.Slerp(Joints[i].rotation, Rotations[i], lerpSpeed * Time.deltaTime);
            //Joints[i].position = Vector3.Lerp(Joints[i].position, Positions[i], lerpSpeed * Time.deltaTime);
        }
    }

    void Init()
    {
        BoneLengths = new float[Joints.Count - 1];
        Positions = new Vector3[Joints.Count];
        Rotations = new Quaternion[Joints.Count];

        Root = Joints[0];

        for (int i = 1; i < Joints.Count; i++)
        {
            BoneLengths[i - 1] = (Vector3.Distance(Joints[i - 1].transform.position, Joints[i].transform.position));
            TotalLength += BoneLengths[i - 1];
        }

        for (int i = 0; i < Joints.Count; i++)
        {
            Positions[i] = (Joints[i].position);
            Rotations[i] = Joints[i].rotation;

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        for (int i = 0; i < Joints.Count; i++)
        {
            Gizmos.DrawSphere(Joints[i].position, 1f);
            if (i > 0)
                Gizmos.DrawLine(Joints[i - 1].position, Joints[i].position);
        }
    }

}
