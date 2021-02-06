using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceGirlPassive : CopilotPassiveMechanic
{

    public float healthPercent = 25f;
    public int increasedDamage;

    private int normalDamage;

    private void Start()
    {
        normalDamage = PlayerInfo.singleton.bulletDamage;
    }

    private void Update()
    {

        if (((float)PlayerInfo.singleton.health / (float)PlayerInfo.singleton.maxHealth) <= (healthPercent / 100f))//Checks if player is at or below health threshold
        {
            PlayerInfo.singleton.bulletDamage = increasedDamage;
        }
        else
        {
            PlayerInfo.singleton.bulletDamage = normalDamage;
        }

    }

    public override void CopyInfo(CopilotMechanic copilotMechanic)
    {
        base.CopyInfo(copilotMechanic);
        healthPercent = ((SpaceGirlPassive)copilotMechanic).healthPercent;
        increasedDamage = ((SpaceGirlPassive)copilotMechanic).increasedDamage;
    }
}
