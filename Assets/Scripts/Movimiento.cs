using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movimiento : MonoBehaviour
{
    private Transform _transform;
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidbody2D;
    
    public float speed = 1;
    public float jumpForce = 5;
    // Start is called before the first frame update
    void Start()
    {
        _transform = gameObject.GetComponent<Transform>();
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            Debug.Log("Movimiento izquierda");
            Vector3 pos = _transform.position;
            pos.x -= speed * Time.deltaTime;
            _transform.position = pos;
            _spriteRenderer.flipX = true;
        }
        
        if (Input.GetKey(KeyCode.D))
        {
            Debug.Log("Movimiento Derecha");
            Vector3 pos = _transform.position;
            pos.x += speed * Time.deltaTime;
            _transform.position = pos;
            _spriteRenderer.flipX = false;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _rigidbody2D.linearVelocity = new Vector2(_rigidbody2D.linearVelocity.x, jumpForce);
        }
    }
}   
