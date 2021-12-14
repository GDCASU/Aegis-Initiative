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
    public GameObject closestEnemy;
    private Camera playerCam;

    [SerializeField]
    private GameObject shipModel;
    
    private void Start()
    {
        timerOne = PlayerInfo.singleton.fireRate;
        FMODUnity.RuntimeManager.LoadBank("Combat");
        playerCam = GameObject.Find("Player Camera").GetComponent<Camera>();
    }
    private void Update()
    {
        RaycastHit hit;
        float verticalSize = 0.3f;
        Vector2 reticlePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)outerReticle.rectTransform.parent, new Vector2(outerReticle.rectTransform.anchoredPosition.x, outerReticle.rectTransform.anchoredPosition.y), Camera.main, out reticlePos);
        Debug.Log(reticlePos);

        if (Physics.CapsuleCast(reticlePos, reticlePos, 0.2f, Camera.main.transform.TransformDirection(Vector3.forward), out hit))
        {
            RaycastHit[] hits = Physics.CapsuleCastAll(reticlePos, reticlePos, 0.2f, Camera.main.transform.TransformDirection(Vector3.forward));
            foreach (var obj in hits)
            {
                if (obj.transform.CompareTag("Enemy"))
                {
                    RedReticle();
                    if (closestEnemy != null)
                    {
                        if (obj.transform.localPosition.z < closestEnemy.transform.localPosition.z)
                        {
                            closestEnemy = obj.transform.gameObject;
                        }
                    }
                    else
                    {
                        closestEnemy = obj.transform.gameObject;
                    }
                }
            }
            Debug.DrawRay(reticlePos, Camera.main.transform.TransformDirection(Vector3.forward) * hits[0].distance, Color.yellow);
            Debug.Log("Did hit");
        }
        else
        {
            WhiteReticle();
            Debug.DrawRay(reticlePos, Camera.main.transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            Debug.Log("Did not Hit");
        }

        //if (Physics.CapsuleCast(shipModel.transform.position, shipModel.transform.position, 0.2f, Camera.main.transform.TransformDirection(Vector3.forward), out hit))
        //{
        //    RaycastHit[] hits = Physics.CapsuleCastAll(shipModel.transform.position, shipModel.transform.position, 0.2f, Camera.main.transform.TransformDirection(Vector3.forward));
        //    foreach(var obj in hits)
        //    {
        //        if(obj.transform.CompareTag("Enemy"))
        //        {
        //            RedReticle();
        //            if(closestEnemy != null)
        //            {
        //                if(obj.transform.localPosition.z < closestEnemy.transform.localPosition.z)
        //                {
        //                    closestEnemy = obj.transform.gameObject;
        //                }
        //            }
        //            else
        //            {
        //                closestEnemy = obj.transform.gameObject;
        //            }
        //        }
        //    }
        //    Debug.DrawRay(shipModel.transform.position, Camera.main.transform.TransformDirection(Vector3.forward) * hits[0].distance, Color.yellow);
        //    Debug.Log("Did hit");
        //}
        //else
        //{
        //    WhiteReticle();
        //    Debug.DrawRay(shipModel.transform.position, Camera.main.transform.TransformDirection(Vector3.forward) * 1000, Color.white);
        //    Debug.Log("Did not Hit");
        //}

        if (closestEnemy != null)
        {
            if (closestEnemy.GetComponentInParent<EnemyMovement>() != null)
            {
                if (closestEnemy.GetComponentInParent<EnemyMovement>().isFlyingAway)
                {
                    WhiteReticle();
                    closestEnemy = null;
                }
            }
            if (closestEnemy.transform.localPosition.z < 0)
                closestEnemy = null;
        }
        if (InputManager.GetButton(PlayerInput.PlayerButton.Shoot) && Time.timeScale == 1)
        {
            if (alternate)
            {
                if (timerOne < 0)
                {
                    FMODUnity.RuntimeManager.PlayOneShot(Shoot, transform.position, GameManager.singleton.sfxVolume);
                    bullet = Instantiate(bulletPrefab, spawnR.position, spawnR.rotation);
                    bullet.GetComponent<Rigidbody>().velocity = spawnR.forward.normalized * speed;
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
                    if (closestEnemy != null)
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
        if (enemy.GetComponentInParent<EnemyMovement>() != null)
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
        //Vector3 enemyConversion = playerCam.WorldToViewportPoint(enemy.transform.position);
        //Vector2 reticleConversion = playerCam.WorldToViewportPoint(reticleReferencePoint.transform.position);

        //var distance = Vector2.Distance(enemyConversion, reticleConversion);
        ////enemy is in AimAssist range
        //if (distance < PlayerInfo.singleton.aimAssistStrength)
        //{
        //    if (closestEnemy != null)
        //    {
        //        //enemy is closer than current closestEnemy
        //        if (distance < Vector2.Distance(playerCam.WorldToViewportPoint(closestEnemy.transform.position), reticleConversion))
        //        {
        //            RedReticle();
        //            closestEnemy = enemy;
        //        }
        //    }
        //    else
        //    {
        //        RedReticle();
        //        closestEnemy = enemy;
        //    }
        //}
        //else
        //{
        //    //enemy is out of AimAssist range
        //    if (closestEnemy == enemy)
        //    {
        //        WhiteReticle();
        //        closestEnemy = null;
        //    }
        //}
    }

    private void EnemyInRange()
    {
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
        }
    }
}