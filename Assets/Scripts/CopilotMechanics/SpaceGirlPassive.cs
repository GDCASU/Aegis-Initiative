using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceGirlPassive : CopilotPassiveMechanic
{
    
    public float healthPercent = 25.0f;
    public int increasedDamage;
    public GameObject bullets;

    private int normalDamage;

    private void Start()
    {
        normalDamage = bullets.GetComponent<Bullet>().damage;
    }

    private void Update()
    {

        if (PlayerHealth.singleton.health / PlayerHealth.singleton.maxHealth <= healthPercent / 100)
            bullets.GetComponent<Bullet>().damage = increasedDamage;
            else
            bullets.GetComponent<Bullet>().damage = normalDamage;
    }

    public override void CopyInfo(CopilotMechanic copilotMechanic)
    {
        base.CopyInfo(copilotMechanic);
    }
}
