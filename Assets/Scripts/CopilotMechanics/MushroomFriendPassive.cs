using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomFriendPassive : CopilotPassiveMechanic
{
    public int defenseIncrease; //amount to increase defense
    public float sporeTimer; //lifetime of spores
    public int healthLost; //set threshold amount of 10% health decrease
    public int health; //to detect if health decreased or increased
    private bool defenseUp; //check if spores should be added to increase defense
    public List<float> spores; 

    void Start()
    {
        defenseUp = false;
        health = PlayerInfo.singleton.health;
        healthLost = (int)(PlayerInfo.singleton.health * 0.1f);
        spores = new List<float>();
    }

    void Update()
    {
        //if Player healed, then match base health to current Player health
        if (PlayerInfo.singleton.health > health)
        {
            health = PlayerInfo.singleton.health;
        }

        //update each active spores' lifetime and remove spores that reached its lifetime
        //update Player's defense based on the number of active spores
        if (spores.Count >= 1)
        {
            for (int index = 0; index < spores.Count; index++)
            {
                {
                    spores[index] -= Time.deltaTime;
                }
            }
            spores.RemoveAll(EndTimer);
            PlayerInfo.singleton.defense = spores.Count*defenseIncrease;
        }

        //if Player received 10% or more damage to their health, 
        //add a spore and update base health to Player's current health
        if (defenseUp)
        {
            spores.Add(sporeTimer);
            health = PlayerInfo.singleton.health;
            defenseUp = false;
        }

        if (!defenseUp && PlayerInfo.singleton.health < health && PlayerInfo.singleton.health % healthLost == 0)
        {
            defenseUp = true;
        }
    }

    private bool EndTimer(float time)
    {
        return time <= 0;
    }

    public override void CopyInfo(CopilotMechanic copilotMechanic)
    {
        base.CopyInfo(copilotMechanic);
        defenseIncrease = ((MushroomFriendPassive)copilotMechanic).defenseIncrease;
    }
}
