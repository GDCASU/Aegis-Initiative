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

    public enum FlyingInDirection
    {
        Towards = -1,
        Away = 1
    }

    [Header("Basic Variables")]
    [Tooltip("How fast the enemy ship moves when flying in")]
    [SerializeField]
    [Range(0.0f, 5.0f)]
    private float flyingInSpeed = 2;
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

    [Header("Flying In Variables")]
    [SerializeField]
    [Tooltip("Duration of the enemy flying in sequence")]
    [Range(0.0f, 25.0f)]
    float flyingInTime = 3;
    [Tooltip("Towards or Away from the player when flying in")]
    [SerializeField]
    private FlyingInDirection flyingInDirection = FlyingInDirection.Towards;

    [Header("Hover Variables")]
    [Tooltip("Does ship follow the player after flying in")]
    [SerializeField]
    private bool hover = true;
    [Tooltip("Duration that the enemy will follow the player for")]
    [SerializeField]
    [Range(0.0f, 10.0f)]
    private float hoverTimer = 3.0f; //how long ship stays

    [Header("Leave Variables")]
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

    private float flyAwayPitch;
    private float flyAwayYaw;
    private float flyAwayRoll;

    private float startY; //local start Y position
    private float startX; //local start X position

    private bool isFlyingAway;

    Transform shipModel;

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
        RotateShip(Mathf.LerpAngle(0, -pitch, t += Time.deltaTime), 0, 0); //up down tilting

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
            RotateShip(0,0 , Mathf.LerpAngle(0, -roll, t += Time.deltaTime)); //left right tilting
        }
        
        Move(roll * waveFrequency, 0); //move left/right
    }

    void Move(float x, float y)
    {
        if(!rotateWithMovement)RotateShip(0, 0, 0);

        if(flyingInTime > 0)
        {
            transform.Translate(new Vector3(x, y, flyingInSpeed * (int)flyingInDirection) * Time.deltaTime); //ship movement wave or no wave
            flyingInTime -= Time.deltaTime;
        }
        else
        {
            if(hoverTimer > 0 && hover)
            {
                transform.Translate(new Vector3(x, y, 0) * Time.deltaTime); //ship will hover
                hoverTimer -= Time.deltaTime;
            }
            else
            {
                if (!isFlyingAway)
                {
                    t = 0;
                    isFlyingAway = true;
                }
                FlyAway();
            }
        }
    }

    void FlyAway()
    {
        rotateWithMovement = true;
        t += Time.deltaTime;
        switch (leaveDirection)
        {
            //pitch yaw roll
            case (LeaveDirection.UpLeft):
                RotateShip(Mathf.LerpAngle(0, flyAwayPitch, t), Mathf.LerpAngle(0, flyAwayYaw, t), Mathf.LerpAngle(0, flyAwayRoll, t)); //pitch/yaw/roll
                break;
            case (LeaveDirection.Up):
                RotateShip(Mathf.LerpAngle(0, flyAwayPitch, t), 0, 0); //pitch
                break;
            case (LeaveDirection.UpRight):
                RotateShip(Mathf.LerpAngle(0, flyAwayPitch, t), Mathf.LerpAngle(0, -flyAwayYaw, t), Mathf.LerpAngle(0, -flyAwayRoll, t)); //pitch/-yaw/-roll
                break;
            case (LeaveDirection.Left):
                RotateShip(0, Mathf.LerpAngle(0, flyAwayYaw * 1.5f, t), 0); //0/yaw/roll
                break;
            case (LeaveDirection.Right):
                RotateShip(0, Mathf.LerpAngle(0, -flyAwayYaw * 1.5f, t), 0); //0/-yaw/-roll
                break;
        }
        transform.Translate(shipModel.forward * Time.deltaTime * leaveSpeed, Space.World);
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
