using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomFriendActive : CopilotActiveMechanic
{
    //for testing
    public float health; //to visualize when testing

    //real stuff
    public int heal; //amount to heal player
    public float healTime; //time to restore health

    public float sporeTimer;
    public bool sporeStart;
    private bool healPlayer;
    private ParticleSystem spores;
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
        //if (healPlayer)
        //{
        //    //if (InputManager.GetButtonDown(PlayerInput.PlayerButton.ActiveAbility))
        //    if (Input.GetKeyDown(KeyCode.Space))
        //    {
        //        sporeTimer = healTime;
        //        spores.Simulate(0.0f, true, true); //reset particle system sporeTimer
        //        spores.Play();
        //    }
        //}

        if (healPlayer)
        {
            //if (InputManager.GetButtonDown(PlayerInput.PlayerButton.ActiveAbility))
            if (Input.GetKeyDown(KeyCode.Space))
            {
                bubbleMesh.enabled = true;
            }
        }

        if (sporeStart)
        {
            sporeTimer = healTime;
            spores.Simulate(0.0f, true, true); //reset particle system sporeTimer
            spores.Play();

            sporeStart = false;
            print("start spores");
        }

        if (sporeTimer > 0)
        {
            sporeTimer -= Time.deltaTime;
        }

        if (sporeTimer < 0)
        {
            PlayerHealth.singleton.health = PlayerHealth.singleton.maxHealth; 
            spores.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            healPlayer = false;
            sporeTimer = 0;

            bubbleMesh.transform.localScale = bubbleScale;
            bubbleInflated = false;
            bubbleMesh.enabled = false;
        }

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

        if (!healPlayer && PlayerHealth.singleton.health <= PlayerHealth.singleton.maxHealth * 0.25f)
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
