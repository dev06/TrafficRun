using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct VTransform
{
	public Vector3 position;
	public Quaternion rotation;
}
public class Vehicle : MonoBehaviour
{
	public float speed = 50;

	public bool isMoving;
	private VTransform _vtransform;

	public void Init()
	{
		speed = Random.Range(30f, 55f);
	}
	void Update()
	{
		if (!isMoving)
		{
			return;
		}
		transform.Translate(-transform.forward * Time.deltaTime * speed, Space.World);
	}

	public void Move()
	{
		transform.gameObject.SetActive(true);
		isMoving = true;
		_vtransform.position = transform.localPosition;
		_vtransform.rotation = transform.localRotation;
	}

	public void Deactivate()
	{
		isMoving = false;
		transform.localPosition = _vtransform.position;
		transform.localRotation = _vtransform.rotation;
		transform.gameObject.SetActive(false);
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "section/endwall")
		{
			Deactivate();
		}
	}
}
