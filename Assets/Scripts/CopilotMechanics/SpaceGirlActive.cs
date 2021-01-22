using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceGirlActive : CopilotActiveMechanic
{
    public float healthPercent = 25.0f;
    public bool activeUsed;
 
    private void Start()
    {
        activeUsed = false;
    }
    
    private void Update()
    {
        if (PlayerHealth.singleton.health / PlayerHealth.singleton.maxHealth <= healthPercent / 100)
        {
            if (activeUsed == false && InputManager.GetButtonDown(PlayerInput.PlayerButton.ActiveAbility))
            {
                PlayerHealth.singleton.Heal(PlayerHealth.singleton.maxHealth);
                activeUsed = true;
            }
        }
    }

    public override void CopyInfo(CopilotMechanic copilotMechanic)
    {
        base.CopyInfo(copilotMechanic);
        healthPercent = ((SpaceGirlActive)copilotMechanic).healthPercent;
        activeUsed = ((SpaceGirlActive)copilotMechanic).activeUsed;
    }
}
