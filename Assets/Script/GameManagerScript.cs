using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{
    public List<GameObject> Players;
    public bool LocalplayerDied;
    public GameObject ResultUiGM;
    public bool GameStarted;
    GameObject lobbyManager;
    public GameObject[] lobbyPlayer;
    public GameObject GameMenu;
    public int LevelPoints;
    public Text LevelUpTEXT;
    int LevelOFFireRate, LevelOfSpeed, LevelOfDamage;
    public Text FireRateLevel, SpeedLevel, DamageLevel;
    public Text PLayerLeft;
    public AudioSource backmus;

    void Start()
    {
        backmus.Play();
        lobbyManager = GameObject.FindGameObjectWithTag("LobbyManager");
        lobbyPlayer = lobbyManager.GetComponent<LobbyManager>().lobbyPlayer;
        StartCoroutine(destroy());
    }

    void Update() 
    {
        LevelUpTEXT.text = "Level Up Points :- " + LevelPoints;
        FireRateLevel.text = "X " + LevelOFFireRate;
        SpeedLevel.text = "X " + LevelOfSpeed;
        DamageLevel.text = "X " + LevelOfDamage;
        PLayerLeft.text = "Player Left : " + Players.Count;
        if (LocalplayerDied)
        {
            ResultUiGM.SetActive(true);
            if (Players.Count > 1)
            {
                ResultUiGM.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Waiting for end";
            }
            else
            {
                if(Players[0]!=null)
                ResultUiGM.transform.GetChild(0).gameObject.GetComponent<Text>().text = Players[0].gameObject.name + " is the winner";
            }
        }
        else 
        {
            if (Players.Count == 1 && GameStarted && Players[0]!=null) 
            {
                Players[0].GetComponent<PlayerScript>().Win = true;
                StartCoroutine(ReturnToLobby());
            }
        }
    }

    IEnumerator destroy()
    {
        yield return new WaitForSeconds(5);

        for (int i = 0; i < lobbyPlayer.Length; i++)
            Destroy(lobbyPlayer[i].gameObject);
    }

    IEnumerator ReturnToLobby()
    {
        yield return new WaitForSeconds(3);
        lobbyManager.GetComponent<LobbyManager>().ReturnToLobby();
    }

    public void OpenMenu()
    {
        if (!GameMenu.activeSelf)
            GameMenu.SetActive(true);
        else
            GameMenu.SetActive(false);
    }

    public void UpgradeFireRate()
    {
        if (LevelPoints > 0)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().UpgradeFireRate();
            LevelOFFireRate++;
            LevelPoints--;
        }
    }

    public void UpgradeSpeed()
    {
        if (LevelPoints > 0)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().UpgradeSpeed();
            LevelOfSpeed++;
            LevelPoints--;
        }
    }

    public void UpgradeDamage()
    {
        if (LevelPoints > 0)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().UpgradeDamage();
            LevelOfDamage++;
            LevelPoints--;
        }
    }
}
