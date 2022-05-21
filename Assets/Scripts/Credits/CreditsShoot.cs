﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsShoot : MonoBehaviour
{
    public bool ControlWithMouse = true;
    public bool ReticleMovesBack = false;
    public Transform Reticle;
    public Transform LaserSpawn1;
    public Transform LaserSpawn2;
    public GameObject LaserParent;
    public float MoveSpeed = 30f;
    public float MoveBackSpeed = 10f;
    public Canvas canvas;
    private string shootSFX = "event:/SFX/Combat/Shoot";
    private Ray ray;
    private bool isShooting = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveReticle();
        if (InputManager.GetButtonDown(PlayerInput.PlayerButton.Shoot) && !isShooting) Shoot();
    }

    private void MoveReticle()
    {
        Reticle.Rotate(Vector3.forward, Time.deltaTime * 10);

        if (ControlWithMouse)
        {
            Reticle.position = Input.mousePosition;
        }
        else
        {
            float horizontalDirection = Input.GetAxis("Horizontal");
            float verticalDirection = Input.GetAxis("Vertical");

            Vector2 newPosition = new Vector2(horizontalDirection * Time.deltaTime * MoveSpeed, verticalDirection * Time.deltaTime * MoveSpeed);

            if (Mathf.Abs(Reticle.GetComponent<RectTransform>().anchoredPosition.x + newPosition.x) < canvas.GetComponent<RectTransform>().rect.width / 2)
            {
                Reticle.GetComponent<RectTransform>().anchoredPosition += new Vector2(newPosition.x, 0);
            }

            if (Mathf.Abs(Reticle.GetComponent<RectTransform>().anchoredPosition.y + +newPosition.y) < canvas.GetComponent<RectTransform>().rect.height / 2)
            {
                Reticle.GetComponent<RectTransform>().anchoredPosition += new Vector2(0, newPosition.y);
            }

            if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W))
                MoveReticleBack();
        }
    }

    private void MoveReticleBack()
    {
        if (ReticleMovesBack) Reticle.GetComponent<RectTransform>().anchoredPosition = Vector3.MoveTowards(Reticle.GetComponent<RectTransform>().anchoredPosition, Vector3.zero, Time.deltaTime * MoveBackSpeed);
    }

    private void Shoot()
    {
        StartCoroutine(ShootAnim(20));
        
        ray = Camera.main.ScreenPointToRay(Reticle.position);
        Vector3 direction = ray.direction * 300;

        if (Physics.Raycast(ray, out RaycastHit hit, 300))
        {
            direction = hit.point;
            hit.transform.GetComponentInParent<CreditsNameMovement>()?.Shot();
        }

        ShootLaserPrefabs(direction);
    }

    private void ShootLaserPrefabs(Vector3 position)
    {
        FMODUnity.RuntimeManager.PlayOneShot(shootSFX, transform.position, GameManager.singleton.sfxVolume);
        GameObject laser = Instantiate(LaserParent, LaserSpawn1.position, LaserSpawn1.rotation);
        laser.GetComponent<CreditsLaser>().Direction = position;
        laser = Instantiate(LaserParent, LaserSpawn2.position, LaserSpawn2.rotation);
        laser.GetComponent<CreditsLaser>().Direction = position;
    }

    private IEnumerator ShootAnim(int steps)
    {
        if (!isShooting)
        {
            isShooting = true;
            RectTransform reticleRectTransform = Reticle.GetComponent<RectTransform>();
            reticleRectTransform.localScale = Vector3.one;

            for (int j = 0; j < 2; j++)
            {
                for (int i = 0; i < steps; i++)
                {
                    if (i < steps / 2) reticleRectTransform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 2, (float)i / 10);
                    if (i >= steps / 2) reticleRectTransform.localScale = Vector3.Lerp(Vector3.one * 2, Vector3.one, (float)(i - 10) / 10);
                    yield return new WaitForSeconds(.005f);
                }
            }

            isShooting = false;
        }
    }
}