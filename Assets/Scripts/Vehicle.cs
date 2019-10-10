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
	public ParticleSystem fx_fire;
	public bool isMoving;
	private VTransform _vtransform;
	private float speed;
	private Rigidbody _rigidbody;
	private bool _exploded;
	private bool _canPool;
	public void Init ()
	{
		_canPool = true;
	}
	void Update ()
	{
		if (!isMoving)
		{
			return;
		}
		transform.Translate (-transform.forward * Time.deltaTime * speed, Space.World);
	}

	public void Move (float _speed)
	{
		speed = _speed;
		transform.gameObject.SetActive (true);
		isMoving = true;
		_vtransform.position = transform.localPosition;
		_vtransform.rotation = transform.localRotation;
		_canPool = false;
		fx_fire.Stop();
	}

	public void Deactivate ()
	{
		isMoving = false;
		_exploded = false;
		transform.localPosition = _vtransform.position;
		transform.localRotation = _vtransform.rotation;
		transform.gameObject.SetActive (false);
		_canPool = true;
	}

	public void Explode()
	{
		if (_exploded) { return; }
		_exploded = true;
		isMoving = false;
		if (_rigidbody == null)
		{
			_rigidbody = GetComponent<Rigidbody>();
		}
		fx_fire.Play();
		_rigidbody.isKinematic = false;
		_rigidbody.AddForce (Vector3.up * 500f + (Vector3.forward * 3000f), ForceMode.Force);
		_rigidbody.AddTorque (new Vector3 (Random.value, Random.value, Random.value) * 2000f);
		StartCoroutine("IReadyToDeactive");
	}

	IEnumerator IReadyToDeactive()
	{
		yield return new WaitForSecondsRealtime(3f);
		Deactivate();
	}

	void OnTriggerEnter (Collider col)
	{
		if (col.gameObject.tag == "section/endwall")
		{
			Deactivate ();
		}
	}

	public bool CanPool
	{
		get { return _canPool;}
	}
}