using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;

    //ID for Powerups
    //0 = Triple Shot
    //1 = Speed
    //2 = Shields
    [SerializeField]
    private int _powerupID;

    [SerializeField]
    private AudioClip _clip;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //move down at a speed of 3
        //when we leave the screen, destroy this object
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if(transform.position.y < -4.5f)
        {
            Destroy(this.gameObject);
        }

    }

    //OnTriggerCollision
    //Only be collectable by the Player (HINT: Use Tags)
    //on collected , destroy
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            //communicate with player
            Player player = other.transform.GetComponent<Player>();

            AudioSource.PlayClipAtPoint(_clip,transform.position);

            if(player != null)
            {
                //if powerup id is 0
                //else if powerup id is 1
                //play speed powerup
                //else if powerup id is 2
                //play shield powerup
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
                        break;
                }
              

            }

            Destroy(this.gameObject);
        }
    }
}
