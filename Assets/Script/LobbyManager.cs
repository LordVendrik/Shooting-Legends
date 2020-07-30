using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyManager : NetworkLobbyManager
{
    public GameObject Lobby;
    public GameObject Menu;
    public GameObject[] lobbyPlayer;
    public Button Back;
    bool SetBack;

    public void SetBack1()
    {
        if (SceneManager.GetActiveScene().name == "MenuScene")
        {
            if (!SetBack)
            {
                Back.onClick.AddListener(GameObject.Find("MenuManager").GetComponent<MenuManagerScript>().OnClickBack);
                SetBack = true;
            }
        }
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name != "tutorial scene")
        {
            lobbyPlayer = GameObject.FindGameObjectsWithTag("lobyplyr");
            SetBack = false;
        }
    }

    public override void OnStartHost()
    {
        base.OnStartHost();
        Debug.Log("Game");
        Lobby.SetActive(true);
    }
    public override void OnStartClient(NetworkClient lobbyClient)
    {
        base.OnStartClient(lobbyClient);
        Lobby.SetActive(true);
    }
    public override void OnClientSceneChanged(NetworkConnection conn)
    {
        base.OnClientSceneChanged(conn);
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);

    }

    public void ReturnToLobby() 
    {
        SendReturnToLobby();
    }
}
