using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject[] powerups;
    [SerializeField]
    private bool _stopSpawning = false;
    [SerializeField]
    private int _waveNumber;
    [SerializeField]
    private int _waitSeconds;

    private WaveManager _waveManager;


    // Start is called before the first frame update
    void Start()
    {
        _waveNumber = 1;

        _waveManager = GameObject.Find("Wave_Manager").GetComponent<WaveManager>();
        if (_waveManager == null)
        {
            Debug.LogError("WaveManager is NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
        StartCoroutine(SpawnRarePowerupRoutine());
    }


    IEnumerator SpawnEnemyRoutine()
    {
        int count = 0;

        yield return new WaitForSeconds(2.0f);

        while (_stopSpawning == false && count < 5)
        {
            if (_waveNumber < 6)
            {
                _waitSeconds = 6;
                _waitSeconds -= _waveNumber;
            }
            else
            {
                _waitSeconds = 1;
            }
            Vector3 posToSpawn = new Vector3(Random.Range(-9.0f, 9.0f), 7.5f, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            count++;


            yield return new WaitForSeconds(_waitSeconds);
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(2.0f);

        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-9.0f, 9.0f), 7.5f, 0);
            int randomPowerUp = Random.Range(0, 5);
            Instantiate(powerups[randomPowerUp], posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3.0f, 8.0f));
        }
    }

    IEnumerator SpawnRarePowerupRoutine()
    {
        yield return new WaitForSeconds(20.0f);

        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-9.0f, 9.0f), 7.5f, 0);
            int randomRarePowerUp = Random.Range(5, 7);
            Instantiate(powerups[randomRarePowerUp], posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(20.0f, 30.0f));
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
        DestroyAll("Enemy", "Powerup", "EnemyLaser");
    }
    public void OnNextWave()
    {
        _waveNumber = _waveManager.GetWave();
        _stopSpawning = false;
    }

    public void DestroyAll(string Enemy, string Powerup, string EnemyLaser)
    {
        GameObject[] enemy = GameObject.FindGameObjectsWithTag(Enemy);
        foreach (GameObject target in enemy)
        {
            GameObject.Destroy(target);
        }
        GameObject[] powerup = GameObject.FindGameObjectsWithTag(Powerup);
        foreach (GameObject target in powerup)
        {
            GameObject.Destroy(target);
        }
        GameObject[] laser = GameObject.FindGameObjectsWithTag(EnemyLaser);
        foreach (GameObject target in laser)
        {
            GameObject.Destroy(target);
        }
    }

}
