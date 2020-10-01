using System.Collections;
using System.Collections.Generic;
using Unity.MPE;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public enum WaveMovement
    {
        None,
        UpDown,
        LeftRight
    }

    [SerializeField]
    private WaveMovement waveMovement;

    private float maxWavePeak; //wave height
    private float minWaveDip; //wave deepness
    private float waveFrequency; //frequency of wave
    private float maxAngle; //max angle of ship

    private bool atPosMax;

    private float pitch;
    private float yaw;
    private float roll;
    private float _forwardSpeed;

    private float startY; //local start Y position

    Transform shipModel;
    float time = 3;

    // Start is called before the first frame update
    void Start()
    {
        maxWavePeak = 1.2f;
        minWaveDip = 0;
        waveFrequency = 0.5f;
        maxAngle = 10;

        atPosMax = false;

        pitch = 0;
        yaw = 15;
        roll = 15;

        _forwardSpeed = 2;

        shipModel = transform.GetChild(0);
        startY = transform.localPosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        if(time > 0)
        {
            transform.Translate(new Vector3(0, pitch / _forwardSpeed, _forwardSpeed) * Time.deltaTime);
            time -= Time.deltaTime;
        }

        switch (waveMovement)
        {
            case WaveMovement.None:
                break;
            case WaveMovement.UpDown:
                UpDownWave();
                break;
            case WaveMovement.LeftRight:
                break;
            default:
                break;
        }
    }

    void UpDownWave()
    {
        checkDistance(transform.localPosition.y); //check if at peak or dip of wave
        if (!atPosMax) //at top of wave
        {
            if (pitch < maxAngle)
            {
                pitch += waveFrequency;
            }
        }
        else //at bottom of wave
        {
            if (pitch > -maxAngle)
            {
                pitch -= waveFrequency;
            }
        }

        shipModel.localEulerAngles = new Vector3(Mathf.LerpAngle(0, -pitch, 1f), 0, 0); //sin wave movement

        if(time <= 0)
        {
            transform.Translate(new Vector3(0, pitch / _forwardSpeed, 0) * Time.deltaTime); //only move on Y axis
        }
    }

    void checkDistance(float currentDist)
    {
        if(currentDist >= (startY + maxWavePeak))
        {
            atPosMax = true;
        }
        
        if(currentDist <= (startY + minWaveDip))
        {
            atPosMax = false;
        }
    }
}
