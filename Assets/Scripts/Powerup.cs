using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3;
    [SerializeField]
    private int _powerupID;

    [SerializeField]
    private AudioClip _powerUpSoundEffect;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * Time.deltaTime * _speed);

        if (transform.position.y < -5.8f)
        {

            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Player player;
            player = collision.GetComponent<Player>();

            AudioSource.PlayClipAtPoint(_powerUpSoundEffect, transform.position);
            if (player != null)
            {
                switch (_powerupID)
                {
                    case 0:
                        player.TripleShotActive();
                        break;
                    case 1:
                        player.SpeedBoostActive();
                        break;
                    case 2:
                        player.ShieldActive();
                        break;
                    default:
                        Debug.Log("Powerup unkown");
                        break;


                }

            }

            Destroy(this.gameObject);
        }
    }
}
