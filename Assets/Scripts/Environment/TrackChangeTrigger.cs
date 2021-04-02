using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackChangeTrigger : MonoBehaviour
{
	public CinemachineDollyCart PlayerDollyCart;
	public CinemachinePathBase newTrack;

	private void OnTriggerEnter(Collider other)
	{
		gameObject.SetActive(false);
		PlayerDollyCart.m_Path = newTrack;
		PlayerDollyCart.m_Position = 0;
	}
}
