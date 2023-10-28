using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        
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
        Player player = other.transform.GetComponent<Player>();
        Debug.Log(this.tag);

        if (other.tag == "Player")
        {
            if (player != null)
            {
                switch (this.tag)
                {
                    case "TripleShot":
                        player.TripleShotPowerup();
                        break;
                    case "SpeedUp":
                        player.SpeedPowerup();
                        break;
                    case "ShieldUp":
                        player.ShieldPowerup();
                        break;
                    default:
                        Debug.Log("Default Case");
                        break;
                }
            } 
            Destroy(this.gameObject);
        }
          
    }
}


