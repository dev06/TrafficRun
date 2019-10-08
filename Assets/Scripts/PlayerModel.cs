using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{
	public ParticleSystem fx_fire;
	private MeshRenderer _mesh;
	private Collider _collider;
	private Rigidbody _rigidbody;
	private bool _vehicleHit;
	void Start()
	{
		_rigidbody = GetComponent<Rigidbody>();
		_collider = GetComponent<Collider>();
		_mesh = GetComponent<MeshRenderer>();
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "section/point_trigger")
		{
			if (EventManager.OnSectionTriggerHit != null)
			{
				EventManager.OnSectionTriggerHit();
			}
		}

		if (col.gameObject.tag == "section/finish")
		{
			if (EventManager.OnGameEvent != null)
			{
				EventManager.OnGameEvent(EventID.FINISH);
			}
		}

		if (col.gameObject.tag == "section/vehicle")
		{
			if (_mesh.enabled && GameController.Instance.debug == false)
			{
				if (EventManager.OnGameEvent != null)
				{
					EventManager.OnGameEvent(EventID.VEHICLE_HIT);
				}
				_vehicleHit = true;
				fx_fire.Play();
				_rigidbody.isKinematic = false;
				_rigidbody.AddForce(Vector3.up * 500f, ForceMode.Force);
				_rigidbody.AddTorque(new Vector3(Random.value, Random.value, Random.value) * 2000f );
			}
			//_mesh.enabled = _collider.enabled = false;
		}
	}

	void Update()
	{
		if (_vehicleHit)
		{
			fx_fire.transform.position = transform.position;
		}
	}
}
