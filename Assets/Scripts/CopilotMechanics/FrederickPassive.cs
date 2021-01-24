using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrederickPassive : CopilotPassiveMechanic
{
    [Header("Frederick")]
    public BasicPlayerShooting playerShooting;
    public float passiveDuration;
    public float passiveCooldown;
    public float passiveFirerate;
    public float passiveSpeed;  //Speed of bullet

    private float _defaultFirerate;
    private float _defaultSpeed;
    private float _passiveTimer;
    private float _cooldownTimer;

    private void Start()
    {
        playerShooting = GetComponent<BasicPlayerShooting>();

        _defaultFirerate = playerShooting.fireRate;
        _defaultSpeed = playerShooting.speed;
    }

    private void Update()
    {
        //This starts the cooldown timer
        if (InputManager.GetButtonUp(PlayerInput.PlayerButton.Shoot))
        {
            _cooldownTimer = passiveCooldown;

            SetPlayerShoot(_defaultFirerate, _defaultSpeed);
        }
        //Starts the passive timer
        else if (_cooldownTimer <= 0 && InputManager.GetButtonDown(PlayerInput.PlayerButton.Shoot))
        {
            _passiveTimer = passiveDuration;

            SetPlayerShoot(passiveFirerate, passiveSpeed);
        }

        if (_cooldownTimer > 0) _cooldownTimer -= Time.deltaTime;

        if (_passiveTimer > 0) _passiveTimer -= Time.deltaTime;
        else SetPlayerShoot(_defaultFirerate, _defaultSpeed);
    }

    /// <summary>
    /// Small method to adjust the fire rate and speed
    /// </summary>
    private void SetPlayerShoot(float firerate, float speed)
    {
        playerShooting.fireRate = firerate;
        playerShooting.speed = speed;
    }

    public override void CopyInfo(CopilotMechanic copilotMechanic)
    {
        base.CopyInfo(copilotMechanic);
        passiveDuration = ((FrederickPassive)copilotMechanic).passiveDuration;
        passiveCooldown = ((FrederickPassive)copilotMechanic).passiveCooldown;
        passiveFirerate = ((FrederickPassive)copilotMechanic).passiveFirerate;
        passiveSpeed = ((FrederickPassive)copilotMechanic).passiveSpeed;
    }
}
