using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ShipMovement : MonoBehaviour
{

private Transform playerModel;

    [Header("Settings")]
    public bool joystick = true;

    [Space]

    [Header("Parameters")]
    public float xySpeed = 18;
    public float xyAcceleration = 1;
    public float forwardSpeed = 6;
    public float roll = 20;
    public float pitch = 20;
    public float yaw = 20;
    public float rotationLerp = 0.15f;
    public float rollLerp = 0.09f;
    public Vector3 currentVelocity = new Vector3(0f, 0f, 0f);
    public Vector3 movementDirection;
    public bool stopInput;

    [Space]

    [Header("Public References")]
    public Transform aimTarget;
    public CinemachineDollyCart dolly;
    public Transform cameraParent;

    void Start()
    {
        playerModel = transform.GetChild(0);
        SetSpeed(forwardSpeed);
    }

    private void FixedUpdate()
    {
        // Getting input
        float h = joystick ? InputManager.GetAxis(PlayerInput.PlayerAxis.MoveHorizontal) : InputManager.GetAxis(PlayerInput.PlayerAxis.CameraHorizontal);
        float v = joystick ? InputManager.GetAxis(PlayerInput.PlayerAxis.MoveVertical) : InputManager.GetAxis(PlayerInput.PlayerAxis.CameraHorizontal);

        // Ship decelerates when no button is held in direction when moving
        if (h != Mathf.Sign(currentVelocity.x)) h += -Mathf.Sign(currentVelocity.x);
        if (v != Mathf.Sign(currentVelocity.y)) v += -Mathf.Sign(currentVelocity.y);
        // Turning ship input into acceleration for ship
        h *= xyAcceleration; v *= xyAcceleration;
        h = Mathf.Clamp(currentVelocity.x + h, -xySpeed, xySpeed);
        v = Mathf.Clamp(currentVelocity.y + v, -xySpeed, xySpeed);
        // Resetting when close to zero
        if (Mathf.Abs(h) < xyAcceleration) h = 0;
        if (Mathf.Abs(v) < xyAcceleration) v = 0;
        currentVelocity = new Vector3(h, v, 0f);

        // Setting player movement
        float moveH = currentVelocity.x; if (Mathf.Abs(moveH) <= xyAcceleration) moveH = 0;
        float moveV = currentVelocity.y; if (Mathf.Abs(moveV) <= xyAcceleration) moveV = 0;
        movementDirection = new Vector3(moveH, moveV, 0) * Time.deltaTime;
        if (!stopInput)
        {
            transform.localPosition += movementDirection;
            RotationLook(playerModel, h, v);
        }
        ClampPosition();
    }

    // Keeps the ship from moving too far away from the cart/rails
    void ClampPosition()
    {
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        pos.x = Mathf.Clamp01(pos.x);
        pos.y = Mathf.Clamp01(pos.y);
        transform.position = Camera.main.ViewportToWorldPoint(pos);
        float rnd = 10000f;
        float x = Mathf.Round(transform.localPosition.x * rnd) / rnd;
        float y = Mathf.Round(transform.localPosition.y * rnd) / rnd;
        transform.localPosition = new Vector3(x, y, 0);
    }
    // Angles the ship towards a target location as it moves along the 2d gameplay plane
    void RotationLook(Transform target, float h, float v)
    {
        Vector3 targetEulerAngels = target.localEulerAngles;
        float scale = 10f;
        float x = Mathf.Round(Mathf.LerpAngle(targetEulerAngels.x, -v * pitch, rotationLerp) * scale) / scale;
        float y = Mathf.Round(Mathf.LerpAngle(targetEulerAngels.y, h * yaw, rotationLerp) * scale) / scale;
        float z = Mathf.Round(Mathf.LerpAngle(targetEulerAngels.z, -h * roll, rollLerp) * scale) / scale;
        float minAngle = 0.8f;
        x = (x < minAngle && x > -minAngle) ? 0 : x;
        y = (y < minAngle && y > -minAngle) ? 0 : y;
        z = (z < minAngle && z > -minAngle) ? 0 : z;
        target.localEulerAngles = new Vector3(x, y, z);
        //target.localEulerAngles = new Vector3(Mathf.LerpAngle(targetEulerAngels.x, -v * pitch, rotationLerp), Mathf.LerpAngle(targetEulerAngels.y, h * yaw, rotationLerp), Mathf.LerpAngle(targetEulerAngels.z, -h * roll, rollLerp));
    }

    void SetSpeed(float x)
    {
        dolly.m_Speed = x;
    }

    void FieldOfView(float fov)
    {
        cameraParent.GetComponentInChildren<CinemachineVirtualCamera>().m_Lens.FieldOfView = fov;
    }

}

