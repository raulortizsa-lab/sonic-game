using UnityEngine;
using System.Collections;

public class plataform : MonoBehaviour
{
    public float speed = 2f;
    public Transform pointA;
    public Transform pointB;

    public float delay = 3f;
    private Transform crabTrf;
    private bool movingToB = true;   // Si va hacia B o hacia A
    private bool isWaiting = false;  // Para evitar múltiples corutinas
    
    void Start()
    {
        crabTrf = transform;
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

        yield return new WaitForSeconds(delay); // espera 3 segundos
        movingToB = !movingToB;             // cambia de dirección
        isWaiting = false;
    }
}
