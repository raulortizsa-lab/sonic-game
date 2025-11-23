using UnityEngine;
using System.Collections;

public class player : MonoBehaviour
{
    [Header("Componentes")]
    public Transform _transform;
    public SpriteRenderer _spriteRenderer;
    public Rigidbody2D _rigidbody2D;
    public Animator animator_ref;
    public AudioSource jump_sound;
    public CoinText CoinTextref;

    [Header("Movimiento")]
    public float speed = 1;
    public float jumpForce = 5;

    [Header("Daño")]
    public bool isGrounded;
    private bool isDamaged = false;

    [Header("Anillos")]
    public GameObject ringPrefab;
    public int ringsToSpawn = 10; // cantidad de anillos que salen volando
    public float ringForce = 5f;  // fuerza con la que se lanzan

    void Update()
    {
        if (isDamaged) return;

        isGrounded = Mathf.Abs(_rigidbody2D.linearVelocity.y) < 0.01f;
        animator_ref.SetBool("isJumping", !isGrounded);

        if (isGrounded)
            animator_ref.SetBool("isTrampolin", false);

        if (Input.GetKey(KeyCode.A))
        {
            animator_ref.SetBool("isWalking", true);
            Vector3 pos = _transform.position;
            pos.x -= speed * Time.deltaTime;
            _transform.position = pos;
            _spriteRenderer.flipX = true;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            animator_ref.SetBool("isWalking", true);
            Vector3 pos = _transform.position;
            pos.x += speed * Time.deltaTime;
            _transform.position = pos;
            _spriteRenderer.flipX = false;
        }
        else
        {
            animator_ref.SetBool("isWalking", false);
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            jump_sound.Play();
            _rigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    public void isJumpingInTrampoline()
    {
        animator_ref.SetBool("isTrampolin", true);
    }

    public void Damage()
    {
        if (!isDamaged)
            StartCoroutine(DamageRoutine());
    }

    private IEnumerator DamageRoutine()
    {
        isDamaged = true;
        animator_ref.Play("damage");

        yield return new WaitForSeconds(0.5f);
        // Lanza anillos al ser golpeado
        //DropRings();

        float direction = _spriteRenderer.flipX ? 1f : -1f;
        _rigidbody2D.linearVelocity = Vector2.zero;
        _rigidbody2D.AddForce(new Vector2(direction * 8f, 5f), ForceMode2D.Impulse);

        yield return new WaitForSeconds(1.5f);

        isDamaged = false;
    }

    private void DropRings()
    {
        if (ringPrefab == null) return;

        for (int i = 0; i < ringsToSpawn; i++)
        {
            GameObject ring = Instantiate(ringPrefab, _transform.position, Quaternion.identity);
            ring.GetComponent<Coin>().CoinTextref = CoinTextref;

            Rigidbody2D rb = ring.GetComponent<Rigidbody2D>();
            
            if (rb != null)
            {
                // Dirección aleatoria con fuerza
                Vector2 randomDir = new Vector2(Random.Range(-1f, 1f), Random.Range(0.5f, 1f)).normalized;
                rb.AddForce(randomDir * ringForce, ForceMode2D.Impulse);
            }
        }
    }
}
