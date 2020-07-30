using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerScript : NetworkBehaviour
{
    // Movement Realted Variables
    public float speed;
    public FixedJoystick movefx;
    public FixedJoyastick2 camfx;
    public Vector3 formove;
    public Vector3 sidemove;
    Vector3 lookvec;
    public Rigidbody rb;
    public Vector3 localvelocity;
    float locvelz;
    float locvelx;
    bool dash;
    Vector3 dashposition;
    public float dashcontoller;
    public float withaimMovementspeed;
    public float withoutaimMovementspeed;
    //Movement Related Variables


    public Animator anim; // For animation

    // Weapon Realted Variables
    public GameObject Firrepoint;
    public GameObject Firrepoint2;
    public List<GameObject> vfx = new List<GameObject>();
    private GameObject effectstoSpawn;
    public float firerate;
    float timetofire = 0.4f;
    public int turn = 1;
    public GameObject Shield;
    public bool wantShield = false;
    public bool wantShoot = true;

    [SyncVar]
    float CoolDown;

    bool DisableWeapons;
    public Transform BombPos;
    public GameObject Bomb;
    public float force;
    public float NoOfBombs = 2;
    // Weapon Realted Variables

    //Health Related Variables
    [SyncVar]
    public float health;

    public Image healthbarScreen;
    public float damage = 1;
    public float Bombdamage = 10;
    //Health Related Variables

    // Enemy Related Variables
    public List<GameObject> Enemies = new List<GameObject>();
    // Enemy Related Variables

    // UI Related Variables
    Button ModeButton;
    Text ModeButtonText;
    public Image Cooldownbar;
    public GameObject Canvas;
    GameObject GameManager;
    public bool Win;
    public LobbyManager LobbyManager;
    public string Pname;
    public Text PlayerName;
    bool Setname = true;
    Button bombbut;
    public bool Hidden;
    public ParticleSystem DashGlow;
    public AudioSource ShootSfx;
    // UI Related Variables

    // When Match Start Do All of THis
    void Start()
    {
        ShootSfx = GameObject.Find("ShootSound").GetComponent<AudioSource>();
        DashGlow.Stop();
        DEtachOnscreenHeatlhandCooldownBar();
        wantShoot = true;
        GameManager = GameObject.FindGameObjectWithTag("GameController");
        GameManager.GetComponent<GameManagerScript>().Players.Add(this.gameObject);
        if (isLocalPlayer)
        {
            PlayerName.color = Color.green;
            LobbyManager = GameObject.FindGameObjectWithTag("LobbyManager").GetComponent<LobbyManager>();
            FindObjectOfType<Camera>().GetComponent<CameraScript>().pal = this.gameObject;
            movefx = GameObject.FindGameObjectWithTag("leftjoystick").GetComponent<FixedJoystick>();
            movefx.ps = this;
            camfx = GameObject.FindGameObjectWithTag("Rightjoystick").GetComponent<FixedJoyastick2>();
            camfx.ps = this;
            Button dashbutton = GameObject.FindGameObjectWithTag("Dash").GetComponent<Button>();
            dashbutton.onClick.AddListener(Dashbutton);
            ModeButton = GameObject.FindGameObjectWithTag("Mode").GetComponent<Button>();
            ModeButtonText = GameObject.FindGameObjectWithTag("Mode").transform.GetChild(0).GetComponent<Text>();
            ModeButton.GetComponent<Image>().color = Color.red;
            bombbut = GameObject.FindGameObjectWithTag("bomb").GetComponent<Button>();
            bombbut.onClick.AddListener(throwBomb);
            if (isServer)
            {
                ModeButton.onClick.AddListener(RpcChangeMode);
            }
            else
            {
                ModeButton.onClick.AddListener(CmdChangeMode);
            }
        }
        else
        {
            this.gameObject.tag = "enemy";
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            Enemies.Add(player);
            player.GetComponent<PlayerScript>().Enemies.Add(this.gameObject);
            GameObject[] Otherenemies = GameObject.FindGameObjectsWithTag("enemy");
            for (int i = 0; i < Otherenemies.Length; i++)
            {
                if (Otherenemies[i].name != this.gameObject.name)
                {
                    Enemies.Add(Otherenemies[i]);
                    Otherenemies[i].GetComponent<PlayerScript>().Enemies.Add(this.gameObject);
                }
            }
        }
        effectstoSpawn = vfx[0];
    }

    // WHen match is going on keep doing this
    void Update()
    {
        Canvas.transform.position = new Vector3(transform.position.x - 0.1f, 0.8f, transform.position.z);
        healthbarScreen.fillAmount = health / 100;
        Cooldownbar.fillAmount = CoolDown / 5;

        if (Hidden == false)
        {
            Canvas.gameObject.SetActive(true);
        }

        if (isLocalPlayer)
        {
            if (NoOfBombs <= 0)
            {
                bombbut.gameObject.SetActive(false);
            }
            else
            {
                bombbut.gameObject.SetActive(true);
            }
        }

        if (Setname)
        {
            if (isServer)
                RpcChangeName(PlayerPrefs.GetString("PName"));
            else
                CmdChangeName(PlayerPrefs.GetString("PName"));
        }

        StartCoroutine(SetName());

        //Checking for health
        if (health <= 0)
        {
            Debug.Log("player dead");
            GameManager.GetComponent<GameManagerScript>().GameStarted = true;

            if (isLocalPlayer)
                GameManager.GetComponent<GameManagerScript>().LocalplayerDied = true;

            GameManager.GetComponent<GameManagerScript>().Players.Remove(this.gameObject);
            Destroy(this.gameObject);
        }
        //Checking for health

        else
        {
            if (isLocalPlayer && Win)
            {
                GameManager.GetComponent<GameManagerScript>().ResultUiGM.SetActive(true);
            }

            anim.SetInteger("speed", (int)speed);

            if (dash == true)
            {
                rb.velocity += transform.forward * speed * dashcontoller;    // Controlling Dash
            }

            /// Checking Movement type and Using Weapons
            if (camfx != null)
            {
                if (camfx.AimMode == false && camfx.ShieldMode == false && dash == false)
                {
                    WithoutaimMovement();
                    ShootSfx.Stop();
                    timetofire = Time.time + 0.4f;
                    anim.SetBool("Aimmode", false);
                }
                else if (camfx.AimMode == true && camfx.ShieldMode == false)
                {
                    WithaimMovement();
                    if (!DisableWeapons)
                    {
                        if (isServer)
                            RpcSpawnVfx();
                        else
                            CmdSpawnVfx();
                    }
                    anim.SetBool("Aimmode", true);
                }
                else if (camfx.ShieldMode == true && camfx.AimMode == false)
                {
                    WithaimMovement();
                    if (!DisableWeapons)
                    {
                        if (isServer)
                            RpcSummonShield();
                        else
                            CmdSummonShield();
                    }
                    anim.SetBool("Aimmode", true);
                }

                if ((camfx.ShieldMode == false && wantShield == true) || (camfx.ShieldMode == true && DisableWeapons))
                {
                    if (isServer)
                        RpcUnSummonShield();
                    else
                        CmdUnSummonShield();
                }

                // Setting weapon cooldown
                if (camfx.ShieldMode || camfx.AimMode)
                {
                    if (isServer)
                        RpcCooldownUp();
                    else
                        CmdCooldownUp();
                }
                else
                {
                    if (isServer)
                        RpcCooldownDown();
                    else
                        CmdCooldownDown();
                }
                // Setting weapon cooldown
            }

            // Checking weapon cooldown
            if (CoolDown < 0)
            {
                CoolDown = 0;
            }
            else if (CoolDown > 5)
            {
                CoolDown = 5;
                DisableWeapons = true;
            }
            // Checking weapon cooldown
            /// Checking Movement type and Using Weapons

            // Checking if CHaracter is moving or not and setting animations on that basis
            locvelz = localvelocity.z;
            locvelx = localvelocity.x;

            anim.SetFloat("Horizontal", locvelx);
            anim.SetFloat("Vertical", locvelz);
            // Checking if CHaracter is moving or not and setting animations on that basis
        }
    }

    // Walk when Character is not using weapons
    void WithoutaimMovement()
    {
        if (isLocalPlayer)
        {
            if (movefx.Horizontal > 0.5f || movefx.Vertical < -0.5f || movefx.Vertical > 0.5f || movefx.Horizontal < -0.5f)
            {
                formove = (Vector3.back * movefx.Vertical * Time.deltaTime * speed);
                sidemove = (Vector3.left * movefx.Horizontal * Time.deltaTime * speed);
                lookvec = new Vector3(movefx.Horizontal, 0, movefx.Vertical);
                transform.LookAt(transform.position + lookvec * -1);
                rb.MovePosition(transform.position + formove + sidemove);
                anim.SetBool("IsWalking", true);
                anim.SetBool("candash", true);
                speed = withoutaimMovementspeed;
            }
            else
            {
                anim.SetBool("IsWalking", false);
                anim.SetBool("candash", false);
                speed = 0;
            }
        }
    }

    // Walk when Character is using weapons
    void WithaimMovement()
    {
        if (isLocalPlayer)
        {
            if (movefx.Horizontal > 0.3f || movefx.Vertical < -0.3f || movefx.Vertical > 0.3f || movefx.Horizontal < -0.3f)
            {
                formove = (Vector3.back * movefx.Vertical * Time.deltaTime * speed);
                sidemove = (Vector3.left * movefx.Horizontal * Time.deltaTime * speed);
                rb.MovePosition(transform.position + formove + sidemove);
                anim.SetBool("IsWalking", true);
                speed = withaimMovementspeed;
            }
            else
            {
                anim.SetBool("IsWalking", false);
                speed = 0;
            }

            if (camfx.Horizontal > 0.5f || camfx.Vertical < -0.5f || camfx.Vertical > 0.5f || camfx.Horizontal < -0.5f)
            {
                lookvec = new Vector3(camfx.Horizontal, 0, camfx.Vertical);
                transform.LookAt(transform.position + lookvec * -1);
            }
        }
    }

    // Dash Controller
    public void Dashbutton()
    {
        if (isServer)
            RpcDashbutton();
        else
            CmdDashbutton();
    }

    // for On Screen Health and Cooldown bar
    public void DEtachOnscreenHeatlhandCooldownBar()
    {
        transform.GetChild(3).gameObject.transform.SetParent(null);
    }

    // Stops Dash after some time
    public IEnumerator Dash()
    {
        yield return new WaitForSeconds(0.3f);
        DashGlow.Stop();
        rb.velocity = Vector3.zero;
        anim.SetBool("Dash", false);
        dash = false;
        speed = 0;
    }

    public IEnumerator SetName()
    {
        yield return new WaitForSeconds(1);
        Setname = false;
    }

    public void shootSfxSound()
    {
        ShootSfx.Play();
    }
    
    #region MY Commands Over Network
    [Command]
    public void CmdThrowBomb()
    {
        RpcThrowBomb();
    }

    [ClientRpc]
    public void RpcThrowBomb()
    {
        GameObject b = Instantiate(Bomb, BombPos.position, Quaternion.identity);
        b.GetComponent<Rigidbody>().AddForce(this.gameObject.transform.forward * Time.deltaTime * force, ForceMode.Impulse);
    }

    public void throwBomb()
    {
        if (!isServer)
        {
            CmdThrowBomb();
        }

        if (isServer)
        {
            RpcThrowBomb();
        }

        NoOfBombs--;
    }

    
    [Command]
    public void CmdDashbutton()
    {
        RpcDashbutton();
    }

    [ClientRpc]
    public void RpcDashbutton()
    {
        Debug.Log("dash");
        if (dash == false)
        {
            anim.SetBool("Dash", true);
            dash = true;
            DashGlow.Play();
            StartCoroutine(Dash());
            speed = 2;
        }
    }

    [Command]
    void CmdSpawnVfx() 
    {
        RpcSpawnVfx();
    }

    [ClientRpc]
    void RpcSpawnVfx()
    {
        if (Time.time >= timetofire)
        {
            timetofire = Time.time + 1 / firerate;
            shootSfxSound();
            GameObject vfx;
            if (turn == 1)
            {
                vfx = Instantiate(effectstoSpawn, Firrepoint.transform.position, Firrepoint.transform.rotation);
                turn++;
            }
            else
            {
                vfx = Instantiate(effectstoSpawn, Firrepoint2.transform.position, Firrepoint2.transform.rotation);
                turn--;
            }
        }
        Hidden = false;
    }

    public void Damage() 
    {
        if (isServer)
            RpcDamage();
        else
            CmdDamage();
    }

    [Command]
    public void CmdDamage() 
    {
        RpcDamage();
    }

    [ClientRpc]
    public void RpcDamage()
    {
        this.health = health - damage;
    }

    public void BombDamage()
    {
        if (isServer)
            RpcBombDamage();
        else
            CmdBombDamage();
    }

    [Command]
    public void CmdBombDamage()
    {
        RpcBombDamage();
    }

    [ClientRpc]
    public void RpcBombDamage()
    {
        this.health = health - Bombdamage;
    }

    [Command]
    public void CmdSummonShield() 
    {
        RpcSummonShield();
    }

    [ClientRpc]
    public void RpcSummonShield()
    {
        Shield.SetActive(true);
        Hidden = false;
    }

    [Command]
    public void CmdUnSummonShield()
    {
        RpcUnSummonShield();
    }

    [ClientRpc]
    public void RpcUnSummonShield()
    {
        Shield.SetActive(false);
    }

    [Command]
    public void CmdChangeMode() 
    {
        if (wantShield)
        {
            wantShield = false;
            wantShoot = true;
            ModeButton.GetComponent<Image>().color = Color.red;
            ModeButtonText.text = "Shoot";
        }
        else if (wantShoot)
        {
            wantShoot = false;
            wantShield = true;
            ModeButton.GetComponent<Image>().color = Color.green;
            ModeButtonText.text = "Shield";
        }
    }

    [ClientRpc]
    public void RpcChangeMode()
    {
        if (wantShield) 
        {
            wantShield = false;
            wantShoot = true;
            ModeButton.GetComponent<Image>().color = Color.red;
            ModeButtonText.text = "Shoot";
        }
        else if (wantShoot) 
        {
            wantShoot = false;
            wantShield = true;
            ModeButton.GetComponent<Image>().color = Color.green;
            ModeButtonText.text = "Shield";
        }
    }

    [Command]
    public void CmdCooldownUp()
    {
        RpcCooldownUp();
    }

    [ClientRpc]
    public void RpcCooldownUp() 
    {
        CoolDown += Time.deltaTime;
    }

    [Command]
    public void CmdCooldownDown()
    {
        RpcCooldownDown();
    }

    [ClientRpc]
    public void RpcCooldownDown()
    {
        CoolDown -= Time.deltaTime;
        DisableWeapons = false;
    }

    public void UpgradeFireRate()
    {
        if (isServer)
            RpcUpgradeFireRate();
        else
            CmdUpgradeFireRate();
    }

    [Command]
    public void CmdUpgradeFireRate()
    {
        RpcUpgradeFireRate();
    }

    [ClientRpc]
    public void RpcUpgradeFireRate()
    {
        firerate += 0.5f;
    }


    public void UpgradeDamage()
    {
        if (isServer)
            RpcUpgradeDamage();
        else
            CmdUpgradeDamage();
    }

    [Command]
    public void CmdUpgradeDamage()
    {
        RpcUpgradeDamage();
    }

    [ClientRpc]
    public void RpcUpgradeDamage()
    {
        damage -= 0.1f;
        Bombdamage -= 1f;
    }


    public void UpgradeSpeed()
    {
        if (isServer)
            RpcUpgradeSpeed();
        else
            CmdUpgradeSpeed();
    }

    [Command]
    public void CmdUpgradeSpeed()
    {
        RpcUpgradeSpeed();
    }

    [ClientRpc]
    public void RpcUpgradeSpeed()
    {
        withoutaimMovementspeed += 0.1f;
        withaimMovementspeed += 0.1f;
    }
    

    public void SetName(string Name)
    {
        PlayerName.text = Name;
        this.gameObject.name = PlayerName.text;
    }

    [Command]
    public void CmdChangeName(string name)
    {
        RpcChangeName(name);
    }

    [ClientRpc]
    public void RpcChangeName(string name)
    {
        PlayerName.text = name;
        this.gameObject.name = PlayerName.text;
    }
    #endregion
}
