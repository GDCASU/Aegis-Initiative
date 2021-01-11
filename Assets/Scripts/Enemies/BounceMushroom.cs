/*
 * Bounces the Player away from the attached GameObject upon collision in a calculated or user-defined direction and specified distance for a specified duration.
 * For the current script version, Player has no control over ship during bounce process.
 * 
 * Author: Cristion Dominguez
 * Date: 20 Nov. 2020
 * 
 */

 /*
  * Revision Author: Cristion Dominguez
  * Revision Date: 27 Nov. 2020
  * 
  * Modifications: Fixed a bug where if the Player interacted with multiple Bounce Mushrooms, they would undergo multiple pushing coroutines and have their velocity
  * and angular velocity altered.
  * 
  */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class BounceMushroom : MonoBehaviour
{
    [Header("Direction Control")]
    [SerializeField]
    private bool directionCustomizable;  // Shall the direction be defined by the script user?

    [Header("Push Values")]
    [SerializeField]
    private float pushDistance = 0.5f;  // distance Player shall be pushed to after collision

    [SerializeField]
    private float pushDuration = 0.2f;  // duration Player shall be pushed after collision

    [Header("For Customized Direction")]
    [SerializeField]
    private float directionInUnitCircleAngles = 0;  // direction the Player shall be bounced to in degrees relative to the x-axis
                                                    // (utilized only if directionCustomizable == true)

    private bool bounceable = true;  // Shall the Bounce Mushroom bounce the Player? 

    private static bool isBouncing = false;  // Is the Player currently bouncing away from a Bounce Mushroom? [Shared amidst all Bounce Mushrooms.]

    private static float originalPlayerSpeed;  // original speed of Player ship [Shared amidst all Bounce Mushrooms.]

    /// <summary>
    /// Collects the Players transform and determines the position to push the Player to after collision with attached game object. Passes the Player transform and push position
    /// into PushGameObject coroutine. If the Player is bounced into another Bounce Mushroom, the push effect from the previous mushroom is cancelled.
    /// </summary>
    /// <param name="collision"> info about the collision </param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player" && bounceable == true)
        {
            // Acquire Player transform and initiate the direction variable.
            Transform player = collision.transform;
            Vector2 direction;

            // If the Player is not under a bounce effect, save its original movement speed.
            if (!isBouncing)
                originalPlayerSpeed = player.GetComponent<ShipMovement>().xySpeed;

            // Stop the Player from bouncing and do not allow the Bounce Mushroom to affect the Player anymore.
            isBouncing = false;
            bounceable = false;

            // If bounce direction is customizable, calculate a vector of size 1 from the origin facing directionInUnitCircleAngles degrees from the x-axis.
            // Otherwise, calculate a vector from the collision point to the Player game object center.
            if (directionCustomizable)
                direction = new Vector2(Mathf.Cos(directionInUnitCircleAngles * Mathf.Deg2Rad), Mathf.Sin(directionInUnitCircleAngles * Mathf.Deg2Rad));
            else
                direction = player.position - collision.GetContact(0).point;

            // Establish a vector of push distance magnitude with the previously calculated direction.
            Vector2 pushPosition = new Vector2(direction.x, direction.y);
            pushPosition = pushPosition.normalized * pushDistance + new Vector2(player.localPosition.x, player.localPosition.y);

            // Push the Player gameobject.
            StartCoroutine(PushGameObject(player, pushPosition));
        }
    }

    /// <summary>
    /// Moves Player game object to final position on the XY-plane for push duration as long as the Player does not interact with another Bounce Mushroom.
    /// </summary>
    /// <param name="player"> transform of the Player </param>
    /// <param name="finalPosition"> position to move Player game object to </param>
    /// <returns></returns>
    private IEnumerator PushGameObject(Transform player, Vector2 finalPosition)
    {
        float elapsedTime = 0;  // time since coroutine has first moved the Player
        Vector2 initialPosition = new Vector2(player.localPosition.x, player.localPosition.y);  // initial position of Player
        Vector2 currentPosition;  // current position of Player

        // Set current Player movement speed to 0.
        player.GetComponent<ShipMovement>().xySpeed = 0;

        // Allow the coroutine to push Player.
        isBouncing = true;

        // Whilst elapsed time < push duration, if the Player does not encounter another Bounce Mushroom, calculate and set the current position for Player 
        // as well as increment elapsed time. Otherwise, reset the Player's velocity and angular velocity, and then exit coroutine.
        while (elapsedTime < pushDuration)
        {
            if (isBouncing == true)
            {
                currentPosition = Vector2.Lerp(initialPosition, finalPosition, elapsedTime / pushDuration);
                player.transform.localPosition = new Vector3(currentPosition.x, currentPosition.y, player.localPosition.z);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            else
            {
                player.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                player.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 0);
                yield break;
            }
        }

        // Place the Player at the final position, return Player movement speed back to original value, and notify other Bounce Mushrooms that the Player
        // is not bouncing.
        player.transform.localPosition = finalPosition;
        player.GetComponent<ShipMovement>().xySpeed = originalPlayerSpeed;
        isBouncing = false;
    }
}