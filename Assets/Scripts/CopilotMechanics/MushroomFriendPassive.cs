using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomFriendPassive : CopilotPassiveMechanic
{
    public float defenseIncrease; //amount to increase defense
    public float sporeTimer; //lifetime of spores
    public float healthLostPercent; //Percent of max health needed to gain orb
    private int healthLost; //set threshold amount of 10% health decrease
    private int health; //to detect if health decreased or increased
    void Start()
    {
        health = PlayerInfo.singleton.health;
        healthLost = (int)(PlayerInfo.singleton.maxHealth * healthLostPercent);
        PlayerInfo.singleton.damageEvent += AddSpore;
    }
    public void AddSpore()
    {
        if ((health - PlayerInfo.singleton.health) >= healthLost)
        {
            health = PlayerInfo.singleton.health;
            PlayerInfo.singleton.AddStatusEffect(StatusEffects.FORITIFY, defenseIncrease, sporeTimer, true);
        }
    }

    public override void CopyInfo(CopilotMechanic copilotMechanic)
    {
        base.CopyInfo(copilotMechanic);
        defenseIncrease = ((MushroomFriendPassive)copilotMechanic).defenseIncrease;
        sporeTimer = ((MushroomFriendPassive)copilotMechanic).sporeTimer;
    }
    private void OnDestroy()
    {
        if(PlayerInfo.singleton != null)
        {
            PlayerInfo.singleton.damageEvent -= AddSpore;
        }
    }
}
