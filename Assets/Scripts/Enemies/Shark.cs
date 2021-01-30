/* 
 * Handles the spiral, charge and rotate movements as well as the lifetime of Shark gameobject.
 * 
 * Author: Cristion Dominguez
 * Date: 29 Jan. 2021
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shark : MonoBehaviour
{
    [Header("Revolve")]
    [SerializeField]
    private float revolveSpeed = 300f;  // speed at which Shark revolves around origin

    [SerializeField]
    private float revolveTime = 5f;  // time Shark should revolve for

    [SerializeField]
    private bool isRotating = true;  // Is the Shark itself rotating?

    [Header("Rotation to Player")]
    [SerializeField]
    private float rotateToPlayerTime = 1f;  // time Shark should rotate towards the Player

    [Header("Charge")]
    [SerializeField]
    private float chargeSpeed = 0.25f;  // speed at which Shark charges ahead

    [SerializeField]
    private float chargeTime = 2f;  // time Shark should charge until being destroyed

    private int collisionDamage;  // damage dealt to Player upon collision

    private GameObject origin;  // point the Shark shall revolve around

    private float yAngles = 0f;  // angles the Shark is on the y-axis

    /// <summary>
    /// Sets the origin for the Shark to revolve around, collision damage dealt to Player, and starts Shark movement towards the origin.
    /// </summary>
    private void Start()
    {
        origin = transform.parent.gameObject;
        collisionDamage = GetComponent<EnemyHealth>().collisionDamage;
        StartCoroutine(MoveToCenter());
    }

    /// <summary>
    /// For every frame, revolves Shark around origin and rotates Shark if isRotating.
    /// </summary>
    private void Update()
    {
        transform.RotateAround(origin.transform.position, origin.transform.forward, revolveSpeed * Time.deltaTime);

        if (!isRotating)
            transform.localEulerAngles = new Vector3(0, yAngles, 0);
    }

    /// <summary>
    /// Moves Shark towards the center (origin) at a calculated speed for revolveTime. At some point, rotates the Shark to the Player.
    /// </summary>
    private IEnumerator MoveToCenter()
    {
        float elapsedTime = 0;
        float toCenterSpeed = transform.localPosition.magnitude / revolveTime;  // speed at which Shark shall move towards origin
        float startToRotateTime = revolveTime - rotateToPlayerTime;  // time before the Shark begins rotating to Player
        bool isRotatingToPlayer = false;  // Is the Shark rotating to the Player?

        while (elapsedTime < revolveTime)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, Vector3.zero, toCenterSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            
            if (elapsedTime >= startToRotateTime && !isRotatingToPlayer)
            {
                StartCoroutine(RotateToPlayer());
                isRotatingToPlayer = true;
            }
            

            yield return null;
        }

        transform.localPosition = Vector3.zero;
    }

    /// <summary>
    /// Rotates the Shark to the Player for rotateToPlayerTime. Afterward, initiate the Shark's charge movement.
    /// </summary>
    private IEnumerator RotateToPlayer()
    {
        float elapsedTime = 0;
        float angularSpeed = 180 / rotateToPlayerTime;

        // If the Shark is rotating on its z-axis, alter the Shark's y-axis rotation directly.
        if (isRotating)
        {
            while (elapsedTime < rotateToPlayerTime)
            {
                transform.Rotate(0, angularSpeed * Time.deltaTime, 0);
                elapsedTime += Time.deltaTime;

                yield return null;
            }

            transform.localEulerAngles = new Vector3(0, 180f, 0);
        }
        // If Shark is NOT rotating on its z-axis, alter the the Shark's y-axis rotation via the yAngles variable.
        else
        {
            while (elapsedTime < rotateToPlayerTime)
            {
                yAngles += angularSpeed * Time.deltaTime;
                elapsedTime += Time.deltaTime;

                yield return null;
            }

            yAngles = 180f;
        }

        StartCoroutine(Charge());
    }

    /// <summary>
    /// Moves Shark in the front-facing direction at chargeSpeed for chargeTime. Afterward, destroys the Shark's parent gameobject.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Charge()
    {
        float elapsedTime = 0;

        while (elapsedTime < chargeTime)
        {
            origin.transform.Translate(transform.forward * chargeSpeed);
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        Destroy(origin);
    }

    /// <summary>
    /// If the Shark collides with Player, deals damage to the Player and destroys Shark parent.
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Player")
        {
            collision.transform.GetComponent<PlayerInfo>().TakeDamage(collisionDamage);
            Destroy(origin);
        }
    }

}
