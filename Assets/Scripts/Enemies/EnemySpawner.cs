﻿using Fungus;
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
        MiddleRight,
        BottomLeft,
        BottomRight
    }

    [Serializable]
    public struct enemies //struct for which enemies to spawn
    {
        public spawnLocation SpawnLocation;
        public GameObject enemyPrefab; //enemy prefab
        [Tooltip("-1 is the middle of the screen and 1 is the edge")]
        [Range(-1.0f, 1.0f)]
        public float adjustPlaneLocation; //spawn ships closer or farther from player on 2D plane
        [Tooltip("1 is on the reticle, 5 is furthest")]
        [Range(1.0f, 5.0f)]
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
    public Transform player;
    private Vector3 playerScale;

    private void Start()
    {
        //zSpawnAdjustment = 2.0f;
        playerScale = player.localScale;
    }

    private void OnTriggerEnter(Collider other)
    {
        //print(other.name);
        if (other.gameObject.tag == "Player" )
        {
            for (int x = 0; x < _enemies.Length; x++) //loop through enemies to spawn
            {
                xSpawnAdjustment += xSpawnAdditive * _enemies[x].adjustPlaneLocation * playerScale.x;
                ySpawnAdjustment += ySpawnAdditive * _enemies[x].adjustPlaneLocation * playerScale.y;
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
                    default:
                        break;
                }
                spawnAdjustment = new Vector3(xSpawnAdjustment, ySpawnAdjustment, zSpawnAdjustment); //spawn adjustment vector to add to other.gameObject.transform.position
                _enemies[x].enemyPrefab.transform.parent = other.gameObject.transform.parent;
                _enemies[x].enemyPrefab.transform.localPosition = spawnAdjustment;
                _enemies[x].enemyPrefab.SetActive(true);
            }
        }
    }
}