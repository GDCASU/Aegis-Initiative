/*
 * Revision Author: Cristion Dominguez
 * Revision Date: 22 Jan. 2021
 * 
 * Modificiation: This class' fire rate variable is replaced with the fire rate variable from PlayerInfo script.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicPlayerShooting : MonoBehaviour
{
    public float speed;
    public int magSize;
    public GameObject bulletPrefab;
    private GameObject bullet;
    public GameObject reticle;
    public Transform spawnR;
    public Transform spawnL;
    public bool alternate;

    private string Shoot = "event:/SFX/Combat/Shoot";
    private float timerOne;
    private float timerTwo;
    private GameObject closestEnemy;
    private Camera playerCam;

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
            if(closestEnemy.GetComponentInParent<EnemyMovement>() != null)
            {
                if (closestEnemy.GetComponentInParent<EnemyMovement>().isFlyingAway)
                    closestEnemy = null;
            }
        }            

        if (InputManager.GetButton(PlayerInput.PlayerButton.Shoot) && Time.timeScale==1)
        {
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
                    FMODUnity.RuntimeManager.PlayOneShot(Shoot, transform.position, GameManager.singleton.sfxVolume);
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
            }
        }
        else
        {
            SetClosestEnemy(enemy);
        }
    }

    private void SetClosestEnemy(GameObject enemy)
    {
        Vector3 enemyConversion = playerCam.WorldToViewportPoint(enemy.transform.position);
        if (enemyConversion.z < 0)
        {
            closestEnemy = null;
            return;      //Exits if the enemy is behind of the player
        }
        Vector2 reticleConversion = playerCam.WorldToViewportPoint(reticle.transform.position);

        var distance = Vector2.Distance(enemyConversion, reticleConversion);
        //enemy is in AimAssist range
        if (distance < PlayerInfo.singleton.aimAssistStrength)
        {
            if (closestEnemy != null)
            {
                //enemy is closer than current closestEnemy
                if (distance < Vector2.Distance(closestEnemy.transform.position, reticleConversion))
                {
                    closestEnemy = enemy;
                }
            }
            else
            {
                closestEnemy = enemy;
            }
        }
        else
        {
            //enemy is out of AimAssist range
            if (closestEnemy == enemy)
                closestEnemy = null;
        }
    }

    private void EnemyInRange()
    {
        bullet.GetComponent<Bullet>().LockOn(closestEnemy);
    }
}
