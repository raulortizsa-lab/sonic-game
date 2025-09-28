using UnityEngine;

public class player : MonoBehaviour
{
    // Componentes del personaje
    public Transform _trasnform;
    public SpriteRenderer _spriteRedenderer;
    public Rigidbody2D _ridgidbody2D;

    // Referencias a efectos de sonido
    public AudioSource jump_sound;

    // Variables para el flujo
    public float speed = 1;
    public float jumpForce = 5;

    // Variable para animación
    public Animator animator_ref;

    // Variable para detectar si el personaje está en el suelo
    public bool isGrounded;

    // Update is called once per frame
    void Update()
    {
        // Comprueba si el personaje está en el suelo (esto se puede mejorar con un sistema de colisión)
        isGrounded = Mathf.Abs(_ridgidbody2D.linearVelocity.y) < 0.01f;
        animator_ref.SetBool("isJumping", !isGrounded);

        // Movimiento hacia la izquierda
        if (Input.GetKey(KeyCode.A))
        {
            animator_ref.SetBool("isWalking", true);
            Vector3 pos = _trasnform.position;
            pos.x -= speed * Time.deltaTime;
            _trasnform.position = pos;
            _spriteRedenderer.flipX = true;
        }
        // Movimiento hacia la derecha
        else if (Input.GetKey(KeyCode.D))
        {
            animator_ref.SetBool("isWalking", true);
            Vector3 pos = _trasnform.position;
            pos.x += speed * Time.deltaTime;
            _trasnform.position = pos;
            _spriteRedenderer.flipX = false;
        }
        else
        {
            animator_ref.SetBool("isWalking", false);
        }

        // Salto con la tecla de espacio
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            jump_sound.Play();
            _ridgidbody2D.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }
    }
}
