using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;
public class WaveSpawner : NetworkBehaviour
{

    public GameObject enemyPrefab;

    public GameObject spawnPoint;

    public float timeBetweenWaves = 5f;
    private float countdown = 2f;

    public Text waveCountdownText;

    [SyncVar]
    private int waveIndex = 0;

    BuildManager buildManager;

    void Start()
    {
        buildManager = BuildManager.instance;

    }
    void Update()
    {
        // if (buildManager.startGame)
        {
            if (countdown <= 0f)
            {
                StartCoroutine(SpawnWave());
                countdown = timeBetweenWaves;
            }

            countdown -= Time.deltaTime;

            // waveCountdownText.text = Mathf.Round(countdown).ToString();
        }
    }

    IEnumerator SpawnWave()
    {
        waveIndex++;

        for (int i = 0; i < waveIndex; i++)
        {
            if(isLocalPlayer)
            CmdSpawnEnemy();
            yield return new WaitForSeconds(1.5f);
        }
    }
    [Command]    
    void CmdSpawnEnemy()
    {
        GameObject obj = Instantiate(enemyPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
        NetworkServer.Spawn(obj);
    }

}
