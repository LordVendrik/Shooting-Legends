using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideScript : MonoBehaviour
{
    void OnTriggerEnter(Collider col)
    {
        
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<PlayerScript>().Hidden = true;
            if (col.gameObject.GetComponent<PlayerScript>().Hidden)
                col.gameObject.GetComponent<PlayerScript>().Canvas.SetActive(false);
        }
        else if (col.gameObject.tag == "enemy")
        {
            col.gameObject.GetComponent<PlayerScript>().Hidden = true;
            if (col.gameObject.GetComponent<PlayerScript>().Hidden)
                col.gameObject.GetComponent<PlayerScript>().Canvas.SetActive(false);
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<PlayerScript>().Canvas.SetActive(true);
            col.gameObject.GetComponent<PlayerScript>().Hidden = false;
        }
        else if (col.gameObject.tag == "enemy")
        {
            col.gameObject.GetComponent<PlayerScript>().Canvas.SetActive(true);
            col.gameObject.GetComponent<PlayerScript>().Hidden = false;
        }
    }
}
