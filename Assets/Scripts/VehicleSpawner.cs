using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleSpawner : MonoBehaviour
{
	public List<Vehicle> vehicles = new List<Vehicle> ();

	private int _index;
	private Section _parentSection;
	private float _laneSpeed;

	void Start ()
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			Vehicle v = transform.GetChild (i).GetComponent<Vehicle> ();
			v.Init ();
			vehicles.Add (v);
		}

		_parentSection = GetComponentInParent<Section>();
	}

	public void MoveNextVehicle ()
	{
		if (vehicles[_index].CanPool)
		{
			vehicles[_index].Move (_laneSpeed);
		}
		_index++;
		if (_index > vehicles.Count - 1)
		{
			_index = 0;
		}
	}

	public void Activate()
	{
		_laneSpeed = Random.Range(15f, 20f) * 2f;
		InvokeRepeating ("MoveNextVehicle", Random.value, Random.Range (1f, 3f));
	}

	public void Deactivate()
	{
		CancelInvoke("MoveNextVehicle");

		for (int i = 0; i < vehicles.Count; i++)
		{
			vehicles[i].Deactivate();
		}
	}
}