using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnderwaterFilter : MonoBehaviour
{

	public GameObject filter;

	// Start is called before the first frame update
	void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
			if (filter.activeSelf == false)
			{
				filter.SetActive(true);
			}
			else
			{
				filter.SetActive(false);
			}
		}
	}
}
