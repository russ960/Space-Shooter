using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;
    private Player _player;

    Animator enemyAnimator;
    
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
                Destroy(this.gameObject, 2.8f);
            }
        }
    }
}
