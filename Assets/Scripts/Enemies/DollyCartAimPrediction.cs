/*
 * Predicts the player's future position relative to the Player Dolly Cart and calculates a bullet velocity vector to the position in World Space.
 * 
 * Author: Cristion Dominguez
 * Date: 22 Feb. 2021
 * 
 */

using UnityEngine;

public class DollyCartAimPrediction
{
    /// <summary>
    /// Returns a velocity vector in World Space for a bullet to collide with a target. Both the bullet and target must share the same parent (Dolly Cart).
    /// </summary>
    /// <param name="bulletSpawnpoint"> where bullet spawns </param>
    /// <param name="bulletSpeed"> speed of bullet </param>
    /// <param name="initialTargetPosition"> target position before prediction </param>
    /// <param name="targetVelocity"> the target's velocity vector </param>
    /// <param name="dollyCart"> the dolly cart housing the bullet and target </param>
    /// <param name="futureTargetPosition"> the predicted position where the target and bullet should collide </param>
    /// <param name="timeToReachTarget"> time for the bullet to reach the predicted target position </param>
    /// <returns> bullet velocity vector in World Space </returns>
    public static Vector3 GetBulletVelocityToTarget(Vector3 bulletSpawnpoint, float bulletSpeed, Vector3 initialTargetPosition, Vector3 targetVelocity, Transform dollyCart, out Vector3 futureTargetPosition, out float timeToReachTarget)
    {
        // The bullet, target, and bullet to target collision shall always be in a right angle triangle formation, thus
        // the Pythagorean Theorem is utilized: a^2 + b^2 = c^2
        // p_ti = initial target position, p_tf = final target position, p_bi = initial bullet position, v_t = target velocity, s_b = bullet speed, t = time

        // Calculate the time for an impact between target and bullet.
        // len(p_ti - p_bi)^2 + len(v_t * t)^2 = (s_b * t)^2 ==> t = sqrt[len(p_ti - p_bi)^2 / (s_b^2 - len(v_t)^2)]
        Vector3 bulletSpawnpointToTarget = initialTargetPosition - bulletSpawnpoint;
        float targetDistanceSquared = (bulletSpawnpointToTarget).sqrMagnitude;
        float bulletSpeedSquared = Mathf.Pow(bulletSpeed, 2);
        float targetSpeedSquared = targetVelocity.sqrMagnitude;
        float timeTillImpact;

        // If the target's speed is equal to or larger than the bullet speed, then pass a small number as the denominator for the time equation
        // to avoid math errors.
        if (targetSpeedSquared >= bulletSpeedSquared)
        {
            timeTillImpact = Mathf.Sqrt(targetDistanceSquared / 0.001f);
        }
        else
        {
            timeTillImpact = Mathf.Sqrt(targetDistanceSquared / (bulletSpeedSquared - targetSpeedSquared));
        }

        // Calculate the future target position at timeTillImpact.
        // p_tf = p_ti + v_t * t
        futureTargetPosition = initialTargetPosition + (targetVelocity * timeTillImpact);

        // Assign timeToReachTarget.
        timeToReachTarget = timeTillImpact;

        // Calculate the velocity vector of bullet.
        // p_bi + v_b * t = p_ti + v_t * t ==> v_b = v_t + (p_ti - p_bi) / t
        Vector3 velocityVector = targetVelocity + (bulletSpawnpointToTarget / timeTillImpact);
        velocityVector = dollyCart.TransformVector(velocityVector);
        return velocityVector;
    }
}