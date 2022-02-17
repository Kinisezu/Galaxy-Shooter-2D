using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField]
    private int _waveNumber;
    private int _score;

    private SpawnManager _spawnManager;
    private UIManager _uiManager;
    private Player _player;

    // Start is called before the first frame update
    void Start()
    {
        _waveNumber = 1;

        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _player = GameObject.Find("Player").GetComponent<Player>();

        if (_spawnManager == null)
        {
            Debug.LogError("SpawnManager is NULL");
        }
        if (_uiManager == null)
        {
            Debug.LogError("UIManager is NULL");
        }
        if(_player == null)
        {
            Debug.LogError("Player is NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void AddWave()
    {
        _score = _player.GetScore();

        if (_score % 50 == 0)
        {
            _waveNumber++;
            _uiManager.UpdateWave(_waveNumber);
            StartCoroutine(WaveSpawn());
        }
    }

    public int GetWave()
    {
        return _waveNumber;
    }

    IEnumerator WaveSpawn()
    {
        _spawnManager.OnPlayerDeath();
        yield return new WaitForSeconds(3.0f);
        _spawnManager.OnNextWave();
        _spawnManager.StartSpawning();
    }


}
