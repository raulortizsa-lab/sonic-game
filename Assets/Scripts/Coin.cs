using UnityEngine;
using System.Collections;
using Managers;
using UnityEngine.Serialization;

public class Coin : MonoBehaviour
{
    public SoundLibrary sfxElements;
    private Collider2D _collider;

    public float enableDelay = 1f; // tiempo antes de que el collider se active

    void Awake()
    {
        _collider = GetComponent<Collider2D>();
        if (_collider != null)
            _collider.enabled = false; // desactivamos al inicio
    }

    void Start()
    {
        StartCoroutine(EnableColliderAfterDelay());
    }

    private IEnumerator EnableColliderAfterDelay()
    {
        yield return new WaitForSeconds(enableDelay);

        if (_collider != null)
            _collider.enabled = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!_collider.enabled) return; // evita que se recoja antes de tiempo

        if (other.CompareTag("Player"))
        {
            GameManager.Instance.addRing();
            AudioManager.Instance.Play(sfxElements, ConstantManager.Sfx.Elements.Ring);
            
            Destroy(gameObject);
        }
    }
}
