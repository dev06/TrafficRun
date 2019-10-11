using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleSpawner : MonoBehaviour
{
	public List<Vehicle> vehicles = new List<Vehicle> ();

	private int _index;
	private Section _parentSection;
	private float _laneSpeed;
	private bool _hasInit;

	private void Init()
	{
		if (_hasInit) { return; }
		for (int i = 0; i < transform.childCount; i++)
		{
			Vehicle v = transform.GetChild (i).GetComponent<Vehicle> ();
			v.Init ();
			vehicles.Add (v);
		}

		_parentSection = GetComponentInParent<Section>();
		_hasInit =  true;
	}

	void Start ()
	{

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
		Init();
		_laneSpeed = (Random.Range(10f, 15f) * .015f * LevelController.Instance.Level) + 12f;
		_laneSpeed = Mathf.Clamp(_laneSpeed, 12f, 30f);
		positionVehicles();
		InvokeRepeating ("MoveNextVehicle", Random.value, Random.Range (.5f, 1f));
	}

	private void positionVehicles()
	{
		float v = Random.Range(35f, 70f);
		for (int i = 0; i < vehicles.Count; i++)
		{
			Vector3 _position = new Vector3(0f, 0f, -(i + 1) * v);
			vehicles[i].transform.localPosition = _position;
			vehicles[i].Move(_laneSpeed);
		}
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