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

public class SpeedTrigger : ScreenShake
{
    [Header("Speed Adjustment Values")]
    [SerializeField]
    private int dollyTrackSpeed = 10;  // new speed to set dolly track on

    //BASICALLY HOW TO USE
    /*private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "SpeedTrigger")
        {
            StartCoroutine(AdjustSpeed(dollyTrackSpeed));
        }
    }*/

    /// <summary>
    /// If speed adjustment is permanent, permantantly applies the new speed to the dolly track and calls the TransitionCamera coroutine once. Otherwise, temporarily applies the new
    /// speed to the dolly track and calls the TransitionCamera coroutine twice.
    /// </summary>
    private IEnumerator AdjustSpeed(int speed = 10)
    {
        dollyTrackSpeed = speed;

        ShakeCamera();

        // If speed adjustment is temporary, then set a temporary speed for dolly tracks and transition twice (first w/ shake, then w/o shake).
        float initialSpeed = dollyCart.m_Speed;  // save initial dolly track speed
        dollyCart.m_Speed = dollyTrackSpeed;  // apply new dolly track speed

        if(isPermanent)
        {
            yield break;
        }

        yield return new WaitForSeconds(displacementTime);

        dollyCart.m_Speed = initialSpeed;  // apply initial dolly track speed
    }
}
