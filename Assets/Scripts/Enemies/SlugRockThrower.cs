using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlugRockThrower : MonoBehaviour
{
    public float randomTrackPositionRange;  //This randomly changes the base target position of the rock
    public Vector3 randomTargetPositionRange;   //This randomly moves the target position within a square around the base target

    public GameObject rockPrefab;
    public CinemachineSmoothPath path;
    public CinemachineDollyCart playerCart;

    /// <summary>
    /// Method to shoot rocks towards the player
    /// </summary>
    /// <param name="rockThrowCount">How many rocks to throw at once</param>
    /// <param name="timeTillImpact">How long it takes for the rock to hit the player (needed for physics calculations)</param>
    public void ShootRocks(int rockThrowCount, float timeTillImpact)
    {
        int randomTrackPosition = (int)Random.Range(0, randomTrackPositionRange);

        Vector3 randomTargetPosition = new Vector3(
            Random.Range(-randomTargetPositionRange.x, randomTargetPositionRange.x),
            0,
            Random.Range(-randomTargetPositionRange.z, randomTargetPositionRange.z)
        );

        Vector3 targetPosition = path.EvaluatePositionAtUnit(
            playerCart.m_Position + playerCart.m_Speed * timeTillImpact + randomTrackPosition,
            playerCart.m_PositionUnits + (int)(playerCart.m_Speed * timeTillImpact) + randomTrackPosition
        ) + randomTargetPosition;

        for(int x = 0; x < rockThrowCount; x++)
        {
            Rigidbody rockRb = Instantiate(rockPrefab, transform.position, transform.rotation).GetComponent<Rigidbody>();

            /**
             * This sets up the initial vector and is based on the physics distance equation
             * Delta x = 1/2 * a * t^2
             */
            Vector3 forceVector = (targetPosition - transform.position) / timeTillImpact;
            /**
             * This adds a initial y velocity so that the rock arcs to it's target position.
             * It is based off the same equation but accounts for the initial velocity
             * Y = Y_0 + V_y_0 * t - 1/2 * g * t^2
             */
            forceVector.y = ((targetPosition.y - transform.position.y) + 0.5f * Physics.gravity.y + Mathf.Pow(timeTillImpact, 2)) / timeTillImpact;

            rockRb.velocity = forceVector;
        }
    }

    private void Update()
    {
        /**
         * FOR TESTING PURPOSES ONLY. REMOVE ONCE FINISHED
         */
        if (Input.GetKeyDown(KeyCode.Space)) ShootRocks(1, 3f);
    }
}
