/*
 * Rotates each segment of the Sea Serpent in an array, except for the first segment.
 * 
 * Author: Cristion Dominguez
 * Date: 23 April 2021
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerpentSlither : MonoBehaviour
{
    // Representation of a Serpent segment
    private struct Segment
    {
        public Transform segTransform;
        public bool atPosMax;
        public float roll;
        public float startZ;
    }

    [Tooltip("Max positive value the enemy will reach when using a wave movement")]
    [SerializeField]
    [Range(0.0f, 5.0f)]
    private float maxWavePeak = 1.2f;
    [Tooltip("Max negative value the enemy will reach when using a wave movement")]
    [SerializeField]
    [Range(-5.0f, 0.0f)]
    private float minWaveDip = -1.2f;
    [Tooltip("Frequency of wave")]
    [SerializeField]
    [Range(0.0f, 2.0f)]
    private float waveFrequency = 0.5f;
    [Tooltip("Max angle the enemy can rotate to when rotating based on its movement")]
    [SerializeField]
    [Range(0.0f, 15.0f)]
    private float maxAngle = 10.0f;
    [Tooltip("Time before the next segment can commence slithering")]
    [SerializeField]
    private float slitherOffset = 0.3f;

    [Tooltip("Transforms of Serpent segments (1st transform shall not slither)")]
    [SerializeField]
    private Transform[] segmentTransforms;

    private Segment[] segments;

    /// <summary>
    /// Assigns variables, puts segment Transforms into an empty gameobject to avoid rotation issues, and initializes Serpent segment structs.
    /// </summary>
    private void Start()
    {
        segments = new Segment[segmentTransforms.Length];

        GameObject host;
        for(int i = 1; i < segments.Length; i++)
        {
            host = new GameObject();
            host.transform.parent = transform.GetChild(0);
            host.transform.localPosition = Vector3.zero;
            segmentTransforms[i].parent = host.transform;

            segments[i] = new Segment();
            segments[i].segTransform = host.transform;
            segments[i].atPosMax = false;
            segments[i].roll = 0;
            segments[i].startZ = host.transform.localPosition.z;
        }

        StartCoroutine(CommenceSlithering());
    }

    /// <summary>
    /// Activate slither movement one at a time for each segment in array.
    /// </summary>
    private IEnumerator CommenceSlithering()
    {
        int i = 1;
        while(i < segments.Length)
        {
            StartCoroutine(SlitherLeftAndRight(segments[i]));
            i++;
            yield return new WaitForSeconds(slitherOffset);
        }
    }

    /// <summary>
    /// Translates segment Transforms in a horizontal sine wave movement.
    /// </summary>
    /// <param name="someSegment"> just a random segment </param>
    private IEnumerator SlitherLeftAndRight(Segment someSegment)
    {
        float z;
        float currentZ;
        while(true)
        {
            currentZ = someSegment.segTransform.localPosition.z;
            if (currentZ >= (someSegment.startZ + maxWavePeak))
            {
                someSegment.atPosMax = true;
            }
            if (currentZ <= (someSegment.startZ + minWaveDip))
            {
                someSegment.atPosMax = false;
            }

            if (!someSegment.atPosMax) //at top of wave
            {
                if (someSegment.roll < maxAngle)
                {
                    someSegment.roll += waveFrequency;
                }
            }
            else //at bottom of wave
            {
                if (someSegment.roll > -maxAngle)
                {
                    someSegment.roll -= waveFrequency;
                }
            }

            z = someSegment.roll * waveFrequency;
            someSegment.segTransform.Translate(new Vector3(z, 0, 0) * Time.deltaTime);
            yield return null;
        }
    }
}
