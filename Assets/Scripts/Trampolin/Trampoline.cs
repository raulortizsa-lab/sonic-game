using UnityEngine;

public class Trampoline : MonoBehaviour
{
    [Header("Fuerza del trampolín")]
    public float bounceForce = 15f;
    
    [Header("Sonido de salto")]
    public AudioClip jumpSound;

    private AudioSource audioSource;

    private void Start()
    {
        // Asegúrate de que haya un componente AudioSource en el mismo GameObject
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            // Si no lo encuentra, añade uno automáticamente para evitar errores
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Rigidbody2D rb = other.rigidbody;
            if (rb != null)
            {
                // Reproducir el sonido
                if (jumpSound != null)
                {
                    audioSource.PlayOneShot(jumpSound);
                }

                // Resetear la velocidad vertical para un salto más consistente
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);

                // Aplicar impulso hacia arriba con la fuerza definida
                rb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
            }
        }
    }
}