using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{

    bool blow;
    public GameObject vfx;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Blow());
    }

    // Update is called once per frame
    void Update()
    {
        if (blow)
        {
            GameObject vf = Instantiate(vfx,this.transform.position,Quaternion.identity);
            Destroy(vf, 1f);
            Destroy(this.gameObject);
        }
    }

    IEnumerator Blow()
    {
        yield return new WaitForSeconds(1.5f);
        blow = true;
    }
}
