using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killzone : MonoBehaviour
{
    public Enemigo enemigoRef;
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            enemigoRef.spawnerRef.canSpawn = true;
            enemigoRef.gameObject.SetActive(false);
        }
    }
}
