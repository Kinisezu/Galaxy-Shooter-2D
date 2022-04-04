using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissile : MonoBehaviour
{
    //[SerializeField]
    private Transform _target;
    [SerializeField]
    private float _speed = 3f;
    [SerializeField]
    private float _rotateSpeed = 200f;

    private Player player;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player").GetComponent<Player>();
        _target = player.transform;

        if (rb == null)
        {
            Debug.LogError("RigidBody is NULL");
        }
        if(player == null)
        {
            Debug.LogError("Player is NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 direction = (Vector2)_target.position - rb.position;

        direction.Normalize();

        float _rotateAmount = Vector3.Cross(direction, transform.up).z;

        rb.angularVelocity = -_rotateAmount * _rotateSpeed;

        rb.velocity = transform.up * _speed;

        Destroy(this.gameObject, 5f);

    }   
}
