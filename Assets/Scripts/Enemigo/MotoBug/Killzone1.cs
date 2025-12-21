using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillzoneCangrejo : MonoBehaviour
{
    public GameObject cangrejoobjeto;
    public bool killzoneFirst = false;
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.addScore(100);
            
            killzoneFirst = true;
            Destroy(cangrejoobjeto);
        }
    }
    
    
}
