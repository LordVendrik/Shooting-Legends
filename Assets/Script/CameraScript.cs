using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript: MonoBehaviour
{
    public GameObject pal;
    Vector3 movement;
    public float offset1 = -0.18f;
    public float offset2 = +2.3f;
    public float offset3 = +1.5f;

    // Update is called once per frame
    void Update()
    {
        if (pal != null)
        {
            movement.x = pal.transform.position.x + offset1;
            movement.y = pal.transform.position.y + offset2;
            movement.z = pal.transform.position.z + offset3;

            transform.position = movement;
        }
    }
}
