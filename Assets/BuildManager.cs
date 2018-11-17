using UnityEngine;
using UnityEngine.Networking;

public class BuildManager : NetworkBehaviour
{
    public static BuildManager instance;

    // [SyncVar]
    public bool startGame = false;
	// [SyncVar(hook = "OnChangeAlreadyPlayer")]
    public short alreadyPlayer = 0;
    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one BuildManager in scene!");
            return;
        }
        instance = this;
    }

    public GameObject standardTurretPrefab;
    public GameObject anotherTurretPrefab;

    private GameObject turretToBuild;


    public override void OnStartClient()
    {
        base.OnStartClient();
        Invoke("UpdateStates", 1);
    }
    void OnChangeAlreadyPlayer(short n)
    {
        alreadyPlayer = n;
    }
    void UpdateStates()
    {
        OnChangeAlreadyPlayer(alreadyPlayer);
        // OnChangeName(pName);
    }

    public GameObject GetTurretToBuild()
    {
        return turretToBuild;
    }

    public void SetTurretToBuild(GameObject turret)
    {
        turretToBuild = turret;
    }

}
