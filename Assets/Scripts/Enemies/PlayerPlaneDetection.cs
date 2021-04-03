/*
 * Notifies Flying Bat-Squirrel to stop following the Player after colliding with Player.
 * 
 * Author: Cristion Dominguez
 * Date: 26 March 2021
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlaneDetection : MonoBehaviour
{
    private FlyingBatSquirrel squirrel;

    /// <summary>
    /// Initializes Flying Bat-Squirrel script.
    /// </summary>
    private void Start() => squirrel = transform.parent.GetChild(0).GetComponent<FlyingBatSquirrel>();

    /// <summary>
    /// Notifies Flying Bat-Squirrel to stop following the Player upon collision.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
            squirrel.StopFollowingPlayer();
    }
}
