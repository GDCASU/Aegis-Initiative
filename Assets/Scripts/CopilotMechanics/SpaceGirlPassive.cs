using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceGirlPassive : CopilotPassiveMechanic
{

    public float healthPercent = 0.25f;//Percent Health threshold
    public int damageIncrease;//Amount of damage added to base damage
    public float passiveTimer;//Passive duration in seconds

    private float healthThreshold;

    private void Start()
    {
        healthThreshold = healthPercent * PlayerInfo.singleton.maxHealth;
        PlayerInfo.singleton.damageEvent += CheckPassiveActivation;
        PlayerInfo.singleton.damageEvent += CheckPassiveDeactivation;
    }

    public void CheckPassiveActivation()
    {
        if ((float)PlayerInfo.singleton.health <= healthThreshold)
        {
            PlayerInfo.singleton.AffectPlayer(StatusEffects.FOCUS, damageIncrease, false);
        }
    }
    public void CheckPassiveDeactivation()
    {
        if ((float)PlayerInfo.singleton.health > healthThreshold)
        {
            PlayerInfo.singleton.AffectPlayer(StatusEffects.FOCUS, damageIncrease, true);
        }
    }

    public override void CopyInfo(CopilotMechanic copilotMechanic)
    {
        base.CopyInfo(copilotMechanic);
        healthPercent = ((SpaceGirlPassive)copilotMechanic).healthPercent;
        passiveTimer = ((SpaceGirlPassive)copilotMechanic).passiveTimer;
        damageIncrease = ((SpaceGirlPassive)copilotMechanic).damageIncrease;
    }
    public void OnDestroy()
    {
        PlayerInfo.singleton.damageEvent -= CheckPassiveActivation;
        PlayerInfo.singleton.damageEvent -= CheckPassiveDeactivation;
    }
}
