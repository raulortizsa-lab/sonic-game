using System;
using UnityEngine;

public class CheckPoint_Manager : MonoBehaviour
{
    public bool checkpointActive = true;
    public Sprite activeSprite;
    public SpriteRenderer spriteRenderer;
    public Transform spawnPoint;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && checkpointActive)
        {
            Debug.Log("entro player");
            other.GetComponent<player>().transformToSpawn = spawnPoint;
            GetComponent<AudioSource>().Play();
            checkpointActive = false;
            spriteRenderer.sprite = activeSprite;
        }
    }
}
