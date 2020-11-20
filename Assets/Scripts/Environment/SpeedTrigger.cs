/*
 * Script triggers speed increase for dolly track when Player collides with attached GameObject. Can be permanent or temporary.
 * Script deals solely with local positions when transitioning camera.
 * 
 * RESTRICTIONS: X and Y positions after camera transition shall be the X and Y positions right before the transition.
 *               Shake duration can not exceed transition duration.
 * 
 * Author: Cristion Dominguez
 * Date: 30 Oct. 2020
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedTrigger : MonoBehaviour
{
    [Header("Speed Adjustment Values")]
    [SerializeField]
    private int dollyTrackSpeed = 10;  // new speed to set dolly track on
    [SerializeField]
    private float activeTimeForTemporarySpeed = 5;  // time temporary speed lasts for
    private ScreenShake screenShake;

    //BASICALLY HOW TO USE
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            screenShake = other.GetComponent<ScreenShake>();
            StartCoroutine(AdjustSpeed());
        }
    }

    /// <summary>
    /// If speed adjustment is permanent, permantantly applies the new speed to the dolly track and calls the TransitionCamera coroutine once. Otherwise, temporarily applies the new
    /// speed to the dolly track and calls the TransitionCamera coroutine twice.
    /// </summary>
    private IEnumerator AdjustSpeed()
    {
        float initialPositionZ;
        float newPositionZ;

        // If speed adjustment is permanent, then only transition camera once and set a permanent speed for dolly track.
        if (screenShake.isPermanent == true)
        {
            screenShake.dollyCart.m_Speed = dollyTrackSpeed;  // apply new dolly track speed
            initialPositionZ = screenShake.shipCamera.localPosition.z;  // save initial z position
            newPositionZ = screenShake.shipCamera.localPosition.z - screenShake.cameraDistance;  // calculate new z position
            screenShake.originalX_YPosition = new Vector2(screenShake.shipCamera.localPosition.x, screenShake.shipCamera.localPosition.y);  // the X and Y positions of camera before transitioning

            StartCoroutine(screenShake.TransitionCamera(initialPositionZ, newPositionZ, true));  // move camera to new Z position w/ shake effect
            yield break;
        }
        // If speed adjustment is temporary, then set a temporary speed for dolly tracks and transition twice (first w/ shake, then w/o shake).
        float initialSpeed = screenShake.dollyCart.m_Speed;  // save initial dolly track speed
        screenShake.dollyCart.m_Speed = dollyTrackSpeed;  // apply new dolly track speed

        initialPositionZ = screenShake.shipCamera.localPosition.z;  // save initial z position
        newPositionZ = screenShake.shipCamera.localPosition.z - screenShake.cameraDistance;  // calculate new z position
        screenShake.originalX_YPosition = new Vector2(screenShake.shipCamera.localPosition.x, screenShake.shipCamera.localPosition.y);  // the X and Y positions of camera before transitioning

        StartCoroutine(screenShake.TransitionCamera(initialPositionZ, newPositionZ, true));  // move camera to new Z position w/o shake effect

        yield return new WaitForSeconds(activeTimeForTemporarySpeed);  // wait activeTimeForTemporarySpeed until executing next instructions

        StartCoroutine(screenShake.TransitionCamera(newPositionZ, initialPositionZ, false));  // move camera to initial Z position w/ shake effect

        screenShake.dollyCart.m_Speed = initialSpeed;  // apply initial dolly track speed
    }
}
