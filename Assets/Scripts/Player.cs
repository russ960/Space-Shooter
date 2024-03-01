using System.Collections;
using System.Collections.Generic;
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

    // Start is called before the first frame update
    void Start()
    {
        // Set the start position to 0, 0, 0
        transform.position = new Vector3(0, 0, 0);
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
        CalculateMovement();
        #if UNITY_ANDROID
            if (Input.GetKeyDown(KeyCode.Space) || CrossPlatformInputManager.GetButtonDown("Fire") && Time.time > _canFire)
            {
                FireLaser();
            }
        #else
            if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
            {
                FireLaser();
            }
        #endif


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
            Instantiate(_tripleShotPreFab, transform.position, Quaternion.identity);
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
