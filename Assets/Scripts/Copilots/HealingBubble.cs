using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingBubble : MonoBehaviour
{
    //variables for spore particle system
    public float sporeTimer;
    private bool sporeStart;
    private ParticleSystem spreParticles;

    //variables for healing bubble
    private MeshRenderer bubbleMesh;
    public Vector3 bubbleScale = new Vector3(0.01f, 0.01f, 0.01f);
    public float bubbleMaxXScale = 0.7f;    //Max x value that the bubble scales too when inflating
    private bool bubbleInflated;

    //variables for healing player
    private bool healPlayer;
    private float healTime;

    // Start is called before the first frame update
    void Start()
    {
        sporeStart = false;
        spreParticles = GetComponent<ParticleSystem>();
        spreParticles.Stop();
        bubbleMesh = transform.GetComponent<MeshRenderer>();
        bubbleInflated = false;
        healPlayer = false;
    }

    // Update is called once per frame
    void Update()
    {
        //start spore particle system
        if (sporeStart)
        {
            sporeTimer = healTime;
            healPlayer = true;

            spreParticles.Simulate(0.0f, true, true); //reset particle system sporeTimer
            spreParticles.Play();
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
            spreParticles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            healPlayer = false;
            sporeTimer = 0;
            bubbleMesh.transform.localScale = bubbleScale;
            bubbleInflated = false;
            bubbleMesh.enabled = false;
        }

        //if healing bubble was activated, scale bubble up
        if (!bubbleInflated && bubbleMesh.enabled)
        {
            if (bubbleMesh.transform.localScale.x >= bubbleMaxXScale)
            {
                sporeStart = true;
                bubbleInflated = true;
            }
            else
            {
                bubbleMesh.transform.localScale += bubbleScale;
            }
        }

    }

    //store time for healing and display the healing bubble
    public void SetSporeTimer(float timer)
    {
        healTime = timer;
        bubbleMesh = transform.GetComponent<MeshRenderer>();
        bubbleMesh.enabled = true;
    }

    public bool GetHealPlayer()
    {
        return healPlayer;
    }
}
