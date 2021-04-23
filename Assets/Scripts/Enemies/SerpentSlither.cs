using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerpentSlither : MonoBehaviour
{
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
    private float maxAngle = 10.0f; //max angle of ship

    [SerializeField]
    private float offset = 0.3f;

    [SerializeField]
    private Transform[] segmentTransforms;

    private Segment[] segments;

    private bool stride = false;
    private Transform head;

    private void Start()
    {
        head = transform.GetChild(0).GetChild(0);
        segments = new Segment[segmentTransforms.Length];

        GameObject host;
        for(int i = 1; i < segments.Length; i++)
        {
            if(!segmentTransforms[i].name.Equals("Head Collider"))
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
            else
            {
                segments[i] = new Segment();
                segments[i].segTransform = segmentTransforms[i];
                segments[i].atPosMax = false;
                segments[i].roll = 0;
                segments[i].startZ = segmentTransforms[i].localPosition.z;
            }
        }

        StartCoroutine(CommenceSlithering());
    }

    private IEnumerator CommenceSlithering()
    {
        int i = 1;
        while(i < segments.Length)
        {
            StartCoroutine(SlitherLeftAndRight(segments[i]));
            i++;
            yield return new WaitForSeconds(offset);
        }
    }

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
