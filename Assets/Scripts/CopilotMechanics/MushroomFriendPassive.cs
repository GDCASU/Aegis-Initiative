using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomFriendPassive : CopilotPassiveMechanic
{
    //for testing
    public int defenseIncrease; //amount to increase defense
    public float sporeTimer; //lifetime of spores

    //real stuff
    public int healthLost;

    public int health; //to detect if health decreased or increased

    private bool defenseUp;
    public List<float> spores;

    void Start()
    {
        defenseUp = false;
        health = PlayerHealth.singleton.health;
        healthLost = (int)(PlayerHealth.singleton.health * 0.1f);
        spores = new List<float>();
    }

    void Update()
    {
        if (PlayerHealth.singleton.health > health)
        {
            health = PlayerHealth.singleton.health;
        }

        if (spores.Count >= 1)
        {
            for (int index = 0; index < spores.Count; index++)
            {
                {
                    spores[index] -= Time.deltaTime;
                }
            }
            spores.RemoveAll(EndTimer);
            PlayerHealth.singleton.defense = spores.Count*defenseIncrease;
        }

        if (defenseUp)
        {
            spores.Add(sporeTimer);
            health = PlayerHealth.singleton.health;
            defenseUp = false;
        }

        if (!defenseUp && PlayerHealth.singleton.health < health && PlayerHealth.singleton.health % healthLost == 0)
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
