using UnityEngine;

public class RingBounce : MonoBehaviour
{
    private Rigidbody2D rb;
    private int bounceCount = 0;
    public int maxBounces = 3;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        bounceCount++;

        if (bounceCount > maxBounces)
        {
            rb.linearVelocity *= 0.4f;
            rb.angularVelocity = 0f;
        }
    }
}