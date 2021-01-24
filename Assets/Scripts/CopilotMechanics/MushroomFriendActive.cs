using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomFriendActive : CopilotActiveMechanic
{
    //for testing
    public float health; //to visualize when testing

    public int heal; //amount to heal player
    public float healTime; //time to restore health
    private bool healPlayer; //check that Player is allowed to heal

    //variables for spore particle system
    public float sporeTimer;
    public bool sporeStart;
    private ParticleSystem spores;

    //variables for healing bubble
    private MeshRenderer bubbleMesh;
    private Vector3 bubbleScale;
    private bool bubbleInflated;

    void Start()
    {
        healPlayer = false;
        sporeStart = false;
        spores = GetComponent<ParticleSystem>();
        spores.Stop();
        bubbleMesh = transform.GetChild(0).GetComponent<MeshRenderer>();
        bubbleMesh.enabled = false;
        bubbleInflated = false;
        bubbleScale = new Vector3(0.01f, 0.01f, 0.01f);
    }

    void Update()
    {
        //if Player can heal, activate Player active ability and show healing bubble
        if (healPlayer)
        {
            if (InputManager.GetButtonDown(PlayerInput.PlayerButton.ActiveAbility))
            {
                bubbleMesh.enabled = true;
            }
        }

        //start spore particle system
        if (sporeStart)
        {
            sporeTimer = healTime;
            spores.Simulate(0.0f, true, true); //reset particle system sporeTimer
            spores.Play();
            sporeStart = false;
        }

        if (sporeTimer > 0)
        {
            sporeTimer -= Time.deltaTime;
        }

        //stop spore particle system and reset healing bubble
        if (sporeTimer < 0)
        {
            PlayerInfo.singleton.health = PlayerInfo.singleton.maxHealth; 
            spores.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            healPlayer = false;
            sporeTimer = 0;

            bubbleMesh.transform.localScale = bubbleScale;
            bubbleInflated = false;
            bubbleMesh.enabled = false;
        }

        //if healing bubble was activated, scale bubble up
        if (!bubbleInflated && bubbleMesh.enabled)
        {
            if (bubbleMesh.transform.localScale.x >= 0.7f)
            {
                sporeStart = true;
                bubbleInflated = true;
            }
            else
            {
                bubbleMesh.transform.localScale += bubbleScale;
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
