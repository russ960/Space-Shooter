using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private int _score;
    private UI_Manager _uiManager;
    private Canvas _gameOver; 

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

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }

        if (_uiManager != null)
        {
            _uiManager.SetScore(_score);
        }    
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
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
        StartCoroutine(TripleShotPowerdownCoroutine());
    }

    public void ShieldPowerup()
    {
        _isShieldUp = true;
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
        StartCoroutine(SpeedPowerdownCoroutine());
    }

    IEnumerator SpeedPowerdownCoroutine()
    {
        yield return new WaitForSeconds(5);
        _speed = _speed / _speedMultiplyer;
    }
}
