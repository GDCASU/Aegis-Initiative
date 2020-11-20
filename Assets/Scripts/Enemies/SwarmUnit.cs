/* 
 * Rotates the Swarm unit game object around an established origin and handles its dispersion from the Swarm.
 * 
 * Author: Cristion Dominguez
 * Date: 13 Nov. 2020
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmUnit : MonoBehaviour
{
    [Header("Unit Rotation and Disperse")]
    [SerializeField]
    private float spinSpeed = 700f;  // speed at which unit shall spin around an origin

    [SerializeField]
    private float spinRadius = 0.1f;  // distance at which unit shall be from origin

    [SerializeField]
    private float disperseSpeed = 0.4f;

    [Header("Origin Distance from Swarm Center")]
    [SerializeField]
    private float maxDistanceRatio = 1f;  // max distance (in fraction) at which unit origin can be placed from Swarm origin

    [SerializeField]
    private float minDistanceRatio = 0.7f;  // min distance (in fraction) at which unit origin can be placed from Swarm origin

    private GameObject origin;  // game object that unit shall spin around

    private bool isDispersing = false;  // representing whether the units are dispersing from the Swarm origin

    /// <summary>
    /// Sets up the origin game object when script is enabled.
    /// </summary>
    private void Start()
    {
        this.GetComponentInParent<SwarmBehavior>().OnPlayerCollision += SetDisperseDirection;
        SetOriginObject();
    }

    /// <summary>
    /// If isDispersing == true, moves the unit origin away from the Swarm origin; otherwise, rotates unit around its personal origin at spin speed every frame.
    /// </summary>
    private void Update()
    {
        if (isDispersing)
            origin.transform.Translate(origin.transform.forward * disperseSpeed);
        else
            this.transform.RotateAround(origin.transform.position, origin.transform.up, spinSpeed * Time.deltaTime);
    }
      
    /// <summary>
    /// Instantiates origin game object, makes it the parent of unit, determines origin and unit positions.
    /// </summary>
    private void SetOriginObject()
    {
        // Save local scale of unit.
        Vector3 initialUnitScale = this.transform.localScale;

        // Instantiate origin game object and make it the parent of unit.
        origin = new GameObject ("Unit Origin");
        origin.transform.SetParent(this.transform.parent);
        this.transform.SetParent(origin.transform);
        
        // Restore local scales of origin and unit.
        origin.transform.localScale = new Vector3(1f, 1f, 1f);
        this.transform.localScale = initialUnitScale;

        // Calculate a random position around the Swarm center and place the origin there.
        float swarmRadius = origin.transform.GetComponentInParent<SphereCollider>().radius;
        float randomDistance = Random.Range(minDistanceRatio, maxDistanceRatio) * (swarmRadius - spinRadius);
        origin.transform.localPosition = Random.insideUnitSphere.normalized * randomDistance;

        // Randomize origin rotation.
        origin.transform.rotation = Quaternion.Euler(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));

        // Calculate a random position around the origin center on its respective XZ-plane and place the unit there.
        Vector2 randomStartingPoint = Random.insideUnitCircle.normalized * spinRadius;
        this.transform.localPosition = new Vector3(randomStartingPoint.x, 0, randomStartingPoint.y);
    }

    /// <summary>
    /// Sets isDispersing to true and rotates the unit origin away from the Swarm origin.
    /// </summary>
    private void SetDisperseDirection()
    {
        isDispersing = true;
        origin.transform.rotation = Quaternion.LookRotation(origin.transform.localPosition);
    }

    /// <summary>
    /// Disables unit.
    /// </summary>
    public void DisableUnit()
    {
        this.GetComponentInParent<SwarmBehavior>().OnPlayerCollision -= SetDisperseDirection;
        this.transform.parent.gameObject.SetActive(false);
    }
}
