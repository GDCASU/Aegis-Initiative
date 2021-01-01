using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlugRockThrower : MonoBehaviour
{
    public GameObject rockPrefab;
    public CinemachineSmoothPath path;
    public CinemachineDollyCart playerCart;

    /**
     * VARIABLES TO BE REMOVED AFTER PR APPROVAL
     */
    public float testSpread;
    public float testRotation;

    /// <summary>
    /// Method that shoots rocks out in a square area around the player. Not all rocks are guaranteed
    /// to be able to collide with the player
    /// </summary>
    /// <param name="timeTillImpact">Time for rock to hit player. Helps with physics calculations</param>
    /// <param name="spread">Modifies the square area the rocks can go around the player</param>
    /// <param name="rotationModifier">Modifies the random rock rotation</param>
    /// <param name="startingPosition">Desired location to spawn the rock</param>
    public void ShootRocksAroundPlayer(float timeTillImpact, float spread, float rotationModifier, Vector3 startingPosition)
    {
        //Random point within a square to throw the rock
        Vector3 randomTargetPosition = new Vector3(
            Random.Range(-spread, spread),
            0,
            Random.Range(-spread, spread)
        );

        //The players position at the given time
        Vector3 targetPosition = path.EvaluatePositionAtUnit(
            playerCart.m_Position + playerCart.m_Speed * timeTillImpact,
            playerCart.m_PositionUnits + (int)(playerCart.m_Speed * timeTillImpact)
        ) + randomTargetPosition;

        SpawnRock(timeTillImpact, rotationModifier, startingPosition, targetPosition);
    }

    /// <summary>
    /// Method that shoots rocks out towards the player. These rocks are meant to be able to hit the player.
    /// </summary>
    /// <param name="timeTillImpact">Time for rock to hit player. Helps with physics calculations</param>
    /// <param name="spread">Modifies how far to the right and left the rock will be thrown to the player</param>
    /// <param name="rotationModifier">Modifies the random rock rotation</param>
    /// <param name="startingPosition">Desired location to spawn the rock</param>
    public void ShootRockAtPlayer(float timeTillImpact, float spread, float rotationModifier, Vector3 startingPosition)
    {
        //The players position at the given time
        Vector3 targetPosition = path.EvaluatePositionAtUnit(
            playerCart.m_Position + playerCart.m_Speed * timeTillImpact,
            playerCart.m_PositionUnits + (int)(playerCart.m_Speed * timeTillImpact)
        );

        //Normalized tangent vector where the player will be at a given time
        Vector3 targetTangent = Vector3.Normalize(
            path.EvaluateTangentAtUnit(
                playerCart.m_Position + playerCart.m_Speed * timeTillImpact,
                playerCart.m_PositionUnits + (int)(playerCart.m_Speed * timeTillImpact)
            )
        );

        /**
         * Randomly chooses to go left or right of the player
         */
        if (Random.value < 0.5f)
        {
            targetTangent.x *= -1;
        }
        else
        {
            targetTangent.z *= -1;
        }

        targetTangent.y = 0;
        //Gives random length to the left/right modifier
        targetTangent *= Random.Range(-spread, spread);

        SpawnRock(timeTillImpact, rotationModifier, startingPosition, targetPosition + targetTangent);
    }

    /// <summary>
    /// Method that creates a new rock game object and throws it based on physics calculations
    /// </summary>
    /// <param name="timeTillImpact">Time for rock to hit player. Helps with physics calculations</param>
    /// <param name="rotationModifier">Modifies the random rock rotation</param>
    /// <param name="startingPosition">Desired location to spawn the rock</param>
    /// <param name="targetPosition">The final position of the rock after the given time</param>
    public void SpawnRock(float timeTillImpact, float rotationModifier, Vector3 startingPosition, Vector3 targetPosition)
    {
        Rigidbody rockRb = Instantiate(rockPrefab, startingPosition, Random.rotation).GetComponent<Rigidbody>();

        /**
         * This sets up the initial vector and is based on the physics distance equation
         * Delta x = 1/2 * a * t^2
         */
        Vector3 forceVector = (targetPosition - startingPosition) / timeTillImpact;
        /**
         * This adds a initial y velocity so that the rock arcs to it's target position.
         * It is based off the same equation but accounts for the initial velocity
         * Y = Y_0 + V_y_0 * t - 1/2 * g * t^2
         */
        forceVector.y = ((targetPosition.y - startingPosition.y) - 0.5f * Physics.gravity.y * Mathf.Pow(timeTillImpact, 2)) / timeTillImpact;

        rockRb.velocity = forceVector;
        //Random rotation on rock
        rockRb.angularVelocity = new Vector3(
            Random.Range(-rotationModifier, rotationModifier),
            Random.Range(-rotationModifier, rotationModifier),
            Random.Range(-rotationModifier, rotationModifier)
        );
    }

    private void Update()
    {
        /**
         * FOR TESTING PURPOSES ONLY. REMOVE ONCE PR APPROVED
         */
        if (Input.GetKeyDown(KeyCode.Space)) ShootRockAtPlayer(3f, testSpread, testRotation, transform.position);
    }
}
