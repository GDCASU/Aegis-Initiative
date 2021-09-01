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

    private void Start()
    {
        timerOne = PlayerInfo.singleton.fireRate;
        FMODUnity.RuntimeManager.LoadBank("Combat");
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
        Vector2 enemyConversion = Camera.main.WorldToViewportPoint(enemy.transform.position);
        Vector2 reticleConversion = Camera.main.WorldToViewportPoint(reticle.transform.position);

        if (!enemy.GetComponentInParent<EnemyMovement>().isFlyingAway)
        {
            Debug.Log("Enemy: " + enemyConversion);
            Debug.Log("Reticle: " + reticleConversion);
            var temp = Vector2.Distance(enemyConversion, reticleConversion);
            Debug.Log("Distance: " + temp);
            if (Vector2.Distance(enemyConversion, reticleConversion) < PlayerInfo.singleton.aimAssistStrength)
            {
                //Debug.Log("ENEMY DETECTED");
                if (closestEnemy != null)
                {
                    if (Vector2.Distance(enemyConversion, reticleConversion) < Vector2.Distance(closestEnemy.transform.position, reticleConversion))
                    {
                        closestEnemy = enemy;
                    }
                }
                else
                {
                    closestEnemy = enemy;
                }
            }
        }
            
    }

    private void EnemyInRange()
    {
        bullet.GetComponent<Bullet>().LockOn(closestEnemy);
    }
}
