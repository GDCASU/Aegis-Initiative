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
    [Header("Speed Adjustment Type")]

    [SerializeField]
    private bool isPermanent = true;  // bool representing whether speed adjustment is permanent

    [Header("Speed Adjustment Values")]
    [SerializeField]
    private int dollyTrackSpeed = 10;  // new speed to set dolly track on

    [SerializeField]
    private float activeTimeForTemporarySpeed = 5;  // time temporary speed lasts for

    [Header("Camera Displacement")]
    [SerializeField]
    private float cameraDistance;  // value to displace the camera's Z local position; positive values moves camera away

    [SerializeField]
    private float transitionDuration;  // time to reach desired Z displacement

    [Header("Camera Shake")]
    [SerializeField]
    private float shakeIntensity = 1;  // initial intensity of the shake

    [SerializeField]
    private Vector2 shakeMagnitudeRatio = Vector2.one * 0.5f;  // max magnitude of shake effect on the X and Y axes

    [SerializeField]
    private float shakeFrequency = 10;  // frequency of shaking; when set to low value, the camera shall shake slowly

    [SerializeField]
    private float shakeDuration;  // duration of shaking; value can not exceed transition duration

    private float traumaExponent = 2;

    private Cinemachine.CinemachineDollyCart dollyCart;  // dolly track behavior script

    private Transform shipCamera;  // camera following Player

    private CameraFollow cameraScript;  // script that has camera following Player

    private Vector2 originalX_YPosition;  // original X and Y positions of the ship camera before transition

    /// <summary>
    /// Calls the AdjustSpeed coroutine when the Player collides with the object.
    /// </summary>
    /// <param name="other"> Collider of GameObject that passes through SpeedTrigger object </param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            dollyCart = other.transform.GetComponentInParent<Cinemachine.CinemachineDollyCart>();
            shipCamera = other.transform.parent.transform.Find("CM vcam1");
            cameraScript = shipCamera.GetComponent<CameraFollow>();

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
        if (isPermanent == true)
        {
            dollyCart.m_Speed = dollyTrackSpeed;  // apply new dolly track speed

            initialPositionZ = shipCamera.localPosition.z;  // save initial z position
            newPositionZ = shipCamera.localPosition.z - cameraDistance;  // calculate new z position
            originalX_YPosition = new Vector2(shipCamera.localPosition.x, shipCamera.localPosition.y);  // the X and Y positions of camera before transitioning

            StartCoroutine(TransitionCamera(initialPositionZ, newPositionZ, true));  // move camera to new Z position w/ shake effect
            yield break;
        }

        // If speed adjustment is temporary, then set a temporary speed for dolly tracks and transition twice (first w/ shake, then w/o shake).
        float initialSpeed = dollyCart.m_Speed;  // save initial dolly track speed
        dollyCart.m_Speed = dollyTrackSpeed;  // apply new dolly track speed

        initialPositionZ = shipCamera.localPosition.z;  // save initial z position
        newPositionZ = shipCamera.localPosition.z - cameraDistance;  // calculate new z position
        originalX_YPosition = new Vector2(shipCamera.localPosition.x, shipCamera.localPosition.y);  // the X and Y positions of camera before transitioning

        StartCoroutine(TransitionCamera(initialPositionZ, newPositionZ, true));  // move camera to new Z position w/o shake effect

        yield return new WaitForSeconds(activeTimeForTemporarySpeed);  // wait activeTimeForTemporarySpeed until executing next instructions

        StartCoroutine(TransitionCamera(newPositionZ, initialPositionZ, false));  // move camera to initial Z position w/ shake effect

        dollyCart.m_Speed = initialSpeed;  // apply initial dolly track speed
    }

    /// <summary>
    /// Transitions camera from start Z position to end Z position with shaking if it is enabled.
    /// </summary>
    /// <param name="startPositionZ"> starting Z position </param>
    /// <param name="endPositionZ"> ending Z position </param>
    /// <param name="doShake"> bool representing whether camera should shake during transition </param>
    private IEnumerator TransitionCamera(float startPositionZ, float endPositionZ, bool doShake)
    {
        float elapsedTime = 0;  // time passed since call of method
        float z, trauma;  // current Z position of camera; current trauma of camera shake (respectively)
        float shake = Mathf.Pow(shakeIntensity, traumaExponent);
        float perlinSeed = Random.value;  // seed for perlin noise (method that generates a psuedo-random value from 0 to 1)

        Vector2 initialLimits = cameraScript.limits;  // saving initial camera limits

        // Modify camera limits if shake is enabled.
        if (doShake == true)
            cameraScript.limits = new Vector2(shakeMagnitudeRatio.x * shake, shakeMagnitudeRatio.y * shake);

        // Whilst elapsed time is less than transition duration, transition the camera towards the end Z position and shake the camera if option is enabled.
        while (elapsedTime < transitionDuration)
        {
            z = Mathf.Lerp(startPositionZ, endPositionZ, elapsedTime / transitionDuration);  // calculate current Z position

            // If camera shake is enabled and elapsed time is less than shake duration, assign the X and Y positions psuedo-random values.
            if (doShake == true && elapsedTime < shakeDuration)
            {
                trauma = Mathf.Lerp(shakeIntensity, 0, elapsedTime / shakeDuration);  // calculate new trauma value
                shake = Mathf.Pow(trauma, traumaExponent);  // calculate new shake value

                // Set camera X and Y positions to psuedo-random values and set Z position to the calculated Z value.
                shipCamera.localPosition = new Vector3(
                    shakeMagnitudeRatio.x * (Mathf.PerlinNoise(perlinSeed, Time.time * shakeFrequency) * 2 - 1) * shake,
                    shakeMagnitudeRatio.y * (Mathf.PerlinNoise(perlinSeed + 1, Time.time * shakeFrequency) * 2 - 1) * shake,
                    z);
            }
            // If camera shake is not enabled, then only change Z position to calculated Z value
            else shipCamera.localPosition = new Vector3(originalX_YPosition.x, originalX_YPosition.y, z);

            elapsedTime += Time.deltaTime;  // update elapsed time
            yield return null;
        }

        shipCamera.localPosition = new Vector3(originalX_YPosition.x, originalX_YPosition.y, endPositionZ);  // snap camera to desired final position
        cameraScript.limits = initialLimits;  // restore initial camera limits
    }
}
