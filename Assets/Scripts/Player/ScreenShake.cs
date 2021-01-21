/*
 * Handles the transition and shake of the camera following the Player ship.
 * 
 * NOTES: If cameraToPlayerDistance is 0, then the script shall only shake the camera.
 *        X and Y positions after camera transition shall be the X and Y positions right before the transition.
 *        A camera transition event must not interrupt a running camera transition event.
 * 
 * 
 * Authors: Cristion Dominguez / Brad P
 * Date: 30 Oct. 2020
 * 
 */

/*
 * Revision Author: Cristion Dominguez
 * Revision Date: 27 Nov. 2020
 * 
 * Modifications: Fixed a bug where if the Player interacted with multiple objects, the Ship Camera would no longer be centered on the Player and not shake for each
 * encountered object.
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    [SerializeField]
    public bool isPermanent = true;  // Is the camera transition permanent?

    [SerializeField]
    protected float displacementTime = 5;  // time camera stays at displaced Z position if transition is temporary

    [SerializeField]
    protected float transitionDuration = 1;  // time to reach desired Z position

    [Header("Camera Displacement")]
    [SerializeField]
    public float cameraToPlayerDistance;  // distance the camera should move to relative to the Player gameobject; if value is 0,
                                          // then current distance in active scene shall be preserved and the camera shall only
                                          // shake

    [Header("Camera Shake")]
    [SerializeField]
    protected float shakeIntensity = 1;  // initial intensity of the shake

    [SerializeField]
    protected Vector2 shakeMagnitudeRatio = Vector2.one * 0.5f;  // max magnitude of shake effect on the X and Y axes

    [SerializeField]
    protected float shakeFrequency = 10;  // frequency of shaking; when set to low value, the camera shall shake slowly

    [SerializeField]
    protected float shakeDuration = 1;  // duration of shaking; value can not exceed transition duration

    protected float traumaExponent = 2; // allows an exponential fall in shake magnitude

    [SerializeField]
    public Cinemachine.CinemachineDollyCart dollyCart;  // dolly track behavior script

    public Transform shipCamera;  // camera following Player

    public CameraFollow cameraScript;  // script that has camera following Player

    public Vector2 originalX_YPosition;  // original X and Y positions of the ship camera before transition

    private IEnumerator cameraShake;  // camera shake event

    private IEnumerator cameraTransition; // camera transition event

    private bool isShaking = false; // Is the camera shaking?

    private Vector2 initialLimits;  // movement limits of camera before transition or shake

    /// <summary>
    /// Transitions and/or shakes the camera. If the camera is not shaking when this method is called, the initial position limits and the original X and Y position of the camera
    /// shall be saved; otherwise, the coroutine shaking the camera shall be stopped. Afterwards, if the camera to player distance is 0, then the camera shall only shake; otherwise,
    /// the camera shall transition and shake.
    /// </summary>
    public void ShakeCamera()
    {
        if (!isShaking)
        {
            originalX_YPosition = new Vector2(shipCamera.localPosition.x, shipCamera.localPosition.y);  // the X and Y positions of camera before transitioning
            initialLimits = cameraScript.limits;  // saving initial camera limits
        }
        else
            StopCoroutine(cameraShake);

        if (cameraToPlayerDistance == 0)
        {
            cameraShake = RattleCamera();
            StartCoroutine(cameraShake);
        }
        else
        {
            cameraTransition = MoveCamera();
            StartCoroutine(cameraTransition);
        }
    }

    /// <summary>
    /// Sets the shake intensity, frequency and duration for the Camera Shake event; then calls for the event.
    /// </summary>
    /// <param name="intensity"> shake intensity </param>
    /// <param name="frequency"> shake frequency </param>
    /// <param name="duration"> shake duration </param>
    public void ShakeCamera(float intensity, float frequency = 10f, float duration = 1f)
    {
        shakeIntensity = intensity;
        shakeFrequency = frequency;
        shakeDuration = duration;
        ShakeCamera();
    }

    /// <summary>
    /// Transitions the camera to a new Z position if the transition is permanent; otherwise, transitions the camera to a new Z position and stays there until displacement time
    /// is over, then transition the camera to its initial Z position.
    /// </summary>
    private IEnumerator MoveCamera()
    {
        float initialPositionZ = shipCamera.localPosition.z;  // save initial z position
        float newPositionZ = -cameraToPlayerDistance;  // determine new z position

        // If camera transition is permanent, then only transition camera once.
        if (isPermanent == true)
        {
            StartCoroutine(TransitionCamera(initialPositionZ, newPositionZ, true));  // move camera to new Z position w/ shake effect
            yield break;
        }

        // If camera transition is temporary, transition camera to a new Z position, wait until displacementTime is reached, and then return to initial Z position.
        StartCoroutine(TransitionCamera(initialPositionZ, newPositionZ, true));  // move camera to new Z position w/ shake effect
        yield return new WaitForSeconds(displacementTime);  // wait displacementTime until executing next coroutine
        StartCoroutine(TransitionCamera(newPositionZ, initialPositionZ, false));  // move camera to initial Z position w/o shake effect
    }

    /// <summary>
    /// Transitions camera from a starting Z position to an ending Z position for a transition duration. A camera shake can accompany this transition.
    /// </summary>
    /// <param name="startPositionZ"> starting Z position of the camera </param>
    /// <param name="endPositionZ"> ending Z position of the camera </param>
    /// <param name="doShake"> Should a shake coroutine accompany the transition coroutine? </param>
    public IEnumerator TransitionCamera(float startPositionZ, float endPositionZ, bool doShake)
    {
        float elapsedTime = 0;  // time passed since call of method
        float z; // current Z position of camera

        // Call the shake coroutine if doShake == true.
        if (doShake)
        {
            cameraShake = RattleCamera();
            StartCoroutine(cameraShake);
        }

        // Whilst elapsed time is less than transition duration, transition the camera towards the end Z position.
        while (elapsedTime < transitionDuration)
        {
            z = Mathf.Lerp(startPositionZ, endPositionZ, elapsedTime / transitionDuration);  // calculate current Z position

            shipCamera.localPosition = new Vector3(shipCamera.localPosition.x, shipCamera.localPosition.y, z);  // set new camera position

            elapsedTime += Time.deltaTime;  // update elapsed time
            yield return null;
        }

        // Snap camera to desired final position and notify script that camera is no longer transitioning.
        shipCamera.localPosition = new Vector3(shipCamera.localPosition.x, shipCamera.localPosition.y, endPositionZ);
    }

    /// <summary>
    /// Shakes camera for shake duration.
    /// </summary>
    private IEnumerator RattleCamera()
    {
        float elapsedTime = 0;  // time passed since call of method
        float trauma;  // current trauma of camera shake
        float shake = Mathf.Pow(shakeIntensity, traumaExponent); // percentage of max shake magnitude allowed, which shall decrease
        float perlinSeed = Random.value;  // seed for perlin noise (method that generates a psuedo-random value from 0 to 1)

        // Modify camera limits and notify script that the camera is shaking.
        cameraScript.limits = new Vector2(shakeMagnitudeRatio.x * shake, shakeMagnitudeRatio.y * shake);
        isShaking = true;

        // Whilst elapsed time is less than shake duration, shake the camera.
        while (elapsedTime < shakeDuration)
        {
            trauma = Mathf.Lerp(shakeIntensity, 0, elapsedTime / shakeDuration);  // calculate new trauma value
            shake = Mathf.Pow(trauma, traumaExponent);  // calculate new shake value

            // Set camera X and Y positions to psuedo-random values and preserve current Z position.
            shipCamera.localPosition = new Vector3(
                shakeMagnitudeRatio.x * (Mathf.PerlinNoise(perlinSeed, Time.time * shakeFrequency) * 2 - 1) * shake,
                shakeMagnitudeRatio.y * (Mathf.PerlinNoise(perlinSeed + 1, Time.time * shakeFrequency) * 2 - 1) * shake,
                shipCamera.localPosition.z);

            elapsedTime += Time.deltaTime;  // update elapsed time
            yield return null;
        }

        // Snap camera to desired final position, restore initial camera limits and notify script the camera is no longer shaking.
        shipCamera.localPosition = new Vector3(originalX_YPosition.x, originalX_YPosition.y, shipCamera.localPosition.z);
        cameraScript.limits = initialLimits;
        isShaking = false;
    }
}