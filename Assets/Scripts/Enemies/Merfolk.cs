using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merfolk : EnemyHealth
{
    [Space]

    public GameObject bulletPrefab;
    public Transform bulletSpawn;

    public float shootSpeed = 100f;
    public float speed = 5f;
    public float figure8Radius = 10f;
    [Tooltip("Starts shooting after how many seconds")]
    public float startShooting = 5.0f;
    [Tooltip("How much it slows down when not in water")]
    public float waterMultiplier = 0.8f;
    public bool aboveWater;
    private float belowSpeed;
    private float aboveSpeed;

    private Vector3 center;
    private float angle;
    private int iteration = 1;
    private int direction = 1;
    private Vector3 moveCenter;

    public enum figureEight
    {
        vertical,
        horizontal
    };

    public figureEight figureEightDirection;

    void Start()
    {
        //set the speed according to whether it's above or below water
        aboveSpeed = speed * waterMultiplier;
        belowSpeed = speed / waterMultiplier;
        speed = (aboveWater) ? aboveSpeed : belowSpeed;

        //set the moving and starting centers
        if (figureEightDirection == figureEight.horizontal)
        {
            moveCenter = new Vector3(0, 0, figure8Radius * 2);
            center = transform.localPosition - new Vector3(0, 0, figure8Radius);
        }
        else
        {
            moveCenter = new Vector3(0, figure8Radius * 2, 0);
            center = transform.localPosition - new Vector3(0, figure8Radius, 0);
        }

        //start shooting
        InvokeRepeating("Shoot", startShooting, 5f);
    }

    void Update()
    {
        Figure8();
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, transform.rotation);
        bullet.GetComponent<Rigidbody>().velocity = (PlayerInfo.singleton.transform.position - bullet.transform.position).normalized * shootSpeed;
    }

    void Figure8()
    {
        //if completed one circle of the figure 8, go to the other circle
        if (angle > iteration * 2 * Mathf.PI)
        {
            center = (iteration % 2 == 0) ? center - moveCenter : center + moveCenter;
            direction = (iteration % 2 == 0) ? 1 : -1;
            iteration++;
        }

        //circular motion
        angle += speed * Time.deltaTime;

        Vector3 offset;
        if (figureEightDirection == figureEight.horizontal)
            offset = new Vector3(Mathf.Sin(angle * direction), 0, Mathf.Cos(angle * direction)) * figure8Radius * direction;
        else
            offset = new Vector3(Mathf.Sin(angle * direction), Mathf.Cos(angle * direction), 0) * figure8Radius * direction;

        transform.localPosition = center + offset;
    }
}
