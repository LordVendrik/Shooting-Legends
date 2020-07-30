using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float enespeed;
    public int enelifetime;

    void Start() 
    {
        gameObject.tag = "enemybullet";
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += -1 * transform.forward * enespeed * Time.deltaTime;
        StartCoroutine(enelife());
    }

    public IEnumerator enelife()
    {
        yield return new WaitForSeconds(enelifetime);
        Destroy(this.gameObject);
    }

    void OnTriggerEnter(Collider col) 
    {

        if (col.gameObject.tag == "Player") 
        {
            col.gameObject.GetComponent<PlayerScript>().health--;
            Destroy(this.gameObject);
        }
    }
}
