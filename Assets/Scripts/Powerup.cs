using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;
    //ID for powerups
    //0 = triple shot
    //1 = speed
    //2 = shields
    [SerializeField]
    private int powerupID;
    [SerializeField]
    private AudioClip _audioClip;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if(transform.position.y < -5.75f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Destroy(this.gameObject);
            Player player = other.GetComponent<Player>();

            AudioSource.PlayClipAtPoint(_audioClip, transform.position);

            if (player != null)
            {
                switch (powerupID)
                {
                    case 0:
                        player.TripleShotEnabled();
                        break;
                    case 1:
                        player.SpeedBoostEnabled();
                        break;
                    case 2:
                        player.ShieldsEnabled();
                        break;
                    case 3:
                        player.AmmoRefill();
                        break;
                    case 4:
                        player.HealthRefill();
                        break;
                    case 5:
                        player.MultiShotEnabled();
                        break;
                    case 6:
                        player.NegativeEnabled();
                        break;
                    default:
                        Debug.Log("Default Case");
                        break;
                }
            }  
        }
    }
}
