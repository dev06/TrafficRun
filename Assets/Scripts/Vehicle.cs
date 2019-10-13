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
	private float speed, _targetSpeed;
	private Rigidbody _rigidbody;
	private bool _exploded;
	private bool _canPool;
	private Collider _collider;
	public void Init ()
	{
		_canPool = true;
		_collider = GetComponent<Collider>();
	}
	void Update ()
	{
		if (!isMoving)
		{
			return;
		}
		if (Mathf.Abs(_targetSpeed - speed) > .05f)
		{
			speed = Mathf.Lerp(speed, _targetSpeed, Time.deltaTime * 2f);
		}
		transform.Translate (-transform.forward * Time.deltaTime * speed, Space.World);
	}

	private void setTransform(Vector3 _localPosition, Quaternion _localRotation)
	{
		_vtransform.position = _localPosition;
		_vtransform.rotation = _localRotation;
	}

	public void Move (float _speed)
	{
		_targetSpeed = _speed;
		speed = 50;
		transform.gameObject.SetActive (true);
		isMoving = true;
		// _vtransform.position = transform.localPosition;
		// _vtransform.rotation = transform.localRotation;
		_canPool = false;
		fx_fire.Stop();
		_collider.isTrigger = true;
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

	public void Explode(float upwardForce, float forwardForce)
	{
		if (_exploded) { return; }
		_exploded = true;
		isMoving = false;
		if (GameController.Instance.Player.FuryAchieved == false)
		{
			_collider.isTrigger = false;
		}
		if (_rigidbody == null)
		{
			_rigidbody = GetComponent<Rigidbody>();
		}
		fx_fire.Play();
		_rigidbody.isKinematic = false;
		_rigidbody.AddForce (Vector3.up * upwardForce + (Vector3.forward * forwardForce), ForceMode.Force);
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