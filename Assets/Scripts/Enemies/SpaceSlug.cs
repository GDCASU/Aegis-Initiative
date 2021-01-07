using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SpaceSlug : MonoBehaviour
{
    public int damage; //damage dealt to Player health
    // public Transform[] spaceSlugBody;


    public CinemachineSmoothPath path = default;
    public CinemachineSmoothPath playerPath;
    CinemachineDollyCart playerCart;
    public CinemachineDollyCart slugCart;
    System.Random rng;
    Vector3 start;
    Vector3 middle;
    Vector3 end;

    public float timeAhead;
    public float height;
    public float speed;
    public int widthRange;

    // Start is called before the first frame update
    void Start()
    {
        // lineRenderer.positionCount = numPoints;   
        // DrawCubicCurve();
        playerCart = PlayerHealth.singleton.GetComponentInParent<CinemachineDollyCart>();
        rng = new System.Random();
        start = new Vector3();
        middle = new Vector3();
        end = new Vector3();
        SlugTest();
    }

    // Update is called once per frame
    void Update()
    {   
        if (slugCart.transform.position == end)
        {
            SlugTest();
        }
    }

    private void SlugTest()
    {
        int side = rng.Next(2); //0 = left, 1 = right
        // Vector3 start = new Vector3();
        // Vector3 middle = new Vector3();
        // Vector3 end = new Vector3();
        middle = playerPath.EvaluatePositionAtUnit(playerCart.m_Position + playerCart.m_Speed * timeAhead, playerCart.m_PositionUnits + (int)(playerCart.m_Speed * timeAhead)) + Vector3.up * height;
        if (side == 0)
        {
            start = middle - 2 * Vector3.up * height - playerCart.transform.right.normalized * rng.Next(widthRange);
            end = middle - 2 * Vector3.up * height + playerCart.transform.right.normalized * rng.Next(widthRange);
        }
        else
        {
            start = middle -  2 * Vector3.up * height + playerCart.transform.right.normalized * rng.Next(widthRange);
            end = middle -  2 * Vector3.up * height - playerCart.transform.right.normalized * rng.Next(widthRange);
        }

        path.m_Waypoints[0].position = start;
        path.m_Waypoints[1].position = middle;
        path.m_Waypoints[2].position = end;

        path.InvalidateDistanceCache();
        slugCart.m_Position = 0;

        //speed
        slugCart.m_Speed = speed;

    }

    //If player collides with SpaceSlug, Player takes damage
    void OnCollision(GameObject other)
    {
        if (other.name == "Player")
        {
            //call Player's TakeDamage() method
            other.GetComponent<PlayerHealth>().TakeDamage(damage);
        }
    }
}
