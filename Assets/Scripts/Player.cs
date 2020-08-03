using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
    private float _speedMultipler = 2;

    [SerializeField]
    private GameObject _laserPrefab;

    [SerializeField]
    private GameObject _tripleshotPrefab;

    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;

    //variable is tripleshotactive
    [SerializeField]
    private bool _isTripleShotActive = false;
    [SerializeField]
    private bool _isSpeedBoostActive = false;
    [SerializeField]
    private bool _isShieldActive = false;

    //variable reference to the shield visualiser
    [SerializeField]
    private GameObject _shieldvisualiser;

    [SerializeField]
    private int _score;

    private UIManager _uiManager;

    // Start is called before the first frame update
    void Start()
    {
        //current position = new position(0,0,0)
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        if(_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL.");
        }

        if(_uiManager == null)
        {
            Debug.LogError("The UI Manager is NULL.");
        }
        
    }

    // Update is called once per frame
    void Update()
    {

        CalculateMovement();

        //if I hit the space key
        //spawn gameObject
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
             }
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        float verticalInput = Input.GetAxis("Vertical");

        // transform.Translate(Vector3.right * horizontalInput * _speed * Time.deltaTime);
        // transform.Translate(Vector3.up * verticalInput * _speed * Time.deltaTime);

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        //if sppedboostactive is false - normal speed
        //else speed boost multipler
       
            transform.Translate(direction * _speed * Time.deltaTime);
                   

        //if player position on the y is greater than 0
        //y position = 0
        //else if position on the y is less than -3.8f
        //y pos = -3.8f

      //  if (transform.position.y >= 0)
       // {
       //     transform.position = new Vector3(transform.position.x, 0, 0);
       // }
       // else if (transform.position.y <= -3.8f)
       // {
       //     transform.position = new Vector3(transform.position.x, -3.8f, 0);
       // }

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.6f,0),0);

        //if player on the x > 11
        //x pos = -11
        //else if player on the x is less than - 11
        //x pos = 11

        if (transform.position.x > 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);

        }
        else if (transform.position.x < -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }

    void FireLaser()
    {
        _canFire = Time.time + _fireRate;

        if (_isTripleShotActive == true)
        {
            Instantiate(_tripleshotPrefab,transform.position,Quaternion.identity);
        }
        else
        {

            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
        }
        //if space key press, 
        //if tripleshot is true
            //fire 3 lasers (triple shot prefab)

        //else fire 1 laser

        //instantiate 3 lasers (triple shot prefab)
    }

    public void Damage()
    {
        //if shields is active
        //do nothing
        //deactivate shields
        //return;
        if(_isShieldActive == true)
        {
            _isShieldActive = false;

            //disable the visualiser
            _shieldvisualiser.SetActive(false);

            return;
        }

        _lives--;

        _uiManager.UpdateLives(_lives);

        //check if dead
        //destroy us
        if(_lives < 1)
        {
            //Communicate with Spawn Manager
            _spawnManager.OnPlayerDeath();

            Destroy(this.gameObject);
        }
    }

    public void TripleShotActive()
    {
        //tripleShotActive becomes true
        //start the power down coroutine for triple shot 
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    //IEnumerator TripleShotPowerDownRoutine
    //wait 5 seconds
    //set the triple shot to false
    IEnumerator TripleShotPowerDownRoutine()
    {

        yield return new WaitForSeconds(5.0f);

        _isTripleShotActive = false;
    }

    public void SpeedBoostActive()
    {
        _isSpeedBoostActive = true;
        _speed *= _speedMultipler;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _speed /= _speedMultipler;
        _isSpeedBoostActive = false;
    }

    public void ShieldActive()
    {
        _isShieldActive = true;

        //enable the shield visualiser
        _shieldvisualiser.SetActive(true);
    }

    //method to increase score
    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }
}
