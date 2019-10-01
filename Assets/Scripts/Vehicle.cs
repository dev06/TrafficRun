using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct VTransform
{
	public Vector3 position;
	public Quaternion rotation;
}

[CreateAssetMenu (fileName = "vehicle", menuName = "Scriptable Objects/Vehicle")]
public class VehicleScriptableObject : ScriptableObject
{
	public float minSpeed = 20;
	public float maxSpeed = 30;
}

public class Vehicle : MonoBehaviour
{
	public VehicleScriptableObject vehicleSO;

	public bool isMoving;
	private VTransform _vtransform;
	private float speed;

	public void Init ()
	{
		speed = Random.Range (vehicleSO.minSpeed, vehicleSO.maxSpeed);
	}
	void Update ()
	{
		if (!isMoving)
		{
			return;
		}
		transform.Translate (-transform.forward * Time.deltaTime * speed, Space.World);
	}

	public void Move ()
	{
		transform.gameObject.SetActive (true);
		isMoving = true;
		_vtransform.position = transform.localPosition;
		_vtransform.rotation = transform.localRotation;
	}

	public void Deactivate ()
	{
		isMoving = false;
		transform.localPosition = _vtransform.position;
		transform.localRotation = _vtransform.rotation;
		transform.gameObject.SetActive (false);
	}

	void OnTriggerEnter (Collider col)
	{
		if (col.gameObject.tag == "section/endwall")
		{
			Deactivate ();
		}
	}
}