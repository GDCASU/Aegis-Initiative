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
    public GameObject closestEnemy;
    private Camera playerCam;

    private void Start()
    {
        timerOne = PlayerInfo.singleton.fireRate;
        FMODUnity.RuntimeManager.LoadBank("Combat");
        playerCam = GameObject.Find("Player Camera").GetComponent<Camera>();
    }
    private void Update()
    {
        if(closestEnemy != null)
            if (closestEnemy.GetComponentInParent<EnemyMovement>().isFlyingAway)
                closestEnemy = null;

        if (InputManager.GetButton(PlayerInput.PlayerButton.Shoot))
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
        Vector2 enemyConversion = playerCam.WorldToViewportPoint(enemy.transform.position);
        Vector2 reticleConversion = playerCam.WorldToViewportPoint(reticle.transform.position);

        if(enemy.GetComponentInParent<EnemyMovement>() != null)
        {
            if (!enemy.GetComponentInParent<EnemyMovement>().isFlyingAway)
            {
                var distance = Vector2.Distance(enemyConversion, reticleConversion);
                if (distance < PlayerInfo.singleton.aimAssistStrength)
                {
                    if (closestEnemy != null)
                    {
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
                    if (closestEnemy == enemy)
                        closestEnemy = null;
                }
            }
        }            
    }

    private void EnemyInRange()
    {
        bullet.GetComponent<Bullet>().LockOn(closestEnemy);
    }
}
