/*
 * Linearly increases opacity of Chameleon when Player is within a certain distance.
 * Deals damage to Player when they collide with Chameleon.
 * 
 * Author: Cristion Dominguez
 * Date: 10 March 2021
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chameleon : EnemyHealth
{
    [SerializeField]
    [Range(0f, 1f)]
    [Tooltip("The opacity the Chameleon adopts when the Player is outside its max detection distance (0 = fully transparent)")]
    private float defaultOpacity = 0f;
    [SerializeField]
    [Range(0f, 1f)]
    [Tooltip("The highest opacity the Chameleon can adopt.")]
    private float maxOpacity = 1f;
    [SerializeField]
    [Tooltip("The maximum distance the Chameleon can be detected; opacity increases as the Player's distance decreases past this point.")]
    private float maxDetectionDistance = 30f;
    [SerializeField]
    [Tooltip("The minimum distance the Chameleon can be detected; opacity remains at the max value as the Player's distance decreases past this point.")]
    private float minDetectionDistance = 10f;

    private Transform player;
    private float playerDistance;  // Player distance from Chameleon
    private float traverseDistance;  // difference between maxDetectionDistance and minDetectionDistance
    private float progress;  // decimal ratio of opacity to be displayed based on Player distance 
                             // [i.e. for an opacity range from 0 to 1, if the Player traversed 50% of the way between max detection distance and
                             //  min detection distance, then the Chameleon opacity is set to 0.5, which is 50% transparent.]

    private MeshRenderer renderer;
    private Color opacity;
    
    /// <summary>
    /// Assigns values to variables.
    /// </summary>
    private void Start()
    {
        player = PlayerInfo.singleton.transform;
        renderer = transform.GetComponent<MeshRenderer>();

        opacity = renderer.material.color;
        opacity.a = defaultOpacity;
        renderer.material.color = opacity;
    }

    /// <summary>
    /// Sets Chameleon's parent to none so it can stay still.
    /// </summary>
    private void OnEnable() => transform.SetParent(null);

    /// <summary>
    /// Updates Chameleon opacity based on Chameleon's distance from Player.
    /// </summary>
    private void Update()
    {
        playerDistance = Vector3.Distance(player.position, transform.position);
        traverseDistance = maxDetectionDistance - minDetectionDistance;

        // If Player distance is less than the maxDetectionDistance, calculate a new opacity for Chameleon. Otherwise, set the new opacity to the default value.
        if (playerDistance <= maxDetectionDistance)
        {
            progress = 1 - (playerDistance - minDetectionDistance) / traverseDistance;
            progress *= (maxOpacity - defaultOpacity);
            opacity.a = Mathf.Clamp(progress, defaultOpacity, maxOpacity);
        }
        else
        {
            opacity.a = defaultOpacity;
        }

        // Apply new opacity on Chameleon.
        renderer.material.color = opacity;
    }

    /// <summary>
    /// If Player collides with Chameleon, deals damage to Player and destroys the Chameleon gameobject.
    /// </summary>
    /// <param name="collision"> info on collision </param>
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag.Equals("Player"))
        {
            
            int damage = transform.GetComponent<EnemyHealth>().collisionDamage;
            player.GetComponent<PlayerInfo>().TakeDamage(damage);
            transform.GetComponent<EnemyHealth>().DestroyEnemy();
        }
    }
}