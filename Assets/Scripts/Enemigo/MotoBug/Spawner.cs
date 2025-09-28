using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject enemy;
    public Transform playerPosition;
    public bool canSpawn = true;
    
    // Update is called once per frame
    void Update()
    {
        if (playerPosition.position.x < gameObject.transform.position.x - 20 &&
            playerPosition.position.x > gameObject.transform.position.x - 25 && canSpawn)
        {
            GameObject LocalEnemy = Instantiate(enemy, gameObject.transform.position, Quaternion.identity);
            LocalEnemy.SetActive(true);
            LocalEnemy.GetComponent<Enemigo>().spawnerRef = this;
            canSpawn = false;
        }

    }
}
