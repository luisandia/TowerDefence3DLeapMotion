
using UnityEngine;

public class BuildManager : MonoBehaviour {

	//singleton
	public static BuildManager instance;

	void Awake() {
		if (instance != null){
			Debug.LogError("More than one BuildManager in scene!");
		}
		instance = this;	
	} 
	public GameObject standartTurretPrefab;

	private GameObject turretToBuild;

	public GameObject GetTurretToBuild(){
		return turretToBuild;
	}



	// Use this for initialization
	void Start () {
		turretToBuild=standartTurretPrefab;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
