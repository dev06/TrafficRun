using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleSpawner : MonoBehaviour
{
	public List<Vehicle> vehicles = new List<Vehicle> ();

	private int _index;

	void Start ()
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			Vehicle v = transform.GetChild (i).GetComponent<Vehicle> ();
			v.Init ();
			vehicles.Add (v);
		}

		InvokeRepeating ("MoveNextVehicle", 1.0f, Random.Range (1f, 3f));
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
}