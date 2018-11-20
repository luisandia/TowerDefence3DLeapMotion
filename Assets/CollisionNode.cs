using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

public class CollisionNode : NetworkBehaviour
{


    BuildManager buildManager;
    private GameObject turret;
    public Vector3 positionOffset;

    // public List<GameObject> items;
    GameObject obj;
    public GameObject turretToBuild;
    // Use this for initialization
    void Start()
    {
        PhysicsCallbacks.OnPostPhysics += OnPostPhysics;
        buildManager = BuildManager.instance;
        buildManager.SetTurretToBuild(buildManager.standardTurretPrefab);

    }

    // Update is called once per frame
    void Update()
    {

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


    private void OnPostPhysics()
    {
        InteractionBehaviour intObjEnemy = gameObject.GetComponent<InteractionBehaviour>();
        if (intObjEnemy.isPrimaryHovered)
        {

            if (EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("Horror IsPointerOverGameObject");
                return;

            }

            if (buildManager.GetTurretToBuild() == null)
            {
                Debug.Log("Horror GetTurretToBuild");

                return;
            }

            if (turret != null)
            {
                Debug.Log("Can't build there! - TODO: Display on screen.");
                return;
            }

            GameObject turretToBuild = buildManager.GetTurretToBuild();
            turret = (GameObject)Instantiate(turretToBuild, transform.position + positionOffset, transform.rotation);
        NetworkServer.Spawn(turret);
		// CmdCreateTurret();
		}
    }


    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("colision NODOOOOOOOOOOOOOOOOOOO");
    }
}
