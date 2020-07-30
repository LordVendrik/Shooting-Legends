using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Start_Menu : MonoBehaviour
{
    public GameObject NamePanel;
    public InputField nameInput;

    // Start is called before the first frame update
    void Awake()
    {
        if(GameObject.FindGameObjectWithTag("LobbyManager"))
        Destroy(GameObject.FindGameObjectWithTag("LobbyManager"));
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayButton()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void SetNameButton()
    {
        NamePanel.SetActive(true);
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void NameOkButton()
    {
        PlayerPrefs.SetString("PName", nameInput.text);
        NamePanel.SetActive(false);
    }
}
