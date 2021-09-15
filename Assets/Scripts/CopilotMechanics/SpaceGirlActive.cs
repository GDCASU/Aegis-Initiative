using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceGirlActive : CopilotActiveMechanic
{
    [Range (0,1)]
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
        if (!activeUsed && InputManager.GetButtonDown(PlayerInput.PlayerButton.ActiveAbility))
        {
            if (PlayerInfo.singleton.health <= healthThreshold)//Checks if player is at or below health threshold 
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
