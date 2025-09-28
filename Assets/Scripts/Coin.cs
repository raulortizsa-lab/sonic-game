using UnityEngine;

public class Coin : MonoBehaviour
{
    // Creacion de variable
    public CoinText CoinTextref;
    private AudioSource CoinSound;

    // Detecta cuando objeto A entra dentro del objeto B.
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CoinTextref.sumarContador();
            CoinSound = gameObject.GetComponent<AudioSource>();
            CoinSound.Play();
            // Recoger moneda
            Destroy(gameObject);
        }
    }
}
