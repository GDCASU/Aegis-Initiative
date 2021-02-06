using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceGirlPassive : CopilotPassiveMechanic
{

    public float healthPercent = 25f;//Percent Health threshold
    public int damageIncrease;//Amount of damage added to base damage

    private bool damageNormalized;

    private void Start()
    {
        damageNormalized = true;
    }

    private void Update()
    {

        if (((float)PlayerInfo.singleton.health / (float)PlayerInfo.singleton.maxHealth) <= (healthPercent / 100f))//Checks if player is at or below health threshold
        {
            //Below health threshold
            if (damageNormalized)
            {
                //if damage is normal, adds the set damage increase to PlayerInfo's bulletDamage
                damageNormalized = false;
                PlayerInfo.singleton.bulletDamage += damageIncrease;
            }
        }
        else
        {
            //Above health threshold
            if (!damageNormalized)
            {
                //if damage is not currently normal, subtracts the set damage increase from PlayerInfo's bulletDamage
                damageNormalized = true;
                PlayerInfo.singleton.bulletDamage -= damageIncrease;
            }
        }

    }

    public override void CopyInfo(CopilotMechanic copilotMechanic)
    {
        base.CopyInfo(copilotMechanic);
        healthPercent = ((SpaceGirlPassive)copilotMechanic).healthPercent;
        damageIncrease = ((SpaceGirlPassive)copilotMechanic).damageIncrease;
    }
}
