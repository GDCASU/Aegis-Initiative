/*
 * Revision Author: Cristion Dominguez
 * Revision Date: 15 March 2021
 * 
 * Modification: Added another constant in enum spawnLocation called "AtCurrentPosition" which notifies the EnemySpawner to just spawn/activate the associated
 * enemy gameobject in its current position under its current parent.
 */

using Fungus;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public enum spawnLocation
    {
        TopLeft,
        TopMiddle,
        TopRight,
        MiddleLeft,
        Middle,
        MiddleRight,
        BottomLeft,
        BottomRight,
        AtCurrentPosition
    }

    [Serializable]
    public struct enemies //struct for which enemies to spawn
    {
        public spawnLocation SpawnLocation;
        public GameObject enemy; //enemy in the scene
        [Tooltip("-1 is the middle of the screen and 1 is the edge")]
        [Range(-25f, 25f)]
        public float adjustWidth;
        [Tooltip("-1 is the middle of the screen and 1 is the edge")]
        [Range(-25f, 25f)]
        public float adjustHeight;
        [Tooltip("1 is on the reticle, 5 is furthest")]
        [Range(-25f, 25f)]
        public float adjustDepth;
    }

    [SerializeField]
    public enemies[] _enemies; //array of enemy struct

    private float xSpawnAdditive = 1.25f;
    private float ySpawnAdditive = 0.75f;
    private float xSpawnAdjustment;
    private float ySpawnAdjustment;
    private float zSpawnAdjustment;
    private Vector3 spawnAdjustment;
    private Vector3 playerScale;

    private void Start()
    {
        //zSpawnAdjustment = 2.0f;
        playerScale = PlayerInfo.singleton.transform.parent.localScale;
    }

    private void OnTriggerEnter(Collider other)
    {
        //print(other.name);
        if (other.gameObject.tag == "Player")
        {
            SpawnEnemy();
        }
    }

    public void SpawnEnemy()
    {
        for (int x = 0; x < _enemies.Length; x++) //loop through enemies to spawn
        {
            xSpawnAdjustment = 0;
            ySpawnAdjustment = 0;
            zSpawnAdjustment = 0;
            if (_enemies[x].SpawnLocation != spawnLocation.AtCurrentPosition)
            {
                xSpawnAdjustment += xSpawnAdditive * _enemies[x].adjustWidth * playerScale.x;
                ySpawnAdjustment += ySpawnAdditive * _enemies[x].adjustHeight * playerScale.y;
                zSpawnAdjustment = _enemies[x].adjustDepth * playerScale.z;
                switch (_enemies[x].SpawnLocation) //chance x and/or y adjustment based on selected location
                {
                    case spawnLocation.TopLeft:
                        xSpawnAdjustment += -xSpawnAdditive;
                        ySpawnAdjustment += ySpawnAdditive;
                        break;
                    case spawnLocation.TopMiddle:
                        xSpawnAdjustment = 0.0f;
                        ySpawnAdjustment += ySpawnAdditive;
                        break;
                    case spawnLocation.MiddleLeft:
                        xSpawnAdjustment += -xSpawnAdditive;
                        ySpawnAdjustment = 0.0f;
                        break;
                    case spawnLocation.MiddleRight:
                        xSpawnAdjustment += xSpawnAdditive;
                        ySpawnAdjustment = 0.0f;
                        break;
                    case spawnLocation.BottomLeft:
                        xSpawnAdjustment += -xSpawnAdditive;
                        ySpawnAdjustment += -ySpawnAdditive;
                        break;
                    case spawnLocation.BottomRight:
                        xSpawnAdjustment += xSpawnAdditive;
                        ySpawnAdjustment += -ySpawnAdditive;
                        break;
                    case spawnLocation.Middle:
                        break;
                }

                spawnAdjustment = new Vector3(xSpawnAdjustment, ySpawnAdjustment, zSpawnAdjustment); //spawn adjustment vector to add to other.gameObject.transform.position
                _enemies[x].enemy.transform.parent = PlayerInfo.singleton.gameObject.transform.parent ?? transform;
                _enemies[x].enemy.transform.localPosition = spawnAdjustment;
                _enemies[x].enemy.transform.localRotation = Quaternion.identity;
            }

            _enemies[x].enemy.SetActive(true);
        }
    }
}
