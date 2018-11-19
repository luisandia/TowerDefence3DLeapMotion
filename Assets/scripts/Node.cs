using UnityEngine;
using UnityEngine.EventSystems;

using UnityEngine.Networking;
using System.Collections.Generic;
using System.Collections;
public class Node : NetworkBehaviour
{

    public Color hoverColor;
    public Vector3 positionOffset;

    private GameObject turret;
    public List<GameObject> turrets = new List<GameObject>();
    private Renderer rend;
    private Color startColor;

    BuildManager buildManager;
    public GameObject turretToBuild;
    public GameObject[] Nodes;
    // public List<GameObject> items;
    GameObject obj;
    void Start()
    {
        // rend = GetComponent<Renderer>();
        // startColor = rend.material.color;

        buildManager = BuildManager.instance;
    }


    void OnMouseDown()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;


        if (Physics.Raycast(ray, out hit, 100))
        {
            Debug.Log("hice un clickkkkkkkk");
            obj = hit.transform.gameObject;
            Debug.Log(hit.transform.gameObject);
        }

        // if (!isLocalPlayer) return;
        // GameObject turretToBuild = buildManager.GetTurretToBuild();
        // turret = (GameObject)Instantiate(turretToBuild, transform.position + positionOffset, transform.rotation);
        Debug.Log("OnmouseDown");
        if (isLocalPlayer)
            CmdCreateTurret();
        else
        {
            Debug.Log("No tiene autoridad");
            Debug.Log(isLocalPlayer);
        }
        // this.AssignClientAuthority(player.connectionToClient);
        // CmdCreateTurret();
        // assignAuthorityObj.GetComponent<NetworkIdentity>().AssignClientAuthority(this.GetComponent<NetworkIdentity>().connectionToClient);

    }

    void createTurret()
    {
        buildManager = BuildManager.instance;
        // if (EventSystem.current.IsPointerOverGameObject())
        // {
        //     Debug.Log("Horror EventSystem.current.IsPointerOverGameObject()");
        //     return;
        // }

        // if (buildManager.GetTurretToBuild() == null)
        // {
        //     Debug.Log("HOrror buildManager.GetTurretToBuild() == null");
        //     return;
        // }

        // if (turret != null)
        // {
        //     Debug.Log("Can't build there! - TODO: Display on screen.");
        //     return;
        // }
        Debug.Log("Creando turret");
        // GameObject turretToBuild = buildManager.GetTurretToBuild();
        turret = (GameObject)Instantiate(turretToBuild, transform.position + positionOffset, transform.rotation);
        NetworkServer.Spawn(turret);
        // if(hasAuthority)
        // NetworkServer.SpawnWithClientAuthority(turret,base.connectionToClient);
        // else {
        //     Debug.Log("Networkserverclienth autorithy false");
        // }

    }
    [ClientRpc]
    void RpcCreateTurret()
    {
        if (!isServer)
        {

            createTurret();
        }
    }


    [Command]
    void CmdCreateTurret()
    {

        createTurret();

        RpcCreateTurret();
    }


    void OnMouseEnter()
    {
        buildManager = BuildManager.instance;
        // if (EventSystem.current.IsPointerOverGameObject())
        //     return;

        // if (buildManager.GetTurretToBuild() == null)
        //     return;
        // if (EventSystem.current.IsPointerOverGameObject())
        // {
        //     Debug.Log("Horror mouseEnter EventSystem.current.IsPointerOverGameObject()");
        //     return;
        // }

        // if (buildManager.GetTurretToBuild() == null)
        // {
        //     Debug.Log("HOrror mouseEnter buildManager.GetTurretToBuild() == null");
        //     return;
        // }

        // rend.material.color = hoverColor;
    }




    void OnMouseExit()
    {
        // rend.material.color = startColor;
    }



    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer) return;
        if (Input.GetKeyDown("p"))
        {
            Debug.Log("Aprete p");

            CmdCreateTurret();
        }
    }



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


}
