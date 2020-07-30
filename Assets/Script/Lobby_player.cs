using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Lobby_player : NetworkLobbyPlayer
{

    public GameObject MyParent;
    public GameObject JoinButton;
    public Text MyName;
    public GameObject Lobby;

    public void OnJoinButton() 
    {
        SendReadyToBeginMessage();
        JoinButton.SetActive(false);
    }

    public override void OnClientEnterLobby()
    {
        base.OnClientEnterLobby();
        MyParent = GameObject.FindGameObjectWithTag("MyParent");
        gameObject.transform.SetParent(MyParent.transform);
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        MyName.text = "MyPlayer";
        JoinButton.gameObject.SetActive(true);
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        MyName.text = "Notmyplayer";
        JoinButton.gameObject.SetActive(false);
    }
}
