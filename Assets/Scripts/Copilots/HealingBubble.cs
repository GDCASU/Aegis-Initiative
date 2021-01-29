using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingBubble : MonoBehaviour
{
    //variables for spore particle system
    public float sporeTimer;
    private bool sporeStart;
    private ParticleSystem spores;

    //variables for healing bubble
    private MeshRenderer bubbleMesh;
    private Vector3 bubbleScale;
    private bool bubbleInflated;

    private bool healPlayer;
    private float healTime;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0.0f, 0.0f, 0.0f);
        sporeStart = false;
        spores = GetComponent<ParticleSystem>();
        spores.Stop();
        bubbleMesh = transform.GetComponent<MeshRenderer>();
        //bubbleMesh.enabled = false;
        bubbleInflated = false;
        bubbleScale = new Vector3(0.01f, 0.01f, 0.01f);
        healPlayer = false;
    }

    // Update is called once per frame
    void Update()
    {

            //start spore particle system
            if (sporeStart)
            {
            sporeTimer = healTime;
            //sporeTimer = transform.GetComponentInParent<SelectedCopilots>().active.GetComponent<MushroomFriendActive>().healTime;
            healPlayer = true;

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
                //transform.GetComponentInParent<SelectedCopilots>().active.GetComponent<MushroomFriendActive>().healPlayer = false;

                sporeTimer = 0;

                bubbleMesh.transform.localScale = bubbleScale;
                bubbleInflated = false;
                bubbleMesh.enabled = false;
            }

            //if healing bubble was activated, scale bubble up
            if (!bubbleInflated && bubbleMesh.enabled)
            {
            print("inflate bubble");
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
        
    }

    //store time for healing and display the healing bubble
    public void SetSporeTimer(float timer)
    {
        healTime = timer;
        transform.position = new Vector3(0, 0, 0);
        bubbleMesh = transform.GetComponent<MeshRenderer>();
        bubbleMesh.enabled = true;
        print("Bubble mesh: " + bubbleMesh.enabled);
    }

    public bool GetHealPlayer()
    {
        return healPlayer;
    }
}
