using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionRangeScript : MonoBehaviour
{


    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
        }
    }

}
