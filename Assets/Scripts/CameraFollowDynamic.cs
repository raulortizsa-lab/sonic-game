using UnityEngine;

public class CameraFollowDynamic : MonoBehaviour
{
    public Transform target;
    public float baseSmooth = 3f;
    public float maxSmooth = 8f;
    public Vector3 offset;

    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        if (target == null) return;

        float distance = Vector3.Distance(transform.position, target.position);
        float dynamicSmooth = Mathf.Lerp(baseSmooth, maxSmooth, distance / 5f);

        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, 1f / dynamicSmooth);
    }
}
