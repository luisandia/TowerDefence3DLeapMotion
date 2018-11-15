using UnityEngine;
using Leap.Unity.Interaction;
using System.Collections;
using System.Collections.Generic;


using System;

[AddComponentMenu("")]
public class Enemy : MonoBehaviour {

	public float speed = 10f;

	private Transform target;

	public GameObject impactEffect;

	private int wavepointIndex = 0;
	

	void Start ()
	{
		target = Waypoints.points[0];
		PhysicsCallbacks.OnPostPhysics += OnPostPhysics;
	}

    private void OnPostPhysics()
    {
		Renderer rend = GetComponent<Renderer>();

		InteractionBehaviour intObjEnemy =  gameObject.GetComponent<InteractionBehaviour>();
		if( intObjEnemy.isGrasped){
			//rend.material.color = Color.clear;
			GameObject effectIns = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
			Destroy(effectIns, 2f);
			Destroy(this.gameObject);
			return;
		}
    }

    void Update ()
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