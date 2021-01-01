using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeebeeActive : CopilotActiveMechanic
{
    public ShipMovement shipMovement;
    public float speedOfDodge;
    public float numberofRolls = 1;
    public bool rollPerforming;
    public Vector3 rollDirection;
    public float timeOfRoll;
    private float timer;
    private void Start()
    {
        shipMovement = GetComponent<ShipMovement>();
    }
    private void Update()
    {
        if (InputManager.GetButtonDown(PlayerInput.PlayerButton.ActiveAbility))
        {
            rollDirection = shipMovement.movementDirection*speedOfDodge;
            shipMovement.stopInput = true;
            rollPerforming = true;
            timer = timeOfRoll;
        }
    }
    private void FixedUpdate()
    {
        if (rollPerforming)
        {
            if (timer > 0)
            {
                transform.localPosition += rollDirection;
                transform.localEulerAngles += Vector3.forward * 22.5f * ((rollDirection.x>=0)? -1:1) * numberofRolls;
                timer -= Time.deltaTime;
            }
            else EndRoll();
        }
    }
    public void EndRoll()
    {
        rollPerforming = false;
        shipMovement.stopInput = false;
    }
}
