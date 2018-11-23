using UnityEngine;
using UnityEngine.Networking;

public class Enemy : NetworkBehaviour
{


    public float speed = 10f;


    private Transform target;
    private int wavepointIndex = 0;
    // private bool startGame = false;
    public int orientation = 0;
    void Start()
    {
        switch (orientation)
        {

            case 0:
                target = Waypoints.points[0];
                break;
            case 1:
                target = WaypointsRight.points[0];
                break;

        }
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
        switch (orientation)
        {
            case 0:
                if (wavepointIndex >= Waypoints.points.Length - 1)
                {
                    Destroy(gameObject);
                    return;
                }

                wavepointIndex++;
                target = Waypoints.points[wavepointIndex];
                break;
            case 1:
                if (wavepointIndex >= WaypointsRight.points.Length - 1)
                {
                    Destroy(gameObject);
                    return;
                }

                wavepointIndex++;
                target = WaypointsRight.points[wavepointIndex];
                break;
        }

    }

}
