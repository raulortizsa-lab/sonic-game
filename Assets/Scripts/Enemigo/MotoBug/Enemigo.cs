using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Enemigo : MonoBehaviour
{
    private Transform enemyTranform;
    public float speed;
    public Spawner spawnerRef;
    
    // Start is called before the first frame update
    void Start()
    {
        enemyTranform = gameObject.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currentPosition = enemyTranform.position;
        currentPosition.x -= 0.1f * Time.deltaTime * speed;
        enemyTranform.position = currentPosition;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            
        }
    }
}
