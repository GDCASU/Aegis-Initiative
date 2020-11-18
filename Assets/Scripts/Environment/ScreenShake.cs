using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    [SerializeField]
    private bool isPermanent = true;  // bool representing whether speed adjustment is permanent

    [SerializeField]
    private float displacementTime = 5;  // time camera is displaced from original position

    [SerializeField]
    private float transitionDuration = 1;  // time to reach desired Z displacement

    [Header("Camera Displacement")]
    [SerializeField]
    private float cameraDistance;  // value to displace the camera's Z local position; positive values moves camera away

    [Header("Camera Shake")]
    [SerializeField]
    private float shakeIntensity = 1;  // initial intensity of the shake

    [SerializeField]
    private Vector2 shakeMagnitudeRatio = Vector2.one * 0.5f;  // max magnitude of shake effect on the X and Y axes

    [SerializeField]
    private float shakeFrequency = 10;  // frequency of shaking; when set to low value, the camera shall shake slowly

    [SerializeField]
    private float shakeDuration = 1;  // duration of shaking; value can not exceed transition duration

    private float traumaExponent = 2;

    protected Cinemachine.CinemachineDollyCart dollyCart;  // dolly track behavior script

    private Transform shipCamera;  // camera following Player

    private CameraFollow cameraScript;  // script that has camera following Player

    private Vector2 originalX_YPosition;  // original X and Y positions of the ship camera before transition

    public void ShakeCamera() //for script to call
    {
        dollyCart = transform.GetComponentInParent<Cinemachine.CinemachineDollyCart>();
        shipCamera = dollyCart.GetComponentInChildren<Cinemachine.CinemachineVirtualCamera>().transform; // Changed this so that it uses a component instead of a name to reduce the risk of having to retouch the script anytime that the camera name is changed
        cameraScript = shipCamera.GetComponent<CameraFollow>();

        StartCoroutine(CameraMovement());
    }

    private IEnumerator CameraMovement()
    {
        float initialPositionZ;
        float newPositionZ;

        initialPositionZ = shipCamera.localPosition.z;  // save initial z position
        newPositionZ = shipCamera.localPosition.z - cameraDistance;  // calculate new z position
        originalX_YPosition = new Vector2(shipCamera.localPosition.x, shipCamera.localPosition.y);  // the X and Y positions of camera before transitioning

        // If speed adjustment is permanent, then only transition camera once and set a permanent speed for dolly track.
        if (isPermanent == true)
        {
            StartCoroutine(TransitionCamera(initialPositionZ, newPositionZ, true));  // move camera to new Z position w/ shake effect
            yield break;
        }

        StartCoroutine(TransitionCamera(initialPositionZ, newPositionZ, true));  // move camera to new Z position w/o shake effect

        yield return new WaitForSeconds(displacementTime);  // wait activeTimeForTemporarySpeed until executing next instructions

        StartCoroutine(TransitionCamera(newPositionZ, initialPositionZ, false));  // move camera to initial Z position w/ shake effect
    }

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
