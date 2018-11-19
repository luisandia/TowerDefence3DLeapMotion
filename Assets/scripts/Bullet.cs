using UnityEngine;

// public class Bullet : using UnityEngine;
using UnityEngine.Networking;

public class Bullet : NetworkBehaviour {
	
	[SyncVar]
	private Transform target;

	public float speed = 70f;
	public GameObject impactEffect;
	
	public void Seek (Transform _target)
	{
		target = _target;
	}

	// Update is called once per frame
	void Update () {

		if (target == null)
		{
			Destroy(gameObject);
			return;
		}

		Vector3 dir = target.position - transform.position;
		float distanceThisFrame = speed * Time.deltaTime;

		if (dir.magnitude <= distanceThisFrame)
		{
			CmdHitTarget();
			return;
		}

		transform.Translate(dir.normalized * distanceThisFrame, Space.World);
		// transform.Translate(dir.normalized * 50, Space.World);

        // GameObject bullet = Instantiate(bulletPrebab, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
        // bullet.GetComponent<Rigidbody>().AddForce(bulletSpawn.transform.forward * 2000);
        // this.velocity = bulletSpawn.transform.forward * 50;


	}

	// [Command]
	void CmdHitTarget ()
	{
		GameObject effectIns = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
		Destroy(effectIns, 2f);

		Destroy(target.gameObject);
		Destroy(gameObject);
	}
}
