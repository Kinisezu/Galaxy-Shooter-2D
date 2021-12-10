using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5.0f;
    private float _speedMultiplier = 2;
    [SerializeField]
    private float _thrusterSpeed = 15f;
    private float _normalSpeed = 5.0f;

    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private GameObject _shieldVisualizer;
    [SerializeField]
    private GameObject _rightEngineDamage;
    [SerializeField]
    private GameObject _leftEngineDamage;
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1.0f;
    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private AudioClip _laserSound;
    private AudioSource _audioSource;

    [SerializeField]
    private bool _isTripleShotActive = false;
    [SerializeField]
    private bool _isShieldActive = false;

    [SerializeField]
    private int _score;


    private SpawnManager _spawnManager;

    private UIManager _uiManager;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);

        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();

        if (_spawnManager == null)
        {
            Debug.Log("The Spawn Manager is NULL");
        }

        if (_uiManager == null)
        {
            Debug.Log("The UI Manager is NULL");
        }

        if(_audioSource == null)
        {
            Debug.LogError("AudioSource on Player is NULL");
        }
        else
        {
            _audioSource.clip = _laserSound;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }

    }

    void CalculateMovement()
    {
        float _horizontalInput = Input.GetAxis("Horizontal");
        float _verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(_horizontalInput, _verticalInput, 0);

        transform.Translate(direction * _speed * Time.deltaTime);

        if (transform.position.x >= 11.5f)
        {
            transform.position = new Vector3(-11.5f, transform.position.y, 0);
        }
        else if (transform.position.x <= -11.5f)
        {
            transform.position = new Vector3(11.5f, transform.position.y, 0);
        }

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.5f, 0), 0);
    }

    void FireLaser()
    {
        _canFire = _fireRate + Time.time;

        if (_isTripleShotActive)
        {
            Instantiate(_tripleShotPrefab, transform.position + new Vector3(0, -0.23f, 0), Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.08f, 0), Quaternion.identity);
        }

        _audioSource.Play();
    }

    public void Damage()
    {
        if (_isShieldActive)
        {
            _isShieldActive = false;
            _shieldVisualizer.SetActive(false);
            return;
        }

        _lives--;
        _uiManager.UpdateLives(_lives);

        switch(_lives)
        {
            case 2:
                _rightEngineDamage.gameObject.SetActive(true);
                break;
            case 1:
                _leftEngineDamage.gameObject.SetActive(true);
                break;
            default:
                break;
        }

        if (_lives < 1)
        {
            Destroy(this.gameObject);

            _spawnManager.OnPlayerDeath();
        }
    }

    public void TripleShotEnabled()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }

    public void SpeedBoostEnabled()
    {
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedPowerDownRoutine());
    }

    IEnumerator SpeedPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _speed /= _speedMultiplier;
    }

    public void ShieldsEnabled()
    {
        _isShieldActive = true;
        _shieldVisualizer.SetActive(true);
    }
    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "EnemyLaser")
        {
            Destroy(other.gameObject);
            Damage();
        }
    }

    public void ThrusterSpeedActive()
    {
        _speed = _thrusterSpeed;
    }

    public void ThrusterSpeedInactive()
    {
        _speed = _normalSpeed;
    }
}