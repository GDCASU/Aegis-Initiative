using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrederickActive : CopilotActiveMechanic
{
    [Header("Frederick")]
    public Bullet playerBullet;
    public float activeDuration;
    public int activeDamage;    //Damage value when active is active

    private float _timerCount;
    private int _defaultDamage;
    private bool _activeIsActive;   //Small bool for if the active is currently running

    private void Start()
    {
        playerBullet = GetComponent<BasicPlayerShooting>().bulletPrefab.GetComponent<Bullet>();

        _defaultDamage = playerBullet.damage;
    }

    private void Update()
    {
        if (!_activeIsActive && InputManager.GetButtonDown(PlayerInput.PlayerButton.ActiveAbility))
        {
            _activeIsActive = true;
            _timerCount = activeDuration;

            playerBullet.damage = activeDamage;
        }
        else if (_activeIsActive)
        {
            _timerCount -= Time.deltaTime;

            if (_timerCount <= 0)
            {
                _activeIsActive = false;
                playerBullet.damage = _defaultDamage;
            }
        }
    }

    public override void CopyInfo(CopilotMechanic copilotMechanic)
    {
        base.CopyInfo(copilotMechanic);
        activeDuration = ((FrederickActive)copilotMechanic).activeDuration;
        activeDamage = ((FrederickActive)copilotMechanic).activeDamage;
    }
}
