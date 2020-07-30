using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombSpawner : MonoBehaviour
{
    public GameObject[] SPawnPoints;
    public GameObject SpawnMat;

    // Update is called once per frame
    void Start()
    {
        for (int i = 0; i < SPawnPoints.Length; i++)
        {
            Instantiate(SpawnMat,SPawnPoints[i].transform.position,Quaternion.identity);
        }
    }
}
