using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thrusters : MonoBehaviour
{
    private Player _player;
    // Start is called before the first frame update
    void Start()
    {
        _player = GetComponent<Player>();

        if (_player == null)
        {
            Debug.LogError("Player is NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {
        ThrusterBoost();
    }

    void ThrusterBoost()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            _player.ThrusterSpeedActive();
        }

        else
        {
            _player.ThrusterSpeedInactive();
        }
    }
}
