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
    }

    [SerializeField]
    public enemies[] _enemies; //array of enemy struct

    private float adjustLocation; //adjust spawning around DollyCart box collider size.
    private float initialX;
    private float initialY;
    private float xSpawnAdjustment;
    private float ySpawnAdjustment;
    private float zSpawnAdjustment;
    private Vector3 spawnAdjustment;

    private void Start()
    {
        Vector3 temp = GameObject.Find("GameDollyCart").GetComponent<BoxCollider>().size; //grab collider size
        adjustLocation = 1.5f; //number to spawn ships farther or closer

        initialX = temp.x * adjustLocation;
        initialY = temp.y * adjustLocation;
        zSpawnAdjustment = -2.0f;
    }

    private void OnTriggerEnter(Collider other)
    {
        //print(other.name);
        if (other.gameObject.name == "GameDollyCart")
        {
            //if (//other.GetComponent<PlayerInfo>().enemiesOnScreen == 0)
            //{
            //other.GetComponent<PlayerInfo>().enemiesOnScreen--;
            for (int x = 0; x < _enemies.Length; x++) //loop through enemies to spawn
            {
                xSpawnAdjustment = initialX;
                ySpawnAdjustment = initialY;
                switch (_enemies[x].SpawnLocation) //chance x and/or y adjustment based on selected location
                {
                    case spawnLocation.TopLeft:
                        xSpawnAdjustment *= -1.0f;
                        break;
                    case spawnLocation.TopMiddle:
                        xSpawnAdjustment = 0.0f;
                        break;
                    case spawnLocation.MiddleLeft:
                        xSpawnAdjustment *= -1.0f;
                        ySpawnAdjustment = 0.0f;
                        break;
                    case spawnLocation.MiddleRight:
                        ySpawnAdjustment = 0.0f;
                        break;
                    case spawnLocation.BottomLeft:
                        xSpawnAdjustment *= -1.0f;
                        ySpawnAdjustment *= -1.0f;
                        break;
                    case spawnLocation.BottomRight:
                        ySpawnAdjustment *= -1.0f;
                        break;
                    default:
                        break;
                }
                spawnAdjustment = new Vector3(xSpawnAdjustment, ySpawnAdjustment, zSpawnAdjustment); //spawn adjustment vector to add to other.gameObject.transform.position
                Instantiate(_enemies[x].enemyPrefab, other.gameObject.transform.position + spawnAdjustment, other.transform.rotation, other.gameObject.transform);
            }
            //}
        }
    }
}
