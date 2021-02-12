using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceGirlActive : CopilotActiveMechanic
{
    public float healthPercent = 0.25f;
    public bool activeUsed;

    private float healthThreshold;

    private void Start()
    {
        activeUsed = false;
        healthThreshold = healthPercent * PlayerInfo.singleton.maxHealth;
    }
    
    private void Update()
    {
        if ((float)PlayerInfo.singleton.health <= healthThreshold)//Checks if player is at or below health threshold
        {
            if (!activeUsed && InputManager.GetButtonDown(PlayerInput.PlayerButton.ActiveAbility))
            {
                //Heals player to full and deactivates active ability
                PlayerInfo.singleton.Heal(PlayerInfo.singleton.maxHealth);
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
