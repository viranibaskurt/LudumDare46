using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleFixer : MonoBehaviour
{

     public void Fix()
    {
        Collider[] hitCol = Physics.OverlapSphere(transform.position, 1);

        foreach (var item in hitCol)
        {
            if(item.GetComponent<Hole>())
            {
                item.GetComponent<Hole>().Fix();
            }
        }
    }
}
