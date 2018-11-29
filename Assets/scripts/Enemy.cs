using UnityEngine;
using UnityEngine.Networking;

public class Enemy : NetworkBehaviour
{


    public float speed = 3.5f;


    private Transform target;
    private int wavepointIndex = 0;

    public float[] wavePointAngle;
    // private bool startGame = false;
    public int orientation = 0;

    void initAngles()
    {
        wavePointAngle = new float[15];
        if(orientation == 0)
        {
            wavePointAngle[0] = 90f;
            wavePointAngle[1] = 90f;
            wavePointAngle[2] = -90f;
            wavePointAngle[3] = -90f;
            wavePointAngle[4] = -90f;
            wavePointAngle[5] = 90f;
            wavePointAngle[6] = 90f;
            wavePointAngle[7] = 90f;
            wavePointAngle[8] = -90f;
            wavePointAngle[9] = 90f;
            wavePointAngle[10] = -90f;
            wavePointAngle[11] = -90f;
            wavePointAngle[12] = -90f;
        }
        else
        {
            wavePointAngle[0] = -90f;
            wavePointAngle[1] = -90f;
            wavePointAngle[2] = 90f;
            wavePointAngle[3] = 90f;
            wavePointAngle[4] = 90f;
            wavePointAngle[5] = -90f;
            wavePointAngle[6] = -90f;
            wavePointAngle[7] = -90f;
            wavePointAngle[8] = 90f;
            wavePointAngle[9] = -90f;
            wavePointAngle[10] = 90f;
            wavePointAngle[11] = 90f;
            wavePointAngle[12] = 90f;
        }
    }
    void Start()
    {
        initAngles();
        transform.Rotate(Vector3.up, -90f, Space.World);
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
            float angle = Vector3.Angle(transform.position, target.position);
            transform.Rotate(Vector3.up, wavePointAngle[wavepointIndex], Space.World);
            Debug.LogWarning("El angulo del enemigo contra el target es: ");
            Debug.LogWarning(angle);
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
