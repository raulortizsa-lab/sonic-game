using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Si toca un objeto con el tag "Platform"
        if (other.CompareTag("Platform"))
        {
            Destroy(gameObject); // Destruye este proyectil
        }

        // Si toca un objeto con el tag "Player"
        if (other.CompareTag("Player"))
        {
            other.GetComponent<player>().Damage();
            Debug.Log("El proyectil ha tocado al jugador!");
        }
    }
}
