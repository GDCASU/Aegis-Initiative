using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Space]

    [Header("Camera Position Offset")]
    // default position the camera will be compated to target
    public Vector3 offset = Vector3.zero;

    [Space]

    [Header("Camera Offset Limit")]
    // How far ship can drag screen from camera center when moving along edges
    public Vector2 limits = new Vector2(0, 0);

    [Space]

    [Header("Smooth Damp Time")]
    [Range(0, 1)]
    public float smoothTimeX = 0.65f;
    [Range(0, 1)]
    public float smoothTimeYpos = 0.3445f;
    [Range(0, 1)]
    public float smoothTimeYneg = 0.1625f;

    private Vector3 velocity = Vector3.zero;
    private Vector3 lastPosition = Vector3.zero;
    private Vector3 currentPosition = Vector3.zero;

    void Update()
    {
        if (!Application.isPlaying)
        {
            transform.localPosition = offset;
        }

        FollowTarget(target);
    }

    void LateUpdate()
    {
        Vector3 localPos = transform.localPosition;
        lastPosition = target.localPosition;
        transform.localPosition = new Vector3(Mathf.Clamp(localPos.x, -limits.x, limits.x), Mathf.Clamp(localPos.y, -limits.y, limits.y), localPos.z);
    }

    public void FollowTarget(Transform t)
    {
        Vector3 localPos = transform.localPosition;
        Vector3 targetLocalPos = t.transform.localPosition;
        if(targetLocalPos.y >= lastPosition.y + 0.0001f) //clamp speed for positive Y
        {
            currentPosition = Vector3.SmoothDamp(localPos, new Vector3(targetLocalPos.x + offset.x, targetLocalPos.y + offset.y, localPos.z), ref velocity, smoothTimeYpos);
        }
        else if(targetLocalPos.y <= lastPosition.y - 0.0001f) //clamp speed for negative Y
        {
            currentPosition = Vector3.SmoothDamp(localPos, new Vector3(targetLocalPos.x + offset.x, targetLocalPos.y + offset.y, localPos.z), ref velocity, smoothTimeYneg);
        }
        else //clamp speed for X axis
        {
            currentPosition = Vector3.SmoothDamp(localPos, new Vector3(targetLocalPos.x + offset.x, targetLocalPos.y + offset.y, localPos.z), ref velocity, smoothTimeX);
        }
        transform.localPosition = currentPosition;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(-limits.x, -limits.y, transform.position.z), new Vector3(limits.x, -limits.y, transform.position.z));
        Gizmos.DrawLine(new Vector3(-limits.x, limits.y, transform.position.z), new Vector3(limits.x, limits.y, transform.position.z));
        Gizmos.DrawLine(new Vector3(-limits.x, -limits.y, transform.position.z), new Vector3(-limits.x, limits.y, transform.position.z));
        Gizmos.DrawLine(new Vector3(limits.x, -limits.y, transform.position.z), new Vector3(limits.x, limits.y, transform.position.z));
    }
}
