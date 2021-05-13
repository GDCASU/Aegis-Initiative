using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Underwater : MonoBehaviour
{
    [Range(0,1)]
    public float reverb;
    private float lastReverb;

    [Range(0, 3)]
    public int level;

    private void Update()
    {
        if (reverb != lastReverb)
        {
            var rev = FMODUnity.FMODStartMusic.music.getReverbLevel(level, out float yarp);
            var test = FMODUnity.FMODStartMusic.music.setReverbLevel(level, reverb);
            lastReverb = reverb;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            FMODUnity.FMODStartMusic.music.setParameterByName("IsUnderwater", 1f);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            FMODUnity.FMODStartMusic.music.setParameterByName("IsUnderwater", 0f);
        }
    }
}
