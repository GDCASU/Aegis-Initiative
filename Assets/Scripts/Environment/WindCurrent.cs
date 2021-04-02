/*
 * Pushes Player in a specified direction with a certain force upon trigger collision.
 * The velocity brought upon by the collision shall be set to 0 once the Player ceases touching the Wind Current.
 * 
 * Author: Cristion Dominguez
 * Date: 19 March 2021
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindCurrent : MonoBehaviour
{
    [Tooltip("Direction to push the player in angles with respect to the +x-axis.")]
    [SerializeField]
    private float directionInUnitCircleAngles = 0f;
    [Tooltip("Force to push Player with.")]
    [SerializeField]
    private float pushForce = 3f;

    private Rigidbody player;

    /// <summary>
    /// Obtains Player rigidbody.
    /// </summary>
    private void Start() => player = PlayerInfo.singleton.transform.GetComponent<Rigidbody>();

    /// <summary>
    /// Calculates the velocity vector for the Wind Current to push the player towards and adds the velocity to the Player.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            Vector2 direction = new Vector2(Mathf.Cos(directionInUnitCircleAngles * Mathf.Deg2Rad), Mathf.Sin(directionInUnitCircleAngles * Mathf.Deg2Rad));
            direction *= pushForce;
            Vector2 velocity = new Vector3(direction.x, direction.y, 0f);

            player.AddForce(velocity, ForceMode.VelocityChange);
        }
    }

    /// <summary>
    /// Sets the Player velocity to 0 when the Wind Current ceases touching the Player.
    /// </summary>
    /// <param name="other"> collider of encountered gameobject </param>
    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            player.velocity = new Vector3(0, 0, 0);
        }
    }
}
