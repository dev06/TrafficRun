using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreArea : MonoBehaviour
{
	public GameObject storeCamera;
	public Transform vehicles;

	void OnEnable()
	{
		EventManager.OnStateChange += OnStateChange;
		EventManager.OnVehicleActive += OnVehicleActive;
	}

	void OnDisable()
	{
		EventManager.OnStateChange -= OnStateChange;
		EventManager.OnVehicleActive -= OnVehicleActive;
	}

	void OnVehicleActive(PurchaseableVehicle v)
	{
		for (int i = 0; i < vehicles.childCount; i++)
		{
			vehicles.GetChild(i).gameObject.SetActive(false);
		}

		vehicles.GetChild(v.GetIndex()).gameObject.SetActive(true);
	}

	void OnStateChange(State s)
	{
		if (s != State.Store)
		{
			storeCamera.SetActive(false);
			return;
		}

		storeCamera.SetActive(true);
	}
}
