using UnityEngine;
using UnityEngine.Networking;

public class Enemy : NetworkBehaviour {
    

    public float speed = 10f;

    
    private Transform target;
    private int wavepointIndex = 0;
    // private bool startGame = false;

    void Start()
    {
        target = Waypoints.points[0];
    }

     void OnCollisionEnter(Collision collision)
    {
        Debug.Log("colision firecontrol");
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.tag == "Bullet")
        {
            Debug.Log("Enemyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyy");
            Destroy(collision.gameObject);
            Destroy(this.gameObject);
            // CmdChangeHealth(-5);
        }
    }
    void Update()
    {

        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, target.position) <= 0.4f)
        {
            GetNextWaypoint();
        }

    }

    void GetNextWaypoint()
    {
        if (wavepointIndex >= Waypoints.points.Length - 1)
        {
            Destroy(gameObject);
            return;
        }

        wavepointIndex++;
        target = Waypoints.points[wavepointIndex];
    }

}
