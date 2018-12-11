using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Prototype.NetworkLobby;
public class SetupLocalPlayer : NetworkBehaviour
{


    public Text namePrefab;
    public Text nameLabel;// nombre
    public Transform namePos;// posicion del name en el canvas
    string textboxname = "";
    // string colourboxname = "";
    public Slider healthPrebab;
    public Slider health;
    public GameObject explosion;
    public GameObject NodesPrefab;
    public GameObject nodes;
    public GameObject[] Nodes;
    public bool ChargeNodes=false;
    NetworkStartPosition[] spawnPos;

    [SyncVar]
    public bool startGame = false;

    [SyncVar(hook = "OnChangeName")]
    public string pName = "player";

    // [SyncVar(hook = "OnChangeColour")]
    // public string pColour = "#ffffff";

    [SyncVar(hook = "OnChangeHealth")]
    public int healthValue = 100;
    BuildManager buildManager;

    void OnChangeName(string n)
    {
        pName = n;
        nameLabel.text = pName;
    }

    // void OnChangeColour(string n)
    // {
    //     pColour = n;
    //     Renderer[] rends = GetComponentsInChildren<Renderer>();

    //     foreach (Renderer r in rends)
    //     {
    //         if (r.gameObject.name == "BODY")
    //             r.material.SetColor("_Color", ColorFromHex(pColour));
    //     }
    // }

    /*
        desde cmdChangehealth se actualiza la vida en el servidor
        luego como es una variable sync, esta se envia a los clients
        en el metodo el cual se haya mandado como parametro en lavariable sync
        el metodo recibe el valor actualizado y este finalmente actualiza 
        la variable sync local del cliente
     */
    public void OnChangeHealth(int hitValue)
    {
        healthValue = hitValue;
        health.value = healthValue;
        Debug.Log("On change health");
    }

    [ClientRpc]
    public void RpcRespawn()
    {
        if (!isLocalPlayer) return;
        if (spawnPos != null && spawnPos.Length > 0)
        {
            this.transform.position = spawnPos[Random.Range(0, spawnPos.Length)].transform.position;
        }
    }


    [Command]
    public void CmdChangeHealth(int hitValue)
    {
        healthValue = healthValue + hitValue;
        health.value = healthValue;
        //aca es cuando se muere y tngo que poner el efecto de explosion(particle system)
        if (health.value <= 0)
        {
            GameObject e = Instantiate(explosion, this.transform.position, Quaternion.identity);
            NetworkServer.Spawn(e);
            this.GetComponent<Rigidbody>().velocity = Vector3.zero;
            Destroy(this.gameObject);
            // RpcRespawn();
            // healthValue = 100;
        }
    }



    [Command]
    public void CmdGameStart()
    {
        // buildManager.alreadyPlayer += 1;
        // Debug.Log("ONChangeGAmeStart");
        // Debug.Log(buildManager.alreadyPlayer);
        // if (buildManager.alreadyPlayer > 1)
        //     startGame = true;
        // if (startGame)
        //     buildManager.startGame = true;
        RpcGameStart();

    }
    [ClientRpc]
    public void RpcGameStart()
    {
        // if (!isLocalPlayer) return;

        buildManager.alreadyPlayer += 1;
        // Debug.Log("RPCGAMESTART");
        // Debug.Log(buildManager.alreadyPlayer);
        // if (buildManager.alreadyPlayer > 1)
            startGame = true;
        // if (startGame)
            buildManager.startGame = true;
    }
    [Command]
    public void CmdChangeName(string newName)
    {
        pName = newName;
        nameLabel.text = pName;
    }

    // [Command]
    // public void CmdChangeColour(string newColour)
    // {
    //     pColour = newColour;
    //     Renderer[] rends = GetComponentsInChildren<Renderer>();

    //     foreach (Renderer r in rends)
    //     {
    //         if (r.gameObject.name == "BODY")
    //             r.material.SetColor("_Color", ColorFromHex(pColour));
    //     }
    // }



    [Command]
    public void CmdUpdatePlayerCharacter(int cid)
    {
        NetworkManager.singleton.GetComponent<LobbyManager>().SwitchPlayer(this, cid);
    }




