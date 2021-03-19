using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidBoss : EnemyHealth
{
    public GameObject laserBot;
    public Transform[] formation1;
    public Transform[] formation2;
    public Transform[] formation3;
    public Transform vacuum1;
    public Transform vacuum2;
    public GameObject[] asteroids;
    private Transform[][] formations = new Transform[3][];
    public GameObject laser;
    public GameObject bigLaser;
    public Transform head;
    public float bigLaserRotationSpeed;
    private bool moveHead;
    public Animation bossAnimation;

    private enum LaserDirection
    {
        Horizontal,
        Vertical
    }

    // Start is called before the first frame update
    void Start()
    {
        formations[0] = formation1;
        formations[1] = formation2;
        formations[2] = formation3;
        StartCoroutine("Event1");
        bossAnimation.Play("AsteroidBossAnimation");
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet") TakeDamage(collision.gameObject.GetComponent<Bullet>().damage);
    }

    IEnumerator Event1()
    {
        int formationIndex = Random.Range(0,3);
        Transform[] formation = formations[formationIndex];
        foreach (Transform t in formation)
        {
            Instantiate(laserBot, t.position, Quaternion.identity, transform);
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(3f);
        StartCoroutine("Event2");
    }

    IEnumerator Event2()
    {
        for (int shot = 0; shot < 8; shot++)
        {
            Vector3 vacuum = (shot % 2 == 0) ? vacuum1.position : vacuum2.position;
            for (int asteroid = 0; asteroid < 6; asteroid++)
            {
                int asteroidIndex = 0;
                Vector3 player = PlayerInfo.singleton.transform.position;
                for (int i = 0; i < 3; i++)
                {
                    asteroidIndex = Random.Range(0, 4);
                    int posNeg = Random.Range(0, 1) * 2 - 1;
                    player[i] += posNeg * asteroidIndex;
                }
                GameObject asteroidObj = Instantiate(asteroids[asteroidIndex], vacuum, Quaternion.identity, transform);
                asteroidObj.GetComponent<Asteroid>().enabled = false;
                asteroidObj.AddComponent<AsteroidBossDespawn>();
                asteroidObj.transform.localScale *= 0.25f;
                asteroidObj.GetComponent<Rigidbody>().AddForce((player - asteroidObj.transform.position) * 20f);
                yield return new WaitForSeconds(0.5f/6);
            }
            yield return new WaitForSeconds(1f);
        }
        yield return new WaitForSeconds(3);
        StartCoroutine("Event3");
    }

    IEnumerator Event3()
    {
        //"charge up"
        yield return new WaitForSeconds(1);

        for (int i = 0; i < 5; i++)
        {
            //shoot laser
            Instantiate(laser, head.position, Quaternion.identity, transform)
                .GetComponent<Rigidbody>()
                .AddForce((PlayerInfo.singleton.transform.position - head.position) * 100f);
            yield return new WaitForSeconds(0.5f);
        }

        bossAnimation["AsteroidBossAnimation"].wrapMode = WrapMode.Once;

        foreach (LaserDirection direction in System.Enum.GetValues(typeof(LaserDirection)))
        {
            yield return new WaitForSeconds(1);
            if (direction == LaserDirection.Horizontal)
                head.transform.localEulerAngles = new Vector3(head.transform.localEulerAngles.x, 5f, head.transform.localEulerAngles.z);
            if (direction == LaserDirection.Vertical)
                head.transform.localEulerAngles = new Vector3(5f, head.transform.localEulerAngles.y, head.transform.localEulerAngles.z);
            
            bigLaser.SetActive(true);
            moveHead = true;

            while (moveHead)
            {
                if (direction == LaserDirection.Horizontal)
                {
                    head.transform.Rotate(Vector3.down * (bigLaserRotationSpeed * Time.deltaTime));
                    if (head.transform.localEulerAngles.y > 5f && head.transform.localEulerAngles.y < 355f)
                        moveHead = false;
                }
                if (direction == LaserDirection.Vertical)
                {
                    head.transform.Rotate(Vector3.left * (bigLaserRotationSpeed * Time.deltaTime));
                    if (head.transform.localEulerAngles.x > 5f && head.transform.localEulerAngles.x < 355f)
                        moveHead = false;
                }
                yield return new WaitForEndOfFrame();
            }
            head.transform.localEulerAngles = new Vector3(0f, 0f, head.transform.localEulerAngles.z);
            bigLaser.SetActive(false);
        }

        bossAnimation["AsteroidBossAnimation"].wrapMode = WrapMode.Loop;
        bossAnimation.Play("AsteroidBossAnimation");

        yield return new WaitForSeconds(2);
        StartCoroutine("Event1");
    }
}
