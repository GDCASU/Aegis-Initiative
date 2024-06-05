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
    public Animation bossAnimation;
    private bool moveHead;

    private enum LaserDirection
    {
        Horizontal,
        Vertical
    }

    void Start()
    {
        //set array of formation arrays
        formations[0] = formation1;
        formations[1] = formation2;
        formations[2] = formation3;

        //start events
        StartCoroutine(SpawningDelay());
        bossAnimation.Play("AsteroidBossAnimation");
    }
    
    //damage
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet") TakeDamage(collision.gameObject.GetComponent<Bullet>().damage);
    }

    public override void DestroyEnemy()
    {
        PlayerInfo.singleton.GetComponent<StageTriggers>().EndLevel();
        base.DestroyEnemy();
    }

    /// <summary>
    /// Spawns laser bots in randomized formation.
    /// </summary>
    IEnumerator Event1()
    {
        //use RNG to choose laser bot formation
        int formationIndex = Random.Range(0,3);
        Transform[] formation = formations[formationIndex];

        //instantiate each laser bot
        foreach (Transform t in formation)
        {
            GameObject bot = Instantiate(laserBot, t.position, Quaternion.identity, transform);
            if (PlayerInfo.singleton) bot.transform.forward = bot.transform.position - PlayerInfo.singleton.transform.position;
            yield return new WaitForSeconds(0.5f);
        }

        //pause before next event
        yield return new WaitForSeconds(3f);
        StartCoroutine("Event2");
    }

    IEnumerator SpawningDelay()
    {
        float timer = 3.2f;
        float screenShake = .5f;
        float subTimer = 0;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            subTimer -= Time.deltaTime;
            if (subTimer <= 0)
            {
                PlayerInfo.singleton?.GetComponent<ScreenShake>()?.ShakeCamera();
                subTimer = screenShake;
            }
            yield return null;
        }
        StartCoroutine("Event2");
    }

    /// <summary>
    /// Shoots asteroids at player from vacuums.
    /// </summary>
    IEnumerator Event2()
    {
        //shoot eight shots
        for (int shot = 0; shot < 8; shot++)
        {
            //alternate between left and right vacuums
            Vector3 vacuum = (shot % 2 == 0) ? vacuum1.position : vacuum2.position;

            //shoot 6 asteroids
            for (int asteroid = 0; asteroid < 6; asteroid++)
            {
                int asteroidIndex = 0;
                Vector3 player = PlayerInfo.singleton.transform.position;

                //randomize the x, y, and z
                for (int i = 0; i < 3; i++)
                {
                    //shoot asteroid in area around the player
                    asteroidIndex = Random.Range(0, 4);
                    int posNeg = Random.Range(0, 2) * 2 - 1;
                    player[i] += posNeg * asteroidIndex;
                }

                //spawn asteroid then make them smaller and move towards player
                GameObject asteroidObj = Instantiate(asteroids[asteroidIndex], vacuum, Quaternion.identity, transform);
                asteroidObj.transform.localScale *= 0.25f;
                asteroidObj.GetComponent<Rigidbody>().AddForce((player - asteroidObj.transform.position) * 25f);
                yield return new WaitForSeconds(0.5f/6);
            }
            if (shot != 7) yield return new WaitForSeconds(1f);
        }
        yield return new WaitForSeconds(1);
        StartCoroutine("Event3");
    }

    /// <summary>
    /// Sweeps laser beam at player vertically and horizontally.
    /// </summary>
    IEnumerator Event3()
    {
        //"charge up"
        yield return new WaitForSeconds(1);

        //
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
