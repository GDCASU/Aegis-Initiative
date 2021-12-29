/*
 * Revision Author: Cristion Dominguez
 * Revision Date: April 23, 2021
 * Modification: Hover can now be permanent.
 */

using System.Collections;
using System.Collections.Generic;
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

    public enum FlyingInDirection
    {
        Towards = -1,
        Player = 0,
        Away = 1,
    }

    [Header("Basic Variables")]
    [Tooltip("Does the enemy rotate based on its movement")]
    [SerializeField]
    private bool rotateWithMovement = true;
    [Tooltip("Does the enemy constantly look at the player")]
    [SerializeField]
    private bool lockedOntoPlayer = false;
    [Tooltip("Type of movement the enemy has when not flying away")]
    [SerializeField]
    private WaveMovement waveMovement;

    [Tooltip("Max positive value the enemy will reach when using a wave movement")]
    [SerializeField]
    [Range(0.0f, 25.0f)]
    private float maxWavePeak = 1.2f;

    [Tooltip("Max negative value the enemy will reach when using a wave movement")]
    [SerializeField]
    [Range(-25.0f, 0.0f)]
    private float minWaveDip = -1.2f;

    [Tooltip("Frequency of wave")]
    [SerializeField]
    [Range(0.0f, 2.0f)]
    private float waveFrequency = 0.5f;
    [Tooltip("Max angle the enemy can rotate to when rotating based on its movement")]
    [SerializeField]
    [Range(0.0f, 15.0f)]
    private float maxAngle = 10.0f; //max angle of ship

    [Header("Flying In Variables")]
    [Tooltip("How fast the enemy ship moves when flying in")]
    [SerializeField]
    [Range(0.0f, 5.0f)]
    private float flyingInSpeed = 2;
    [SerializeField]
    [Tooltip("Duration of the enemy flying in sequence")]
    [Range(0.0f, 25.0f)]
    float flyingInTime = 3;
    [Tooltip("Towards or Away from the player when flying in")]
    [SerializeField]
    private FlyingInDirection flyingInDirection = FlyingInDirection.Towards;
    [Tooltip("Is the enemy flying in")]
    [SerializeField]
    public bool flyingIn = true;

    [Header("Hover Variables")]
    [Tooltip("Does ship follow the player after flying in")]
    [SerializeField]
    private bool hover = true;
    [Tooltip("Duration that the enemy will follow the player for")]
    [SerializeField]
    [Range(0.0f, 10.0f)]
    public float hoverTimer = 3.0f; //how long ship stays
    [SerializeField]
    private bool isHoverPermanent = false;

    [Header("Leave Variables")]
    [Tooltip("Determines the leave rotation")]
    [SerializeField]
    private bool isModelFacingPlayer = false;
    [Tooltip("Direction the enemy will leave based on the players perspective")]
    [SerializeField]
    private LeaveDirection leaveDirection;
    [Tooltip("Speed that the enemy will leave at")]
    [SerializeField]
    [Range(0.0f, 30.0f)]
    private float leaveSpeed = 5;

    private bool atPosMax;

    private float pitch;
    private float yaw;
    private float roll;
    private float t;

    private float i, j, k;

    private float flyAwayPitch;
    private float flyAwayYaw;
    private float flyAwayRoll;

    private float startY; //local start Y position
    private float startX; //local start X position

    public bool isFlyingAway;

    Transform shipModel;
    Vector3 directionToPlayer;

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
    void FixedUpdate()
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
        CheckBounds(transform.localPosition.y); //check if at peak or dip of wave
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
        RotateShip(Mathf.LerpAngle(0, -pitch, t += Time.fixedDeltaTime), 0, 0); //up down tilting

        Move(0, pitch * waveFrequency); //move up/down
    }

    void LeftRight()
    {
        CheckBounds(transform.localPosition.x); //check if at peak or dip of wave
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
            RotateShip(0,0 , Mathf.LerpAngle(0, -roll, t += Time.fixedDeltaTime)); //left right tilting
        }
        
        Move(roll * waveFrequency, 0); //move left/right
    }

    void Move(float x, float y)
    {
        if(!rotateWithMovement)RotateShip(0, 0, 0);

        if(flyingInTime > 0)
        {
            directionToPlayer = (PlayerInfo.singleton.transform.position - transform.position).normalized;
            transform.Translate( ((flyingInDirection == FlyingInDirection.Player && PlayerInfo.singleton )? directionToPlayer * flyingInSpeed:   new Vector3(x, y, flyingInSpeed * (int)flyingInDirection)) * Time.fixedDeltaTime); //ship movement wave or no wave
            flyingInTime -= Time.fixedDeltaTime;
            if (flyingInDirection == FlyingInDirection.Player)
            {
                startY = transform.localPosition.y;
                startX = transform.localPosition.x;
            }
            if (flyingInTime <= 0) flyingIn = false;
            
        }
        else
        {
            if(isHoverPermanent || (hoverTimer > 0 && hover))
            {
                transform.Translate(new Vector3(x, y, 0) * Time.fixedDeltaTime); //ship will hover
                hoverTimer -= Time.fixedDeltaTime;
            }
            else
            {
                if (!isFlyingAway)
                {
                    t = 0;
                    isFlyingAway = true;
                    i = shipModel.transform.localEulerAngles.x;
                    j = shipModel.transform.localEulerAngles.y;
                    k = shipModel.transform.localEulerAngles.z;
                }
                FlyAway();
            }
        }
    }

    void FlyAway()
    {
        rotateWithMovement = true;
        t += Time.fixedDeltaTime;
        switch (leaveDirection)
        {
            //pitch yaw roll
            case (LeaveDirection.UpLeft):
                RotateShip(i + Mathf.LerpAngle(0, flyAwayPitch, t), j + Mathf.LerpAngle(0, flyAwayYaw * (isModelFacingPlayer ? -1 : 1), t), k + Mathf.LerpAngle(0, flyAwayRoll, t)); //pitch/yaw/roll
                break;
            case (LeaveDirection.Up):
                RotateShip(i + Mathf.LerpAngle(0, flyAwayPitch, t), j, k); //pitch
                break;
            case (LeaveDirection.UpRight):
                RotateShip(i + Mathf.LerpAngle(0, flyAwayPitch, t), j + Mathf.LerpAngle(0, -flyAwayYaw * (isModelFacingPlayer ? -1 : 1), t), k + Mathf.LerpAngle(0, -flyAwayRoll, t)); //pitch/-yaw/-roll
                break;
            case (LeaveDirection.Left):
                RotateShip(i, j + Mathf.LerpAngle(0, flyAwayYaw * (isModelFacingPlayer?-1:1 ) * 1.5f, t), k); //0/yaw/roll
                break;
            case (LeaveDirection.Right):
                RotateShip(i, j + Mathf.LerpAngle(0, -flyAwayYaw * (isModelFacingPlayer ? -1 : 1) * 1.5f, t), k); //0/-yaw/-roll
                break;
        }
        transform.Translate(shipModel.forward * Time.fixedDeltaTime * leaveSpeed, Space.World);
    }
    void RotateShip(float x, float y, float z)
    {
        if (rotateWithMovement)
            shipModel.localEulerAngles = new Vector3(x, y, z);
        else if (lockedOntoPlayer)
        {
            shipModel.LookAt(PlayerInfo.singleton.transform.position);
        }
    }

    void CheckBounds(float currentValue)
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
