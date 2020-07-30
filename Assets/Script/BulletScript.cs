using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float speed;
    public int lifetime;
    Vector3 PrevPos;
    public GameObject Effect;

    void Start() 
    {
        PrevPos = transform.position;
        gameObject.tag = "bullet";
    }

    // Update is called once per frame
    void Update()
    {
        PrevPos = transform.position;
        transform.position += -1 * transform.forward * speed * Time.deltaTime;

        RaycastHit[] hits = Physics.RaycastAll(new Ray(PrevPos, (transform.position - PrevPos).normalized), (transform.position - PrevPos).magnitude);

        for (int i = 0; i < hits.Length;i++) 
        {
            Debug.Log(hits[i].collider.gameObject.name);
            if (hits[i].collider.gameObject.tag == "enemy" || hits[i].collider.gameObject.tag == "Player") 
            {
                hits[i].collider.gameObject.GetComponent<PlayerScript>().Damage();
                Destroy(this.gameObject);
                GameObject VFx = (GameObject) Instantiate(Effect, hits[i].point, Quaternion.LookRotation(hits[i].normal));
                Destroy(VFx,1f);
            }
            else if (hits[i].collider.gameObject.tag == "Shield") 
            {
                Destroy(this.gameObject);
                GameObject VFx = (GameObject)Instantiate(Effect, hits[i].point, Quaternion.LookRotation(hits[i].normal));
                Destroy(VFx, 1f);
            }
            else if (hits[i].collider.gameObject.tag == "wall")
            {
                Destroy(this.gameObject);
                GameObject VFx = (GameObject)Instantiate(Effect, hits[i].point, Quaternion.LookRotation(hits[i].normal));
                Destroy(VFx, 1f);
            }
        }

        StartCoroutine(life());
    }

    public IEnumerator life() 
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(this.gameObject);
    }
}
