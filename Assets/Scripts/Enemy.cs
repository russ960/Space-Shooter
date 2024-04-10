using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;
    [SerializeField]
    private GameObject _enemyLaserPreFab;
    private Player _player;
    [SerializeField]
    private AudioClip _explosionSoundClip;
    [SerializeField]
    private Vector3 _laserOffset = new Vector3 (0.0f, 1.05f, 0);
    Animator enemyAnimator;
    private float _fireRate = 3.0f;
    private float _canFire = -1;
    
    void ResetPostionRandom() {
        float transformX = Random.Range(-9.0f, 9.0f);
        transform.position = new Vector3(transformX, 7.4f, 0);
        
     }

    // Start is called before the first frame update
    void Start()
    {
         ResetPostionRandom();
         _player = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Player>();
         if (_player == null)
         {
            Debug.LogError("Player object does not exist.");
         }
         enemyAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(3f,7f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_enemyLaserPreFab, this.transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
            for (int i=0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();

            }
        } 
    }

    private void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y < -5.4)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        

        if (other.transform.name == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            
            if (player != null)
            {
                player.Damage();
                _speed = 0;
                enemyAnimator.SetTrigger("OnEnemyDeath");
                Destroy(this.gameObject, 2.8f);
            }
            else
            {
               Destroy(this.gameObject);
            }
        }

        if (other.transform.tag == "Laser")
        {
            if (transform.parent != null)
            {
                if (_player != null)
                {
                    _player.PlayerScored(10);
                    
                }
                _speed = 0;
                enemyAnimator.SetTrigger("OnEnemyDeath");
                Destroy(GetComponent<Collider2D>());
                Destroy(this.gameObject, 2.8f);
            }
        }
        AudioSource.PlayClipAtPoint(_explosionSoundClip, transform.position);
    }
}

