using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_Manager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPreFab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    //private GameObject _powerPreFab;
    private GameObject[] _powerPreFab;

    private bool _stopSpawning = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   IEnumerator SpawnEnemyRoutine()
   {
        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8.0f, 8.0f), 7.0f, 0);     
            GameObject newEnemy = Instantiate(_enemyPreFab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(2.0f);
        }
   }

   IEnumerator SpawnPowerUpRoutine()
   {
        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8.0f, 8.0f), 7.0f, 0);     
            int randomPowerUp = Random.Range(0,3);
            GameObject newPowerUp = Instantiate(_powerPreFab[randomPowerUp], posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3,8));
        }
   }

   public void OnPlayerDeath()
   {
          _stopSpawning = true;
   }
}
