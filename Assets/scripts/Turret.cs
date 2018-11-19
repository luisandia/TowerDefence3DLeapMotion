using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
public class Turret : NetworkBehaviour
{

    private Transform target;

    [Header("Attributes")]

    public float range = 15f;
    public float fireRate = 0.5f;
    [SyncVar]
    private float fireCountdown = 0f;

    [Header("Unity Setup Fields")]

    public string enemyTag = "Enemy";

    // [SyncVar]
    public Transform partToRotate;
    public float turnSpeed = 10f;

    public GameObject bulletPrefab;
    // [SyncVar]
    public Transform firePoint;

    // Use this for initialization
    void Start()
    {
        // firePoint= firePoint.transform.Find("FirePoint");
        InvokeRepeating("RpcUpdateTarget", 0f, 0.5f);
        // InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    // [ClientRpc]
    void RpcUpdateTarget()
    {
        // Debug.Log("updateTarget");
        // if (!isLocalPlayer) return;
        // CmdNewTarget();
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= range)
        {
            // Debug.Log("Enemigo encontrado");
            target = nearestEnemy.transform;
        }
        else
        {
            // Debug.Log("Enemigo no hay");
            target = null;
        }

    }

    [Command]
    void CmdNewTarget()
    {

    }

    // Update is called once per frame
    // void FixedUpdate()
    void Update()
    {
        // Debug.Log("Actualizando");
        // if (!isLocalPlayer) return;
        // RpcUpdateTarget();
        if (target == null)
            return;
        // Debug.Log("Actualizando parttorotate");
        //Target lock on
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);

        // firePoint= partToRotate.transform.Find("FirePoint");

        if (fireCountdown <= 0f)
        {
            Debug.Log("Disparando");
            if (isLocalPlayer)
            {
                CmdShoot();
                // Shoot();
                fireCountdown = 1f / fireRate;
            }
            else
            {
                Debug.Log("No tiene autoridad local turret");

            }
            // CmdShoot();
            Shoot();
            fireCountdown = 1f / fireRate;
        }

        fireCountdown -= Time.deltaTime;




    }

    [Command]
    void CmdShoot()
    {
        Debug.Log("CMDShoot");
        Shoot();
        RpcShoot();
    }
    [ClientRpc]
    void RpcShoot()
    {
        Shoot();
    }
    void Shoot()
    {
        GameObject bulletGO = (GameObject)Instantiate(bulletPrefab, firePoint.transform.position, firePoint.transform.rotation);
        Bullet bullet = bulletGO.GetComponent<Bullet>();
        // GameObject bullet = Instantiate(bulletPrebab, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
        // bullet.GetComponent<Rigidbody>().AddForce(bulletSpawn.transform.forward * 2000);
        bullet.GetComponent<Rigidbody>().velocity = firePoint.transform.forward * 50;


        if (bullet != null)
            bullet.Seek(target);
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
