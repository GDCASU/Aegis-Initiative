/*
 * Triggers speed increase/decrease for dolly track when Player collides with attached GameObject. Can be permanent or temporary depending on the ScreenShake script
 * attached to Player gameobject.
 * 
 * Author: Cristion Dominguez
 * Date: 30 Oct. 2020
 */

/*
* Revision Author: Cristion Dominguez
* Revision Date: 27 Nov. 2020
* 
* Modifications: Altered interaction with ScreenShake script revised on 27 Nov. 2020. Function remains the same.
* 
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

    private ScreenShake screenShake;  // instance of ScreenShake attached to Player

    /// <summary>
    /// If the attached gameobject collides with the Player, obtains ScreenShake script from Player and calls the AdjustSpeed coroutine.
    /// </summary>
    /// <param name="other"> the collided gameobject </param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            screenShake = other.GetComponent<ScreenShake>();
            StartCoroutine(AdjustSpeed());
        }
    }

    /// <summary>
    /// If the camera transition for the Player is permanent, permantantly applies the new speed to the dolly track and calls the ScreenCamera method from ScreenShake;
    /// otherwise, temporarily applies the new speed to the dolly track, calls the TransitionCamera coroutine from ScreenShake twice, and applies the initial speed
    /// to the dolly track afterwards.
    /// </summary>
    private IEnumerator AdjustSpeed()
    {
        // If speed adjustment is permanent, then only transition camera once and set a permanent speed for dolly track.
        if (screenShake.isPermanent == true)
        {
            screenShake.dollyCart.m_Speed = dollyTrackSpeed;  // apply new dolly track speed
            screenShake.ShakeCamera();

            yield break;
        }

        float initialPositionZ = screenShake.shipCamera.localPosition.z;  // save initial z position
        float newPositionZ = -screenShake.cameraToPlayerDistance;  // determine new z position

        // If speed adjustment is temporary, then set a temporary speed for dolly tracks and transition twice (first w/ shake, then w/o shake).
        float initialSpeed = screenShake.dollyCart.m_Speed;  // save initial dolly track speed
        screenShake.dollyCart.m_Speed = dollyTrackSpeed;  // apply new dolly track speed

        StartCoroutine(screenShake.TransitionCamera(initialPositionZ, newPositionZ, true));  // move camera to new Z position w shake effect
        yield return new WaitForSeconds(activeTimeForTemporarySpeed);  // wait activeTimeForTemporarySpeed until executing next instructions
        StartCoroutine(screenShake.TransitionCamera(newPositionZ, initialPositionZ, false));  // move camera to initial Z position w/o shake effect

        screenShake.dollyCart.m_Speed = initialSpeed;  // apply initial dolly track speed
    }
}
