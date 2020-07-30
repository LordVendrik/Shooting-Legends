using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombRangeDamage : MonoBehaviour
{
    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "enemy" || col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<PlayerScript>().BombDamage();
        }
    }
}
