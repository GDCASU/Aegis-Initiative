using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidHandler : MonoBehaviour
{
    public int numberOfSmall;
    public int numberOfMedium;
    public int numberOfBig;
    public int healthOfSmall;
    public int healthOfMedium;
    public int healthOfBig;
    public Vector2 smallSizeRange;
    public Vector2 mediumSizeRange;
    public Vector2 bigSizeRange;
    public Vector2 velocityRange;
    public GameObject asteroidPrefab1;
    public GameObject asteroidPrefab2;
    public GameObject asteroidPrefab3;
    public GameObject asteroidPrefab4;
    private GameObject temp;
    private GameObject prefab;
    private int rng;
    private void Start()
    {
        
        for (int x = 0; x < numberOfSmall; x++)
        {
            MakeAsteroid(0);
        }
        for (int x = 0; x < numberOfMedium; x++)
        {
            MakeAsteroid(1);
        }
        for (int x = 0; x < numberOfBig; x++)
        {
            MakeAsteroid(2);
        }
    }
    
    public void MakeAsteroid(int size)
    {
        rng = Random.Range(0, 4);
        prefab = asteroidPrefab1;
        switch (rng)
        {
            case 0:
                prefab = asteroidPrefab1;
                break;
            case 1:
                prefab = asteroidPrefab2;
                break;
            case 2:
                prefab = asteroidPrefab3;
                break;
            case 3:
                prefab = asteroidPrefab4;
                break;
        }
        rng = Random.Range(0,3);
        Vector3 position = new Vector3(Random.Range(-(transform.localScale.x * GetComponent<BoxCollider>().size.x) / 2, (transform.localScale.x * GetComponent<BoxCollider>().size.x) / 2),
            Random.Range(-(transform.localScale.y * GetComponent<BoxCollider>().size.y) / 2, (transform.localScale.y * GetComponent<BoxCollider>().size.y) / 2),
            Random.Range(-(transform.localScale.z * GetComponent<BoxCollider>().size.z) / 2, (transform.localScale.z * GetComponent<BoxCollider>().size.z) / 2));
        temp = Instantiate(prefab,transform);
        temp.transform.localPosition = position;
        switch (size)
        {
            case 0:
                temp.GetComponent<Asteroid>().SetProperties(smallSizeRange, transform.position, healthOfSmall);
                break;
            case 1:
                temp.GetComponent<Asteroid>().SetProperties(mediumSizeRange, transform.position , healthOfMedium);
                break;
            case 2:
                temp.GetComponent<Asteroid>().SetProperties(bigSizeRange, transform.position , healthOfBig);
                break;
        }
    }
}
