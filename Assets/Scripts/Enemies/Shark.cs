/* 
 * Handles the spiral, charge and rotate movements as well as the lifetime of Shark gameobject.
 * 
 * Author: Cristion Dominguez
 * Date: 29 Jan. 2021
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shark : MonoBehaviour
{
    [Header("Revolve")]

    [Tooltip("Speed at which Shark revolves around host")]
    [SerializeField]
    private float revolveSpeed = 300f;

    [Tooltip("Time Shark should revolve for before charging")]
    [SerializeField]
    private float revolveTime = 5f;

    [Header("Rotation to Player")]

    [Tooltip("Time after creation Shark should rotate towards Player")]
    [SerializeField]
    private float startRotationTime = 4f;

    [Tooltip("Speed at which the Shark rotate towards the Player")]
    [SerializeField]
    private float rotateSpeed = 4f;

    [Header("Charge")]

    [Tooltip("Speed at which Shark charges ahead")]
    [SerializeField]
    private float chargeSpeed = 10f;
    [Tooltip("Delay before the shark starts chargig to let the player realise whats happening")]
    [SerializeField]
    private float chargeDelay = 1;
    [Tooltip("Time Shark should charge until being destroyed")]
    [SerializeField]
    private float chargeTime = 2f;

    [Tooltip("Intensity to follow the Player's movement whilst charging")]
    [SerializeField]
    private float followIntensity = 0.3f;



    private Transform player;

    private int collisionDamage;  // damage dealt to Player upon collision

    private GameObject host;  // parent gameobject the Shark shall revolve around

    private Vector3 sharkAngles;  // angles of the Shark in World Space

    private bool isCharging = false;  // Is the Shark charging?

    private bool playerDetected = false;  // Has the Player been detected by the Shark?

    public Animator animator;

    /// <summary>
    /// Sets the host for the Shark to revolve around, collision damage dealt to Player, the Player transform, and starts Shark revolutions around its host.
    /// </summary>
    private void Start()
    {
        host = transform.parent.gameObject;
        collisionDamage = GetComponent<EnemyHealth>().collisionDamage;
        sharkAngles = transform.eulerAngles;

        if (PlayerInfo.singleton != null)
        {
            player = PlayerInfo.singleton.transform;
        }

        StartCoroutine(MoveToCenter());
    }

    /// <summary>
    /// For every frame, revolves Shark around host.
    /// </summary>
    private void Update()
    {
        transform.RotateAround(host.transform.position, host.transform.forward, revolveSpeed * Time.deltaTime);
        transform.eulerAngles = sharkAngles;
    }

    #region MovementCoroutines
    /// <summary>
    /// Moves Shark towards the center (host) at a calculated speed for revolveTime. At some point, rotates the Shark to the Player. Once revolveTime
    /// is reached, the Shark charges at Player.
    /// </summary>
    private IEnumerator MoveToCenter()
    {
        float elapsedTime = 0;
        float toCenterSpeed = transform.localPosition.magnitude / revolveTime;  // speed at which Shark shall move towards host
        bool isRotatingToPlayer = false;  // Is the Shark rotating to the Player?
        while (elapsedTime < revolveTime )
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, Vector3.zero, toCenterSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            
            if (elapsedTime >= startRotationTime && !isRotatingToPlayer)
            {
                StartCoroutine(RotateToPlayer());
                isRotatingToPlayer = true;
            }
            
            yield return null;
        }

        transform.localPosition = Vector3.zero;
        isCharging = true;
        StartCoroutine(Charge());
    }

    /// <summary>
    /// Rotates the Shark to the Player at rotateSpeed. If the Player is hit by a ray from the Shark, locks the Shark's orientation onto the Player. All of this
    /// ceases once the Shark charges.
    /// </summary>
    private IEnumerator RotateToPlayer()
    {
        if (player != null)
        {
            RaycastHit hit;

            while (!isCharging)
            {
                if (!playerDetected)
                {
                    if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit))
                        if (hit.transform.tag.Equals("Player"))
                        {
                            playerDetected = true;
                        }

                    sharkAngles = RotateToObject(player, rotateSpeed);
                }
                else
                    transform.LookAt(player);
                yield return null;
            }
        }
    }

    /// <summary>
    /// Disables the EnemyMovement script on host and moves host in the Shark's front-facing direction at chargeSpeed. Also rotates the Shark towards Player at followIntensity.
    /// Afterward, destroys the host gameobject.
    /// </summary>
    private IEnumerator Charge()
    {
        animator.SetBool("Attacking", true);
        float elapsedTime = 0;
        host.GetComponent<EnemyMovement>().enabled = false;
        while (chargeDelay > 0)
        {
            transform.LookAt(player);
            chargeDelay -= Time.deltaTime;
            yield return null;
        }
        transform.LookAt(player);
        while (elapsedTime < chargeTime)
        {
            sharkAngles = RotateToObject(player, followIntensity);

            host.transform.Translate(transform.forward * chargeSpeed * Time.deltaTime, Space.World);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        animator.SetBool("Attacking", true);
        Destroy(host);
    }
    #endregion

    /// <summary>
    /// If the Shark collides with Player, deals damage to the Player and destroys Shark parent.
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Player")
        {
            collision.transform.GetComponent<PlayerInfo>().TakeDamage(collisionDamage);
            Destroy(host);
        }
    }

    /// <summary>
    /// Returns a 3D Vector of euler angles for the Shark rotating towards a target on the next frame.
    /// </summary>
    /// <param name="target"> transform Shark should rotate to </param>
    /// <param name="degreeSpeed"> speed at which Shark rotates in degrees </param>
    /// <returns> 3D Vector of euler angles for next frame </returns>
    private Vector3 RotateToObject(Transform target, float degreeSpeed)
    {
        Vector3 targetDirection = target.position - transform.position;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, degreeSpeed * Time.deltaTime, 0);
        return Quaternion.LookRotation(newDirection).eulerAngles;
    }
}
