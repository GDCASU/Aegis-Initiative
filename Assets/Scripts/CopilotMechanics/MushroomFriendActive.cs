using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomFriendActive : CopilotActiveMechanic
{
    public GameObject HealingBubblePrefab;
    private HealingBubble healingBubbleScript;
    private GameObject healingBubble;   //Spawned bubble
    public int heal; //amount to heal player
    public float healTime; //time to restore health
    public float minHealthPercentage = 0.25f;   //Percernt of max health needed to activate bubble
    public bool healPlayer; //check that Player is allowed to heal


    void Start()
    {
        healPlayer = false;
    }

    void Update()
    {
        //if Player can heal, activate Player active ability and show healing bubble
        if (healPlayer)
        {
            if (InputManager.GetButtonDown(PlayerInput.PlayerButton.ActiveAbility))
            {
                if (healingBubble == null)
                {
                    healingBubble = Instantiate(HealingBubblePrefab, PlayerInfo.singleton.transform);
                    healingBubbleScript = healingBubble.GetComponent<HealingBubble>();
                    healingBubbleScript.SetSporeTimer(healTime);
                }
                else
                {
                    healingBubbleScript.SetSporeTimer(healTime);
                }
            }
            if (healingBubbleScript != null)
            {
                healPlayer = healingBubbleScript.GetHealPlayer();
            }
        }

        //if Player health is 25% or lower, allow Player to heal
        if (!healPlayer && PlayerInfo.singleton.health <= PlayerInfo.singleton.maxHealth * minHealthPercentage)
        {
            healPlayer = true;
        }
    }

    public override void CopyInfo(CopilotMechanic copilotMechanic)
    {
        base.CopyInfo(copilotMechanic);
        heal = ((MushroomFriendActive)copilotMechanic).heal;
        healTime = ((MushroomFriendActive)copilotMechanic).healTime;
        HealingBubblePrefab = ((MushroomFriendActive)copilotMechanic).HealingBubblePrefab;
    }
}
