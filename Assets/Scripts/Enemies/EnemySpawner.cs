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
        MiddleRight,
        BottomLeft,
        BottomRight
    }

    [Serializable]
    public struct enemies //struct for which enemies to spawn
    {
        public spawnLocation SpawnLocation;
        public GameObject enemyPrefab; //enemy prefab
        [Range(0.0f, 4.0f)]
        public float adjustLocation; //spawn ships closer or farther from player
    }

    [SerializeField]
    public enemies[] _enemies; //array of enemy struct

    private float initialX;
    private float initialY;
    private float xSpawnAdjustment;
    private float ySpawnAdjustment;
    private float zSpawnAdjustment;
    public Transform player;
    private Vector3 spawnAdjustment;
    private Vector3 playerScale;

    private void Start()
    {
        zSpawnAdjustment = -2.0f;
        playerScale = player.localScale;
    }

    private void OnTriggerEnter(Collider other)
    {
        //print(other.name);
        if (other.gameObject.tag == "Player" )
        {
            //if (//other.GetComponent<PlayerInfo>().enemiesOnScreen == 0)
            //{
            //other.GetComponent<PlayerInfo>().enemiesOnScreen--;
            for (int x = 0; x < _enemies.Length; x++) //loop through enemies to spawn
            {
                xSpawnAdjustment = playerScale.x * _enemies[x].adjustLocation;
                ySpawnAdjustment = playerScale.y * _enemies[x].adjustLocation;
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
