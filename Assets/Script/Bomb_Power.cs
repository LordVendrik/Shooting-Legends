using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb_Power : MonoBehaviour
{
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<PlayerScript>().NoOfBombs++;
            Destroy(this.gameObject);
        }
    }
}
