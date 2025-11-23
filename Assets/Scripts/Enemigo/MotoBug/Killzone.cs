using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killzone : MonoBehaviour
{
    public Enemigo enemigoRef;
    public bool killzoneFirst = false;
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            killzoneFirst = true;
            enemigoRef.spawnerRef.canSpawn = true;
            enemigoRef.gameObject.SetActive(false);
        }
    }
    
    
}
