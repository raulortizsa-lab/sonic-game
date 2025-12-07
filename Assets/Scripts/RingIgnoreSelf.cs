using UnityEngine;
using System.Collections;

public class RingIgnoreSelf : MonoBehaviour
{
    IEnumerator Start()
    {
        Collider2D col = GetComponent<Collider2D>();

        col.enabled = false;     // evita la “pegazón”
        yield return new WaitForSeconds(0.15f);
        col.enabled = true;      // ahora sí que colisionen normalmente
    }
}