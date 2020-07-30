using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Lobby_Script : MonoBehaviour
{
    public GameObject Canvas;

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "tutorial scene")
        {
            Canvas.gameObject.SetActive(false);
        }
        else 
        {
            Canvas.gameObject.SetActive(true);
        }
    }
}
