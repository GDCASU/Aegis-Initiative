/*
 * Bounces the Player away from the attached GameObject upon collision in a calculated or user-defined direction and specified distance for a specified duration.
 * For the current script version, Player has no control over ship during bounce process.
 * 
 * Author: Cristion Dominguez
 * Date: 20 Nov. 2020
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceMushroom : MonoBehaviour
{
    [Header("Direction Control")]
    [SerializeField]
    private bool directionCustomizable;  // bool representing whether the direction shall be defined by the script user

    [Header("Push Values")]
    [SerializeField]
    private float pushDistance = 0.5f;  // distance Player shall be pushed to after collision

    [SerializeField]
    private float pushDuration = 0.2f;  // duration Player shall be pushed after collision

    [Header("For Customized Direction")]
    [SerializeField]
    private float directionInUnitCircleAngles = 0;  // direction the Player shall be bounced to in degrees relative to the x-axis
                                                    // (utilized only if directionCustomizable == true)

    /// <summary>
    /// Collects the Players transform and determines the position to push the Player to after collision with attached game object. Passes the Player transform and push position
    /// into PushGameObject coroutine.
    /// </summary>
    /// <param name="collision"> info about the collision </param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            Transform player = collision.transform;
            Vector2 direction;

            // If bounce direction is customizable, calculate a vector of size 1 from the origin facing directionInUnitCircleAngles degrees from the x-axis.
            // Otherwise, calculate a vector from the collision point to the Player game object center.
            if (directionCustomizable)
                direction = new Vector2(Mathf.Cos(directionInUnitCircleAngles * Mathf.Deg2Rad), Mathf.Sin(directionInUnitCircleAngles * Mathf.Deg2Rad));
            else
                direction = player.position - collision.GetContact(0).point;

            // Establish a vector of push distance magnitude with the previously calculated direction.
            Vector2 pushPosition = new Vector2(direction.x, direction.y);
            pushPosition = pushPosition.normalized * pushDistance + new Vector2(player.localPosition.x, player.localPosition.y);

            // Move Player game object.
            StartCoroutine(PushGameObject(player, pushPosition));
        }
    }

    /// <summary>
    /// Moves Player game object to final position on the XY-plane for push duration.
    /// </summary>
    /// <param name="player"> transform of the Player </param>
    /// <param name="finalPosition"> position to move Player game object to </param>
    /// <returns></returns>
    private IEnumerator PushGameObject(Transform player, Vector2 finalPosition)
    {
        float elapsedTime = 0;  // time since call of coroutine
        Vector2 initialPosition = new Vector2(player.localPosition.x, player.localPosition.y);  // initial position of Player
        Vector2 currentPosition;  // current position of Player

        // Save original Player movement speed and set current Player movement speed to 0.
        float originalMovementSpeed = player.GetComponent<ShipMovement>().xySpeed;
        player.GetComponent<ShipMovement>().xySpeed = 0;

        // Whilst elapsed time < push duration, calculate and set the current position for Player as well as increment elapsed time.
        while (elapsedTime < pushDuration)
        {
            currentPosition = Vector2.Lerp(initialPosition, finalPosition, elapsedTime / pushDuration);
            player.transform.localPosition = new Vector3(currentPosition.x, currentPosition.y, player.localPosition.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Place the Player at the final position and return Player movement speed back to original value.
        player.transform.localPosition = finalPosition;
        player.GetComponent<ShipMovement>().xySpeed = originalMovementSpeed;
    }
}
