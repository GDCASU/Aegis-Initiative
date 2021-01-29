using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomFriendActive : CopilotActiveMechanic
{
    public GameObject HealingBubblePrefab;
    private HealingBubble healingBubbleScript;
    public int heal; //amount to heal player
    public float healTime; //time to restore health
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
                if (GameObject.Find("HealingBubble(Clone)") == null)
                {
                    Instantiate(HealingBubblePrefab, PlayerInfo.singleton.transform);
                    healingBubbleScript = GameObject.Find("HealingBubble(Clone)").GetComponent<HealingBubble>();
                    healingBubbleScript.SetSporeTimer(healTime);
                }
                else
                {
                    healingBubbleScript.SetSporeTimer(healTime);
                }
            }
            if (healingBubbleScript != null && !healingBubbleScript.GetHealPlayer())
            {
                healPlayer = false;
            }
        }

        //if Player health is 25% or lower, allow Player to heal
        if (!healPlayer && PlayerInfo.singleton.health <= PlayerInfo.singleton.maxHealth * 0.25f)
        {
            healPlayer = true;
        }
    }

    public override void CopyInfo(CopilotMechanic copilotMechanic)
    {
        base.CopyInfo(copilotMechanic);
        heal = ((MushroomFriendActive)copilotMechanic).heal;
        healTime = ((MushroomFriendActive)copilotMechanic).healTime;
    }
}
