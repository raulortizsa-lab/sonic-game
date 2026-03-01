using System;
using UnityEngine;
using System.Collections;
using Unity.Burst.CompilerServices;
using Managers;
using Random = UnityEngine.Random;

public class player : MonoBehaviour
{
    [Header("Componentes")]
    public SoundLibrary Sfxlibrary;
    public Transform _transform;
    public SpriteRenderer _spriteRenderer;
    public Rigidbody2D _rigidbody2D;
    public Animator animator_ref;
    public AudioSource jump_sound;
    public CoinText CoinTextref;
    public BoxCollider2D boxCollider2D;

    
    [Header("Movimiento")]
    public float speed = 1;
    public float jumpForce = 5;

    [Header("Daño")]
    public bool isGrounded;
    private bool isDamaged = false;

    [Header("Anillos")]
    public GameObject ringPrefab;
    public float ringForce = 5f;

    [Header("Spindash")]
    public bool isSpindash = false;
    public int spindashCharge = 0;
    public int spindashMaxCharge = 20;
    
    void Update()
    {
        if (isDamaged) return;

        isGrounded = Mathf.Abs(_rigidbody2D.linearVelocity.y) < 0.01f;
        animator_ref.SetBool("isJumping", !isGrounded);

        if (isGrounded)
            animator_ref.SetBool("isTrampolin", false);

        // -----------------------
        // AGACHARSE
        // -----------------------
        if (Input.GetKey(KeyCode.S))
        {
            animator_ref.SetBool("isDuck", true);

            if (isGrounded)
            {
                boxCollider2D.offset = new Vector2(boxCollider2D.offset.x, -0.06940699f);
                boxCollider2D.size = new Vector2(boxCollider2D.size.x, 0.25f);
            }
            if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                isSpindash = true;
                if (spindashCharge < spindashMaxCharge)
                {
                    spindashCharge += 3;
                }
                Debug.Log(spindashCharge);
            }
        }
        else
        {
            _rigidbody2D.AddForce(new Vector2(spindashCharge, 0f), ForceMode2D.Impulse);
            animator_ref.SetBool("isDuck", false);
            boxCollider2D.offset = new Vector2(boxCollider2D.offset.x, 0f);
            boxCollider2D.size = new Vector2(boxCollider2D.size.x, 0.41f);
            
            isSpindash = false;
            spindashCharge = 0;
            Debug.Log(spindashCharge);
        }

        // -----------------------
        // MOVIMIENTO (solo si NO está agachado)
        // -----------------------
        if (!animator_ref.GetBool("isDuck"))
        {
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
        }
        else
        {
            animator_ref.SetBool("isWalking", false);
        }

        // -----------------------
        // SALTO (no puede saltar agachado)
        // -----------------------
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !animator_ref.GetBool("isDuck"))
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
        
        float direction = _spriteRenderer.flipX ? 1f : -1f;
        _rigidbody2D.linearVelocity = Vector2.zero;
        _rigidbody2D.AddForce(new Vector2(direction * 8f, 5f), ForceMode2D.Impulse);
        
        yield return new WaitForSeconds(0.4f);
        // Sonic ring burst
        DropRings();
        AudioManager.Instance.Play(Sfxlibrary,ConstantManager.Sfx.Elements.DropRing);

        

        yield return new WaitForSeconds(1.5f);
        isDamaged = false;
    }

    // --------------------------------------------------------
    //     RING BURST - SONIC STYLE (completo y funcional)
    // --------------------------------------------------------
    private void DropRings()
    {
        int ringCount = GameManager.Instance.rings;

        float totalAngle = 160f;
        float centerAngle = 90f;
        float startAngle = centerAngle - (totalAngle / 2f);
        
        float angleStep = ringCount > 1 ? totalAngle / (ringCount - 1) : 0f;
        GameManager.Instance.rings = 0;
        GameManager.Instance.TMP_rings.text = "0";
        
        float spawnRadius = 0.8f;
        
        
        for (int i = 0; i < ringCount; i++)
        {
            float angle = (startAngle + i * angleStep) * Mathf.Deg2Rad;

            Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

            Vector3 spawnPos = _transform.position + (Vector3)(direction * spawnRadius);

            GameObject ring = Instantiate(ringPrefab, spawnPos, Quaternion.identity);

            Rigidbody2D rb = ring.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;

                float randomForce = Random.Range(ringForce * 0.9f, ringForce * 1.1f);
                rb.AddForce(direction.normalized * randomForce, ForceMode2D.Impulse);

                rb.AddTorque(Random.Range(-120f, 120f));
            }
        }

    }
    
    

}
