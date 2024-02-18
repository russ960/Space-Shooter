using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astroid : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;
    [SerializeField]
    private GameObject _explosionPreFab;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * _speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.tag == "Laser")
        {
            if (transform.parent != null)
            {
                Instantiate(_explosionPreFab, transform.position, Quaternion.identity);
                Destroy(other.gameObject);
                Destroy(this.gameObject);
            }
        }
    }
}
