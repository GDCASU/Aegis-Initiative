/*
 * Maintains a list of temporary status effects on an attached gameobject.
 * NOTE: Small initial draft. Shall undergo immense changes if officers decide to keep it in project.
 * 
 * Author: Cristion Dominguez
 * Date: 2 January 2021
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A list of possible status effects.
public enum StatusEffects
{
    VULNERABILITY
}

public class StatusMechanics : MonoBehaviour
{
    private Dictionary<StatusEffects, IEnumerator> statuses = new Dictionary<StatusEffects, IEnumerator>();  // a list of status effects and their respective coroutines
                                                                                                             // that shall deactive them once a certain time is reached

    /// <summary>
    /// Adds or resets a status effect on the gameobject for a specified amount of time.
    /// </summary>
    /// <param name="effect"> a status effect </param>
    /// <param name="activeTime"> time the effect should last </param>
    public void ApplyStatusEffect(StatusEffects effect, float activeTime)
    {
        // If the gameobject already has a certain status effect, then stop the coroutine currently attached to that effect and initiate a new one.
        // Otherwise, add the status effect to the statuses list and start the coroutine.
        if (statuses.ContainsKey(effect))
        {
            StopCoroutine(statuses[effect]);
            statuses[effect] = StartEffectLifecycle(effect, activeTime);
        }
        else
            statuses.Add(effect, StartEffectLifecycle(effect, activeTime));

        StartCoroutine(statuses[effect]);
    }

    /// <summary>
    /// Removes the status effect on the gameobject after a specified amount of time.
    /// </summary>
    /// <param name="effect"> a status effect </param>
    /// <param name="activeTime"> time the effect should last </param>
    /// <returns></returns>
    private IEnumerator StartEffectLifecycle(StatusEffects effect, float activeTime)
    {
        yield return new WaitForSeconds(activeTime);
        statuses.Remove(effect);
    }

    /// <summary>
    /// Answers whether a gameobject possesses a certain status effect.
    /// </summary>
    /// <param name="effect"> a status effect </param>
    /// <returns> a true or false </returns>
    public bool HasStatusEffect(StatusEffects effect) => statuses.ContainsKey(effect);
}
