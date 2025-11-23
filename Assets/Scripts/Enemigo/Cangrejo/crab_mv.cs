using System.Collections;
using UnityEngine;

public class crab_mv : MonoBehaviour
{
    public float speed = 2f;
    public Transform pointA;
    public Transform pointB;

    public GameObject proyectil;
    public Transform position1;
    public Transform position2;

    private Transform crabTrf;
    private bool movingToB = true;   // Si va hacia B o hacia A
    private bool isWaiting = false;  // Para evitar múltiples corutinas
    private Animator _animator;
    
    public Killzone _Killzoneref;
    
    void Start()
    {
        crabTrf = transform;
        _animator = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        if (isWaiting) return; // Si está esperando, no se mueve
        
        Vector3 target = movingToB ? pointB.position : pointA.position;

        // Mover hacia el target
        crabTrf.position = Vector3.MoveTowards(crabTrf.position, target, speed * Time.deltaTime);

        // Si ya llegó
        if (Vector3.Distance(crabTrf.position, target) < 0.01f)
        {
            StartCoroutine(WaitAndSwitch());
        }
    }

    IEnumerator WaitAndSwitch()
    {
        isWaiting = true;
        _animator.SetBool("Waiting", isWaiting);

        yield return new WaitForSeconds(1f); // espera 3 segundos
          GameObject p1 = Instantiate(proyectil, position1.position, Quaternion.identity);
         GameObject p2 =  Instantiate(proyectil, position2.position, Quaternion.identity);

 // Obtener sus rigidbodies
    Rigidbody2D rb1 = p1.GetComponent<Rigidbody2D>();
    Rigidbody2D rb2 = p2.GetComponent<Rigidbody2D>();

    // Aplicar impulso en forma de arco
    if (rb1 != null)
        rb1.AddForce(new Vector2(-2f, 5f), ForceMode2D.Impulse); // derecha + arriba
    if (rb2 != null)
        rb2.AddForce(new Vector2(2f, 5f), ForceMode2D.Impulse); // izquierda + arriba


        yield return new WaitForSeconds(3f); // espera 3 segundos
        movingToB = !movingToB;             // cambia de dirección
        isWaiting = false;
        _animator.SetBool("Waiting", isWaiting);
        _animator.SetBool("Point", movingToB);
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && _Killzoneref.killzoneFirst == false)
        {
            player p = other.gameObject.GetComponent<player>();
            p.Damage();
        }
    }
}