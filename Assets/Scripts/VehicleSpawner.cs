using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleSpawner : MonoBehaviour
{
	public List<Vehicle> vehicles = new List<Vehicle> ();

	private int _index;
	private Section _parentSection;

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
		if (!vehicles[_index].isMoving)
		{
			vehicles[_index].Move ();
		}
		_index++;
		if (_index > vehicles.Count - 1)
		{
			_index = 0;
		}
	}

	public void Activate()
	{
		InvokeRepeating ("MoveNextVehicle", Random.value, Random.Range (1f, 15f));
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