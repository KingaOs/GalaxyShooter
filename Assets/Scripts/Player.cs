using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;

    private bool _isTripleShot;
    private bool _isSpeedBoost;
    [SerializeField]
    private GameObject _tripleShot;

    [SerializeField]
    private int _score;

    private UIManager _uiManager;

    private bool _isShieldActive;
    [SerializeField]
    private GameObject _shield;

    [SerializeField]
    private List<GameObject> _engines = new List<GameObject>();

    [SerializeField]
    private AudioClip _laserSoundEffect;

    [SerializeField]
    private AudioSource _audioSource;

    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();


        if (_uiManager == null)
        {

            Debug.Log("The UI Manager is NULL.");
        }


        if (_spawnManager == null)
            Debug.LogError("The Spawn Manager is NULL");
        if (_audioSource == null)
            Debug.Log("Audio Source not found!");
        else
            _audioSource.clip = _laserSoundEffect;

    }



    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            _canFire = Time.time + _fireRate;
            FireLaser();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            _speed = 10;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _speed = 5;
        }

    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(direction * Time.deltaTime * _speed);

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), transform.position.z);

        if (transform.position.x > 11.3f)
            transform.position = new Vector3(-11.3f, transform.position.y, transform.position.z);
        else if (transform.position.x < -11.3f)
            transform.position = new Vector3(11.3f, transform.position.y, transform.position.z);
    }

    void FireLaser()
    {
        _canFire = Time.time + _fireRate;

        if (_isTripleShot)
        {
            Instantiate(_tripleShot, transform.position + new Vector3(0, 0, 0), Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.3f, 0), Quaternion.identity);
        }

        _audioSource.Play();

    }

    public void Damage()
    {
        if (_isShieldActive)
        {
            _isShieldActive = false;
            _shield.SetActive(false);
            return;
        }
        _lives -= 1;


        
        _uiManager.UpdateLives(_lives);

        if (_lives < 1)
        {
            _spawnManager.OnPLayerDeath();
            Destroy(this.gameObject);
        }

        var engine = Random.Range(0, _engines.Count);
        _engines[engine].SetActive(true);
        _engines.RemoveAt(engine);

    }


    public void TripleShotActive()
    {
        _isTripleShot = true;
        StartCoroutine(CoolDownPowerup());
    }

    IEnumerator CoolDownPowerup()
    {
        yield return new WaitForSeconds(5f);
        _isTripleShot = false;

    }

    public void SpeedBoostActive()
    {
        _speed *= 2;
        StartCoroutine(SpeedBoostCoolDownRoutine());
    }

    IEnumerator SpeedBoostCoolDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _speed = 5f;
    }

    public void ShieldActive()
    {
        _isShieldActive = true;
        _shield.SetActive(true);
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }
}
