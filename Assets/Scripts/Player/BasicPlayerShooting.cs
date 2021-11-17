/*
 * Revision Author: Cristion Dominguez
 * Revision Date: 22 Jan. 2021
 * 
 * Modificiation: This class' fire rate variable is replaced with the fire rate variable from PlayerInfo script.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BasicPlayerShooting : MonoBehaviour
{
    public float speed;
    public int magSize;
    public GameObject bulletPrefab;
    private GameObject bullet;
    [SerializeField]
    private GameObject reticleReferencePoint;
    [SerializeField]
    private Image outerReticle;
    [SerializeField]
    private Image innerReticle;
    public Transform spawnR;
    public Transform spawnL;
    public bool alternate;

    private string Shoot = "event:/SFX/Combat/Shoot";
    private float timerOne;
    private float timerTwo;
    private GameObject closestEnemy;
    private Camera playerCam;

    public Text targetName;
    public Text aid;
    public Text wtf;

    private void Start()
    {
        timerOne = PlayerInfo.singleton.fireRate;
        FMODUnity.RuntimeManager.LoadBank("Combat");
        playerCam = GameObject.Find("Player Camera").GetComponent<Camera>();
    }
    private void Update()
    {
        if (closestEnemy != null)
        {
            if (closestEnemy.GetComponentInParent<EnemyMovement>() != null)
            {
                if (closestEnemy.GetComponentInParent<EnemyMovement>().isFlyingAway)
                {
                    WhiteReticle();
                    closestEnemy = null;
                    targetName.text = "none";
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        { 
            Time.timeScale = (Time.timeScale!=.1f)?1f:.1f;
        }
        if (closestEnemy != null)
        {
            wtf.text = closestEnemy.name + " wtf"; 
        }
        else wtf.text = "idk wtf";
        if (InputManager.GetButton(PlayerInput.PlayerButton.Shoot) && Time.timeScale==1)
        {
            //Time.timeScale = 0;
            if (closestEnemy != null)
            {
                aid.text = closestEnemy.name;
            }
            else aid.text = "officially lost"; 
            if (alternate)
            {
                if (timerOne < 0)
                {
                    FMODUnity.RuntimeManager.PlayOneShot(Shoot, transform.position, GameManager.singleton.sfxVolume);
                    bullet = Instantiate(bulletPrefab, spawnR.position, spawnR.rotation);
                    bullet.GetComponent<Rigidbody>().velocity =  spawnR.forward.normalized * speed;
                    if (closestEnemy != null)
                    {
                        EnemyInRange();
                    }
                    timerOne = PlayerInfo.singleton.fireRate;
                    timerTwo = PlayerInfo.singleton.fireRate / 2f;
                }
                if (timerTwo < 0)
                {
                    FMODUnity.RuntimeManager.PlayOneShot(Shoot, transform.position, GameManager.singleton.sfxVolume);
                    bullet = Instantiate(bulletPrefab, spawnL.position, spawnL.rotation);
                    bullet.GetComponent<Rigidbody>().velocity = spawnL.forward.normalized * speed;
                    if (closestEnemy != null)
                    {
                        EnemyInRange();
                    }
                    timerTwo = PlayerInfo.singleton.fireRate;
                }
                timerTwo -= Time.deltaTime;
            }
            else 
            {
                if (timerOne < 0)
                {
                    //FMODUnity.RuntimeManager.PlayOneShot(Shoot, transform.position, GameManager.singleton.sfxVolume);
                    bullet = Instantiate(bulletPrefab, spawnR.position, spawnR.rotation);
                    bullet.GetComponent<Rigidbody>().velocity = spawnR.forward.normalized * speed;
                    if(closestEnemy != null)
                    {
                        EnemyInRange();
                    }
                    bullet = Instantiate(bulletPrefab, spawnL.position, spawnL.rotation);
                    bullet.GetComponent<Rigidbody>().velocity = spawnL.forward.normalized * speed;
                    if (closestEnemy != null)
                    {
                        EnemyInRange();
                    }
                    timerOne = PlayerInfo.singleton.fireRate;
                }
            }          
        }
        timerOne -= Time.deltaTime;
    }

    public void AimAssist(GameObject enemy)
    {
        if(enemy.GetComponentInParent<EnemyMovement>() != null)
        {
            if (!enemy.GetComponentInParent<EnemyMovement>().isFlyingAway)
            {
                SetClosestEnemy(enemy);
                //wtf.text = closestEnemy? closestEnemy.name:"after1";
            }
        }
        else
        {
            SetClosestEnemy(enemy);
            //wtf.text = closestEnemy ? closestEnemy.name : "after2";
        }
    }

    private void SetClosestEnemy(GameObject enemy)
    {
        Vector3 enemyConversion = playerCam.WorldToViewportPoint(enemy.transform.position);
        //if (closestEnemy != null && closestEnemy == enemy && enemyConversion.z < 0)
        //{
        //    WhiteReticle();
        //    closestEnemy = null;
        //    targetName.text = "im fucking it up 1";
        //    return;      //Exits if the enemy is behind the player
        //}
        //aid.text = "" + enemyConversion.z;
        Vector2 reticleConversion = playerCam.WorldToViewportPoint(reticleReferencePoint.transform.position);

        var distance = Vector2.Distance(enemyConversion, reticleConversion);
        //enemy is in AimAssist range
        if (distance < PlayerInfo.singleton.aimAssistStrength)
        {
            if (closestEnemy != null)
            {
                //enemy is closer than current closestEnemy
                if (distance < Vector2.Distance(playerCam.WorldToViewportPoint(closestEnemy.transform.position), reticleConversion))
                {
                    RedReticle();
                    closestEnemy = enemy;
                    targetName.text = enemy.name;
                }
            }
            else
            {
                RedReticle();
                closestEnemy = enemy;
                targetName.text = enemy.name;
            }
        }
        else
        {
            //enemy is out of AimAssist range
            if (closestEnemy == enemy)
            {
                WhiteReticle();
                closestEnemy = null;
                targetName.text = "im fucking it up 2";
            }
        }
    }

    private void EnemyInRange()
    {
        aid.text = "called";
        bullet.GetComponent<Bullet>().LockOn(closestEnemy);
    }

    private void RedReticle()
    {
        outerReticle.color = Color.red;
        innerReticle.color = Color.red;
    }

    private void WhiteReticle()
    {
        outerReticle.color = Color.white;
        innerReticle.color = Color.white;
    }

    public void ResetEnemy(GameObject enemy)
    {
        if (closestEnemy == enemy)
        {
            WhiteReticle();
            closestEnemy = null;
            targetName.text = "im fucking it up 2";
        }
    }
}
