using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FireControl : NetworkBehaviour
{


    public GameObject bulletPrebab;
    public GameObject bulletSpawn;





    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer) return;
        if (Input.GetKeyDown("space"))
        {
            CmdShoot();
        }
    }


  
    void createBullet()
    {
        GameObject bullet = Instantiate(bulletPrebab, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
        // bullet.GetComponent<Rigidbody>().AddForce(bulletSpawn.transform.forward * 2000);
        bullet.GetComponent<Rigidbody>().velocity = bulletSpawn.transform.forward * 50;
        // NetworkServer.Spawn(bullet);
        Destroy(bullet, 3.5f);
    }


    [ClientRpc]
    void RpcCreateBullet()
    {
        if (!isServer)
        {
            createBullet();
        }
    }


    [Command]
    void CmdShoot()
    {
        createBullet();
        RpcCreateBullet();
    }


    

}
