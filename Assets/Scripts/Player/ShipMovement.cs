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
    public float forwardSpeed = 6;
    public float roll = 20;
    public float pitch = 20;
    public float yaw = 20;
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

    void Update()
    {
        float h = joystick ? InputManager.GetAxis(PlayerInput.PlayerAxis.MoveHorizontal) : InputManager.GetAxis(PlayerInput.PlayerAxis.CameraHorizontal);
        float v = joystick ? InputManager.GetAxis(PlayerInput.PlayerAxis.MoveVertical) : InputManager.GetAxis(PlayerInput.PlayerAxis.CameraHorizontal);
        movementDirection = new Vector3(h, v, 0) * xySpeed * Time.deltaTime;
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
    }
    // Angles the ship towards a target location as it moves along the 2d gameplay plane
    void RotationLook(Transform target, float h, float v)
    {
        Vector3 targetEulerAngels = target.localEulerAngles;
        target.localEulerAngles = new Vector3(
            Mathf.LerpAngle(targetEulerAngels.x, -v * pitch, .1f), Mathf.LerpAngle(targetEulerAngels.y, h * yaw,  .1f), Mathf.LerpAngle(targetEulerAngels.z, -h * roll, .1f)
        );
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

