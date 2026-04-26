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
    public bool ViewDirection; // true = izquierda, false = derecha 

    public Transform transformToSpawn;
    
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
    public bool isSpinDashExecuting = false;

    public int spindashCharge = 0;
    public int spindashMaxCharge = 20;

    public float spindashReleaseDuration = 0.35f;
    private Coroutine spindashReleaseRoutine;

    [Header("Collider Sonic - mismas medidas")]
    [SerializeField] private float normalColliderOffsetY = 0f;
    [SerializeField] private float normalColliderSizeY = 0.41f;

    [SerializeField] private float duckColliderOffsetY = -0.06940699f;
    [SerializeField] private float duckColliderSizeY = 0.25f;

    private bool wasDucking = false;
    
    void Update()
    {
        if (isDamaged) return;

        isGrounded = Mathf.Abs(_rigidbody2D.linearVelocity.y) < 0.01f;
        animator_ref.SetBool("isJumping", !isGrounded);

        if (isGrounded)
            animator_ref.SetBool("isTrampolin", false);

        // -----------------------
        // AGACHARSE / SPINDASH
        // -----------------------
        bool isHoldingDown = Input.GetKey(KeyCode.S);

        if (isHoldingDown)
        {
            animator_ref.SetBool("isDuck", true);

            if (isGrounded && !wasDucking)
            {
                SetDuckCollider();
                wasDucking = true;
            }

            // CARGAR SPINDASH
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                isSpindash = true;
                isSpinDashExecuting = false;

                animator_ref.SetBool("isSpinDash", true);      // intro / carga
                animator_ref.SetBool("IsSpinDashExc", false);  // ejecución apagada

                if (spindashCharge < spindashMaxCharge)
                {
                    spindashCharge += 3;
                    spindashCharge = Mathf.Clamp(spindashCharge, 0, spindashMaxCharge);

                    animator_ref.speed = 1 + (spindashCharge / (float)spindashMaxCharge);
                }

                Debug.Log("Cargando spindash: " + spindashCharge);
            }
        }
        else
        {
            animator_ref.SetBool("isDuck", false);

            if (wasDucking)
            {
                SetNormalColliderSafe();
                wasDucking = false;
            }

            // SOLTAR SPINDASH SOLO SI REALMENTE HABÍA UNO CARGADO
            if (isSpindash && spindashCharge > 0)
            {
                ReleaseSpinDash();
            }
        }

        // -----------------------
        // MOVIMIENTO
        // Solo si NO está agachado, NO carga spindash y NO está ejecutando spindash
        // -----------------------
        if (!animator_ref.GetBool("isDuck") && !isSpindash && !isSpinDashExecuting)
        {
            if (Input.GetKey(KeyCode.A))
            {
                animator_ref.SetBool("isWalking", true);

                Vector3 pos = _transform.position;
                pos.x -= speed * Time.deltaTime;
                _transform.position = pos;

                _spriteRenderer.flipX = true;
                ViewDirection = true;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                animator_ref.SetBool("isWalking", true);

                Vector3 pos = _transform.position;
                pos.x += speed * Time.deltaTime;
                _transform.position = pos;

                _spriteRenderer.flipX = false;
                ViewDirection = false;
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
        // SALTO
        // No puede saltar agachado, cargando spindash o ejecutando spindash
        // -----------------------
        if (Input.GetKeyDown(KeyCode.Space) 
            && isGrounded 
            && !animator_ref.GetBool("isDuck") 
            && !isSpindash 
            && !isSpinDashExecuting)
        {
            jump_sound.Play();
            _rigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    // --------------------------------------------------------
    // COLLIDER SONIC
    // Mantiene tus mismas medidas, pero evita que al volver a normal
    // el collider se clave en el piso.
    // --------------------------------------------------------
    private void SetDuckCollider()
    {
        boxCollider2D.offset = new Vector2(boxCollider2D.offset.x, duckColliderOffsetY);
        boxCollider2D.size = new Vector2(boxCollider2D.size.x, duckColliderSizeY);
    }

    private void SetNormalColliderSafe()
    {
        float oldBottom = boxCollider2D.offset.y - (boxCollider2D.size.y * 0.5f);

        boxCollider2D.offset = new Vector2(boxCollider2D.offset.x, normalColliderOffsetY);
        boxCollider2D.size = new Vector2(boxCollider2D.size.x, normalColliderSizeY);

        float newBottom = boxCollider2D.offset.y - (boxCollider2D.size.y * 0.5f);

        float difference = oldBottom - newBottom;

        if (difference > 0f)
        {
            _transform.position += new Vector3(0f, difference, 0f);
        }

        Debug.Log("Collider normal seguro. Compensación Y: " + difference);
    }

    // --------------------------------------------------------
    // SPINDASH
    // --------------------------------------------------------
    private void ReleaseSpinDash()
    {
        isSpindash = false;
        isSpinDashExecuting = true;

        animator_ref.SetBool("isSpinDash", false);      // apaga intro / carga
        animator_ref.SetBool("IsSpinDashExc", true);    // prende ejecución
        animator_ref.speed = 1f;

        float direction = ViewDirection ? -1f : 1f;

        _rigidbody2D.linearVelocity = new Vector2(0f, _rigidbody2D.linearVelocity.y);
        _rigidbody2D.AddForce(new Vector2(direction * spindashCharge, 0f), ForceMode2D.Impulse);

        Debug.Log("Soltando spindash con fuerza: " + spindashCharge);

        spindashCharge = 0;

        if (spindashReleaseRoutine != null)
            StopCoroutine(spindashReleaseRoutine);

        spindashReleaseRoutine = StartCoroutine(EndSpinDashExecution());
    }
    
    private IEnumerator EndSpinDashExecution()
    {
        yield return new WaitForSeconds(spindashReleaseDuration);

        isSpinDashExecuting = false;
        animator_ref.SetBool("IsSpinDashExc", false);

        Debug.Log("Terminó ejecución de spindash");
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

        DropRings();
        AudioManager.Instance.Play(Sfxlibrary, ConstantManager.Sfx.Elements.DropRing);

        yield return new WaitForSeconds(1.5f);

        isDamaged = false;
    }

    // --------------------------------------------------------
    // RING BURST - SONIC STYLE
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
    
    [ContextMenu("Debug/Simular muerte")]
    public void dead()
    {
       this.transform.position = transformToSpawn.position;
    }
}