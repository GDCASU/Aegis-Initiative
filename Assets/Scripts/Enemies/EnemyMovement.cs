/*
 * Revision Author: Cristion Dominguez
 * Revision Date: 28 Feb. 2021
 * 
 * Modifcation: Added RotationType to specify how a gameobject should rotate HoverInterval to specify if a gameobject should hover temporarily or permanently.
 */
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

    public enum LeaveDirection
    {
        None,
        UpLeft,
        Up,
        UpRight,
        Left,
        Right
    }

    public enum MoveDirection
    {
        Towards = -1,
        Away = 1
    }

    public enum RotationType
    {
        AwayFromPlayer,  // look away from Player's general position whilst swaying
        TowardsPlayer,  // look towards the Player's general position whilst swaying
        LockedOntoPlayer  // look at the Player without swaying
    }

    public enum HoverInterval
    {
        Temporary,  // hover until hoverTimer reaches 0
        Permanent  // hover permanently
    }

    [SerializeField]
    private WaveMovement waveMovement;
    [SerializeField]
    private RotationType rotation = RotationType.AwayFromPlayer;
    [SerializeField]
    [Range(0.0f, 5.0f)]
    private float maxWavePeak = 1.2f; //wave height
    [SerializeField]
    [Range(-5.0f, 0.0f)]
    private float minWaveDip = 0; //wave deepness
    [SerializeField]
    [Range(0.0f, 2.0f)]
    private float waveFrequency = 0.5f; //frequency of wave
    [SerializeField]
    [Range(0.0f, 15.0f)]
    private float maxAngle = 10.0f; //max angle of ship
    [Space]
    [SerializeField]
    [Range(0.0f, 5.0f)]
    private float shipSpeed = 2;
    [SerializeField]
    private HoverInterval hoverLength = HoverInterval.Temporary;
    [SerializeField]
    [Range(0.0f, 10.0f)]
    private float hoverTimer = 3.0f;  // how long the ship hovers if temporary
    [SerializeField]
    private LeaveDirection leaveDirection;
    [SerializeField]
    private MoveDirection moveDirection;

    private bool atPosMax;

    private float pitch;
    private float yaw;
    private float roll;

    private float flyAwayPitch;
    private float flyAwayYaw;
    private float flyAwayRoll;

    private float startY; //local start Y position
    private float startX; //local start X position

    Transform shipModel;
    float time = 3;

    // Start is called before the first frame update
    void Start()
    {
        atPosMax = false;

        pitch = 0;
        yaw = 0;
        roll = 0;

        flyAwayPitch = -60;
        flyAwayYaw = -60;
        flyAwayRoll = 60;

        shipModel = transform.GetChild(0);
        startY = transform.localPosition.y;
        startX = transform.localPosition.x;
    }

    // Update is called once per frame
    void Update()
    {
        switch (waveMovement)
        {
            case WaveMovement.None:
                Move(0, 0);
                break;
            case WaveMovement.UpDown:
                UpDownWave();
                break;
            case WaveMovement.LeftRight:
                LeftRight();
                break;
            default:
                break;
        }
    }

    void UpDownWave()
    {
        checkBounds(transform.localPosition.y); //check if at peak or dip of wave
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
        RotateShip(Mathf.LerpAngle(0, -pitch, 1f), 0, 0); //up down tilting

        Move(0, pitch * waveFrequency); //move up/down
    }

    void LeftRight()
    {
        checkBounds(transform.localPosition.x); //check if at peak or dip of wave
        if (!atPosMax) //at top of wave
        {
            if (roll < maxAngle)
            {
                roll += waveFrequency;
            }
        }
        else //at bottom of wave
        {
            if (roll > -maxAngle)
            {
                roll -= waveFrequency;
            }
        }

        if(hoverTimer > 0)
        {
            RotateShip(0, 0, Mathf.LerpAngle(0, -roll, 1f)); //left right tilting
        }
        
        Move(roll * waveFrequency, 0); //move left/right
    }

    void Move(float x, float y)
    {
        RotateShip(0, 0, 0);

        if(time > 0)
        {
            transform.Translate(new Vector3(x, y, shipSpeed * (int)moveDirection) * Time.deltaTime); //ship movement wave or no wave
            time -= Time.deltaTime;
        }
        else
        {
            if(hoverLength == HoverInterval.Permanent)
            {
                transform.Translate(new Vector3(x, y, 0) * Time.deltaTime); //ship will hover
            }
            else if(hoverTimer > 0 && hoverLength == HoverInterval.Temporary)
            {
                transform.Translate(new Vector3(x, y, 0) * Time.deltaTime); //ship will hover
                hoverTimer -= Time.deltaTime;
            }
            else
            {
                FlyAway();
            }
        }
    }

    void FlyAway()
    {
        rotation = RotationType.AwayFromPlayer;

        switch(leaveDirection)
        {
            //pitch yaw roll
            case (LeaveDirection.UpLeft):
                RotateShip(Mathf.LerpAngle(0, flyAwayPitch, 1f), Mathf.LerpAngle(0, flyAwayYaw, 1f), Mathf.LerpAngle(0, flyAwayRoll, 1f)); //pitch/yaw/roll
                break;
            case (LeaveDirection.Up):
                RotateShip(Mathf.LerpAngle(0, flyAwayPitch, 1f), 0, 0); //pitch
                break;
            case (LeaveDirection.UpRight):
                RotateShip(Mathf.LerpAngle(0, flyAwayPitch, 1f), Mathf.LerpAngle(0, -flyAwayYaw, 1f), Mathf.LerpAngle(0, -flyAwayRoll, 1f)); //pitch/-yaw/-roll
                break;
            case (LeaveDirection.Left):
                RotateShip(0, Mathf.LerpAngle(0, flyAwayYaw * 1.5f, 1f), Mathf.LerpAngle(0, flyAwayRoll * 1.5f, 1f)); //0/yaw/roll
                break;
            case (LeaveDirection.Right):
                RotateShip(0, Mathf.LerpAngle(0, -flyAwayYaw * 1.5f, 1f), Mathf.LerpAngle(0, -flyAwayRoll * 1.5f, 1f)); //0/-yaw/-roll
                break;
        }
        transform.Translate(shipModel.forward * Time.deltaTime * shipSpeed * 3);
    }

    void RotateShip(float x, float y, float z)
    {
        if (rotation == RotationType.AwayFromPlayer)
            shipModel.localEulerAngles = new Vector3(x, y, z);
        else if (rotation == RotationType.TowardsPlayer)
            shipModel.localEulerAngles = new Vector3(x, y + 180, z);
        else if (rotation == RotationType.LockedOntoPlayer)
            shipModel.LookAt(PlayerInfo.singleton.transform.position);
    }

    void checkBounds(float currentValue)
    {
        if (waveMovement == WaveMovement.UpDown)
        {
            if (currentValue >= (startY + maxWavePeak)) atPosMax = true;
            if (currentValue <= (startY + minWaveDip)) atPosMax = false;
        }
        else if (waveMovement == WaveMovement.LeftRight)
        {
            if (currentValue >= (startX + maxWavePeak)) atPosMax = true;
            if (currentValue <= (startX + minWaveDip)) atPosMax = false;
        }

    }
}
