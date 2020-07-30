using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManagerScript : MonoBehaviour
{
    LobbyManager lobby;
    public GameObject Broadcaster;

    void Start()
    {
        lobby = GameObject.FindGameObjectWithTag("LobbyManager").GetComponent<LobbyManager>();
        lobby.SetBack1();
    }

    public void OnClickHost() 
    {
        SetPort();
        Broadcaster.GetComponent<Broadcast_IP_Script>().StartBroadcast();
        lobby.StartHost();
    }

    public void OnClickConnect() 
    {
        Broadcaster.GetComponent<Broadcast_IP_Script>().CatchBroadcast();
        StartCoroutine(ConnectFeature());
    }

    public void SetPort() 
    {
        lobby.networkPort = 1234;
    }

    public void SetIP() 
    {
        lobby.networkAddress = Broadcaster.GetComponent<Broadcast_IP_Script>().Ip[3];
    }

    public void OnClickBack()
    {
        Broadcaster.GetComponent<Broadcast_IP_Script>().StoopBroadcast();
        lobby.StopHost();
        lobby.StopClient();
        lobby.dontDestroyOnLoad = false;
        SceneManager.LoadScene("Start_SCeNE");
    }

    IEnumerator ConnectFeature()
    {
        yield return new WaitForSeconds(1f);
        SetPort();
        SetIP();
        lobby.StartClient();
    }
}
