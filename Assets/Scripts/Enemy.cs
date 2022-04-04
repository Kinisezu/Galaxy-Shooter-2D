using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;
    private float _startingXPos;
    [SerializeField]
    private GameObject _laserPrefab;

    private Player _player;
    private Animator _anim;
    private WaveManager _waveManager;

    [SerializeField]
    private AudioClip _explosion;

    private float _canFire = -1.0f;
    private float _fireRate = 3.0f;

    [SerializeField]
    private GameObject _enemyShield;
    private GameObject _enemy;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _waveManager = GameObject.Find("Wave_Manager").GetComponent<WaveManager>();

        _enemyShield = _enemy.transform.Find("Enemy_Shield").gameObject;
        _enemyShield = _enemy.transform.Find("Enemy/Enemy_Shield").gameObject;

        _enemyShield.SetActive(false);


        _startingXPos = transform.position.x;

        if(_player == null)
        {
            Debug.LogError("Player is NULL");
        }
        _anim = GetComponent<Animator>();
        if (_anim == null)
        {
            Debug.LogError("Animator is NULL");
        }
        if(_waveManager == null)
        {
            Debug.LogError("Wave Manager is NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        ZigZagMovement();

        if (Time.time > _canFire && _speed > 0f)
        {
            _fireRate = Random.Range(3.0f, 7.0f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }

        }
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -5.5f)
        {
            transform.position = new Vector3(Random.Range(-9.0f, 9.0f), 7.5f, 0);
        }
    }

    void ZigZagMovement()
    {
        if (transform.position.y < 4)
        {
            if (_startingXPos > 0)
            {
                transform.Translate(Vector3.left * _speed * Time.deltaTime);
            }
            else if (_startingXPos <= 0)
            {
                transform.Translate(Vector3.right * _speed * Time.deltaTime);
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if(player != null)
            {
                player.Damage();
            }

            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            AudioSource.PlayClipAtPoint(_explosion, this.gameObject.transform.position);
            Destroy(this.gameObject, 2.5f);
        }

        if(other.tag == "Laser")
        {
            if (_enemyShield.activeInHierarchy)
            {
                _enemyShield.SetActive(false);
            }
            else
            {
                Destroy(other.gameObject);
                if (_player != null)
                {
                    _player.AddScore(10);
                }

                _anim.SetTrigger("OnEnemyDeath");
                _speed = 0;
                AudioSource.PlayClipAtPoint(_explosion, this.gameObject.transform.position);

                Destroy(GetComponent<Collider2D>());
                Destroy(this.gameObject, 2.5f);
            }
        }
    }

    public void EnableEnemyShield()
    {
        _enemyShield.SetActive(true);
    }
}
