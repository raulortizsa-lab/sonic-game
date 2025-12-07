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
    public int ringsToSpawn = 10;
    public float ringForce = 5f;

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

        // Sonic ring burst
        DropRings();

        float direction = _spriteRenderer.flipX ? 1f : -1f;
        _rigidbody2D.linearVelocity = Vector2.zero;
        _rigidbody2D.AddForce(new Vector2(direction * 8f, 5f), ForceMode2D.Impulse);

        yield return new WaitForSeconds(1.5f);
        isDamaged = false;
    }

    // --------------------------------------------------------
    //     RING BURST - SONIC STYLE (completo y funcional)
    // --------------------------------------------------------
    private void DropRings()
    {
        if (ringPrefab == null) return;

        float angleStep = 360f / ringsToSpawn;
        float spawnRadius = 1.0f; // separación real entre anillos

        for (int i = 0; i < ringsToSpawn; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

            // Instancia los anillos separados desde el inicio
            Vector3 spawnPos = _transform.position + (Vector3)(direction * spawnRadius);

            GameObject ring = Instantiate(ringPrefab, spawnPos, Quaternion.identity);
            ring.GetComponent<Coin>().CoinTextref = CoinTextref;

            Rigidbody2D rb = ring.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // Fuerza inicial fuerte para separarlos
                rb.AddForce(direction.normalized * (ringForce * 1.5f), ForceMode2D.Impulse);

                // Torque para que giren
                rb.AddTorque(Random.Range(-100f, 100f));
            }
        }
    }
}
