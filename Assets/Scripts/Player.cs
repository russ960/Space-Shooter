using System.Collections;
using System.Collections.Generic;
using Packages.Rider.Editor.UnitTesting;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
    [SerializeField]
    private int _speedMultiplyer = 3;

    [SerializeField]
    private GameObject _laserPreFab;
    [SerializeField]
    private GameObject _tripleShotPreFab;
    [SerializeField]
    private Vector3 _laserOffset = new Vector3 (0.0f, 1.05f, 0);
    [SerializeField]
    private Vector3 _laserOffsetTripleShot = new Vector3 (-0.25f, 1.05f, 0);
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    private Spawn_Manager _spawnManager;
    [SerializeField]
    private bool _isTripleShotActive = false;
    [SerializeField]
    private bool _isShieldUp = false;
    [SerializeField]
    private GameObject _shieldVisualizer;
    [SerializeField]
    private GameObject _rightDamage;
    [SerializeField]
    private GameObject _leftDamage;
    [SerializeField]
    private int _score;
    private UI_Manager _uiManager;
    private Canvas _gameOver; 
    [SerializeField]
    private AudioClip _laserAudio;
    [SerializeField]
    private AudioClip _powerupSoundClip;
    [SerializeField]
    private Animator _animator;

    public bool isPlayerOne = false;
    public bool isPlayerTwo = false;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        // Set the start position to 0, 0, 0
        switch(transform.name) 
        {
        case "Player":
            transform.position = new Vector3(0, 0, 0);
            break;
        case "Player1":
            transform.position = new Vector3(3, 0, 0);
            break;
        case "Player2":
            transform.position = new Vector3(-3, 0, 0);
            break;    
        default:
            transform.position = new Vector3(0, 0, 0);
            break;
        }

        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<Spawn_Manager>();

        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn_Manager is NULL");
        }

        _uiManager = GameObject.Find("Canvas").GetComponent<UI_Manager>();

        if (_uiManager == null)
        {
            Debug.LogError("UI_Manager is NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.name == "Player" || transform.name == "Player1")
        {
            CalculateMovement();
        }
        else if (transform.name == "Player2")
        {
            CalculateMovementPlayerTwo();
        }
        
        #if UNITY_ANDROID
            if (Input.GetKeyDown(KeyCode.Space) || CrossPlatformInputManager.GetButtonDown("Fire") && Time.time > _canFire)
            {
                FireLaser();
            }
        #else
            if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire && (transform.name == "Player" || transform.name == "Player1")) 
            {
                FireLaser();
            }
        #endif

            if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time > _canFire && transform.name == "Player2")
            {
                FireLaserPlayerTwo();
            }

        if (_uiManager != null)
        {
            _uiManager.SetScore(_score);
        }    
    }

    void CalculateMovement()
    {
        #if UNITY_ANDROID
            float horizontalInput = CrossPlatformInputManager.GetAxis("Horizontal");     //Input.GetAxis("Horizontal");
            float verticalInput = CrossPlatformInputManager.GetAxis("Vertical"); //Input.GetAxis("Vertical"); 
        #else
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
        #endif

        _animator.SetFloat("Direction", Input.GetAxisRaw("Horizontal"));
        transform.Translate(Vector3.right * horizontalInput * _speed * Time.deltaTime);
        transform.Translate(Vector3.up * verticalInput * _speed * Time.deltaTime);            

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);
        
        if (transform.position.x > 11)
        {
            transform.position = new Vector3(-11.0f, transform.position.y, 0);
        }       
        else if (transform.position.x < -11)
        {
            transform.position = new Vector3(11.0f, transform.position.y, 0);
        }
    }

    void FireLaser()
    {
        _canFire = Time.time + _fireRate;
        if (_isTripleShotActive == true)
        {
            Instantiate(_tripleShotPreFab, transform.position + _laserOffsetTripleShot, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPreFab, transform.position + _laserOffset, Quaternion.identity);
        }
        AudioSource.PlayClipAtPoint(_laserAudio, transform.position);
    }

    void CalculateMovementPlayerTwo()
    {
        if (Input.GetKey(KeyCode.A))
        {
            _animator.SetFloat("Direction", -1);
            transform.Translate(Vector3.left * _speed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            _animator.SetFloat("Direction", 1);
            transform.Translate(Vector3.right * _speed * Time.deltaTime);
        }
        else
        {
            _animator.SetFloat("Direction", 0);
        }

        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.up * _speed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
        }
        
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);
        
        if (transform.position.x > 11)
        {
            transform.position = new Vector3(-11.0f, transform.position.y, 0);
        }       
        else if (transform.position.x < -11)
        {
            transform.position = new Vector3(11.0f, transform.position.y, 0);
        }
    }

    void FireLaserPlayerTwo()
    {
        _canFire = Time.time + _fireRate;
        if (_isTripleShotActive == true)
        {
            Instantiate(_tripleShotPreFab, transform.position + _laserOffsetTripleShot, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPreFab, transform.position + _laserOffset, Quaternion.identity);
        }
        AudioSource.PlayClipAtPoint(_laserAudio, transform.position);
    }

    public void Damage()
    {
        if (_isShieldUp == true)
        {
            _isShieldUp = false;
            _shieldVisualizer.SetActive(false);
            return;
        }
        _lives -= 1;
        switch (_lives)
        {
            case 1:
            _leftDamage.SetActive(true);
            break;
            case 2: 
            _rightDamage.SetActive(true);
            break;
        }
        _uiManager.SetLives(_lives);

        

        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
            _uiManager.GameOverText();
        }
    }

    public void PlayerScored(int points)
    {
        _score += points;        
    }

    public void TripleShotPowerup()
    {
        _isTripleShotActive = true;
        AudioSource.PlayClipAtPoint(_powerupSoundClip, transform.position);
        StartCoroutine(TripleShotPowerdownCoroutine());
    }

    public void ShieldPowerup()
    {
        _isShieldUp = true;
        AudioSource.PlayClipAtPoint(_powerupSoundClip, transform.position);
        _shieldVisualizer.SetActive(true);
    }

    IEnumerator TripleShotPowerdownCoroutine()
    {
        yield return new WaitForSeconds(5);
        _isTripleShotActive = false;
    }

    public void SpeedPowerup()
    {
        _speed = _speed * _speedMultiplyer;
        AudioSource.PlayClipAtPoint(_powerupSoundClip, transform.position);
        StartCoroutine(SpeedPowerdownCoroutine());
    }

    IEnumerator SpeedPowerdownCoroutine()
    {
        yield return new WaitForSeconds(5);
        _speed = _speed / _speedMultiplyer;
    }

}