    void OnGUI()
    {
        if (isLocalPlayer)
        {
            textboxname = GUI.TextField(new Rect(25, 15, 100, 25), textboxname);
            if (GUI.Button(new Rect(130, 15, 35, 25), "Set"))
                CmdChangeName(textboxname);


           if (Event.current.Equals(Event.KeyboardEvent("0")) ||
                            Event.current.Equals(Event.KeyboardEvent("1")))
            {
                int charid = int.Parse(Event.current.keyCode.ToString().Substring(5)) + 1;
                CmdUpdatePlayerCharacter(charid);
            }
            // colourboxname = GUI.TextField(new Rect(170, 15, 100, 25), colourboxname);
            // if (GUI.Button(new Rect(275, 15, 35, 25), "Set"))
            //     CmdChangeColour(colourboxname);
            if (!startGame)
            {
                if (GUI.Button(new Rect(275, 15, 35, 25), "StartGame"))
                {
                    // startGame = true;
                    CmdGameStart();

                }
            }
        }

    }




    void Start()
    {
        if (isLocalPlayer)
        {



            GetComponent<MyPlayerController>().enabled = true;
            CameraFollow360.player = this.gameObject.transform;
        }
        else
        {
            GetComponent<MyPlayerController>().enabled = false;
        }
        buildManager = BuildManager.instance;
        GameObject canvas = GameObject.FindWithTag("MainCanvas");
        nameLabel = Instantiate(namePrefab, Vector3.zero, Quaternion.identity) as Text;
        nameLabel.transform.SetParent(canvas.transform);

        health = Instantiate(healthPrebab, Vector3.zero, Quaternion.identity) as Slider;
        health.transform.SetParent(canvas.transform);
        if(ChargeNodes)
        nodes = Instantiate(NodesPrefab, Vector3.zero, Quaternion.identity);

        spawnPos = FindObjectsOfType<NetworkStartPosition>();
    }


    // Update is called once per frame
    void Update()
    {
        //determine if the object is inside the camera's viewing volume
        if (nameLabel != null)
        {
            Vector3 screenPoint = Camera.main.WorldToViewportPoint(this.transform.position);
            bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 &&
                            screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
            //if it is on screen draw its label attached to is name position
            if (onScreen)
            {
                Vector3 nameLabelPos = Camera.main.WorldToScreenPoint(namePos.position);
                nameLabel.transform.position = nameLabelPos;
                health.transform.position = nameLabelPos + new Vector3(0, 15, 0);
            }
            else //otherwise draw it WAY off the screen 
            {
                nameLabel.transform.position = new Vector3(-1000, -1000, 0);
                health.transform.position = new Vector3(-1000, -1000, 0);
            }
        }
    }

    public void OnDestroy()
    {
        if (nameLabel != null && health != null)
        {
            Destroy(nameLabel.gameObject);
            Destroy(health.gameObject);
        }
    }




    void OnCollisionEnter(Collision collision)
    {

        if (isLocalPlayer && collision.gameObject.tag == "Enemy")
        {
            Debug.Log("Collii");
            CmdChangeHealth(-5);
        }
    }


    //authoridad
    /// TRIGGER ZONE START///

    public void OnTriggerStay(Collider other)
    {

        CmdSetAuthority(other.GetComponent<NetworkIdentity>(), this.GetComponent<NetworkIdentity>());

    }

    public void OnTriggerExit(Collider other)
    {

        CmdRemoveAuthority(other.GetComponent<NetworkIdentity>(), this.GetComponent<NetworkIdentity>());

    }


    /// ASSIGN AND REMOVE CLIENT AUTHORITY///

    [Command]
    void CmdSetAuthority(NetworkIdentity grabID, NetworkIdentity playerID)
    {
        grabID.AssignClientAuthority(playerID.connectionToClient);
    }

    [Command]
    void CmdRemoveAuthority(NetworkIdentity grabID, NetworkIdentity playerID)
    {
        grabID.RemoveClientAuthority(playerID.connectionToClient);
    }

    //fin authoridad


}
