using System.Collections;
using UnityEngine;

public class crab_mv : MonoBehaviour
{
    public float speed = 2f;
    public Transform pointA;
    public Transform pointB;

    private Transform crabTrf;
    private bool movingToB = true;   // Si va hacia B o hacia A
    private bool isWaiting = false;  // Para evitar múltiples corutinas
    private Animator _animator;
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
        yield return new WaitForSeconds(3f); // espera 3 segundos
        movingToB = !movingToB;             // cambia de dirección
        isWaiting = false;
        _animator.SetBool("Waiting", isWaiting);
        _animator.SetBool("Point", movingToB);
    }
}