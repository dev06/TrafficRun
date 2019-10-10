﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{
	public GameObject fx_shield;
	public ParticleSystem fx_fire, fx_shield_wind, fx_shield_par;
	public ParticleSystem[] fx_thrust_regular, fx_thrust_fury;
	private MeshRenderer _mesh;
	private Collider _collider;
	private Rigidbody _rigidbody;
	private bool _vehicleHit;


	void OnEnable ()
	{
		EventManager.OnGameEvent += OnGameEvent;
	}

	void OnDisable ()
	{
		EventManager.OnGameEvent -= OnGameEvent;
	}



	void Start ()
	{
		_rigidbody = GetComponent<Rigidbody> ();
		_collider = GetComponent<Collider> ();
		_mesh = GetComponent<MeshRenderer> ();
	}

	void OnGameEvent(EventID id)
	{
		switch (id)
		{
			case EventID.FURY_START:
			{

				ToggleFXFury(fx_thrust_fury, true);
				ToggleFXFury(fx_thrust_regular, false);
				fx_shield_wind.Play();
				fx_shield_par.Play();
				fx_shield.SetActive(true);
				fx_shield.GetComponent<MeshRenderer>().enabled = true;
				break;
			}
			case EventID.FURY_END:
			{
				ToggleFXFury(fx_thrust_fury, false);
				fx_shield.transform.GetComponent<Animation>().Play();
				fx_shield_wind.Stop();
				fx_shield_par.Stop();
				break;
			}

			case EventID.PROTECTION_END:
			{
				fx_shield.SetActive(false);
				break;
			}

		}
	}
	bool isPlaying;
	void OnTriggerEnter (Collider col)
	{
		if (col.gameObject.tag == "section/point_trigger")
		{
			if (EventManager.OnSectionTriggerHit != null)
			{
				EventManager.OnSectionTriggerHit ();
			}
		}

		if (col.gameObject.tag == "section/finish")
		{
			if (EventManager.OnGameEvent != null)
			{
				EventManager.OnGameEvent (EventID.FINISH);
			}
		}

		if (!GameController.Instance.Player.EnableProtection)
		{
			isPlaying = false;
			if (col.gameObject.tag == "section/vehicle")
			{
				if (_mesh.enabled && GameController.Instance.debug == false)
				{
					if (EventManager.OnGameEvent != null)
					{
						EventManager.OnGameEvent (EventID.VEHICLE_HIT);
					}
					_vehicleHit = true;
					fx_fire.Play ();
					_rigidbody.isKinematic = false;
					_rigidbody.AddForce (Vector3.up * 500f, ForceMode.Force);
					_rigidbody.AddTorque (new Vector3 (Random.value, Random.value, Random.value) * 2000f);
				}
				//_mesh.enabled = _collider.enabled = false;
			}
		} else
		{
			if (col.gameObject.tag == "section/vehicle")
			{
				Vehicle v = col.gameObject.GetComponent<Vehicle>();
				v.Explode();
			}
		}


	}

	void OnTriggerExit(Collider col)
	{
		if (col.gameObject.tag == "vehicle/nearMiss")
		{
			if (EventManager.OnGameEvent != null)
			{
				EventManager.OnGameEvent (EventID.NEAR_MISS);
			}
		}
	}

	void Update ()
	{
		if (_vehicleHit)
		{
			fx_fire.transform.position = transform.position;
		}

		if (GameController.Instance.Player.FuryAchieved == false)
		{
			if (FlickInput.IS_HOLDING && !_vehicleHit)
			{
				ToggleFXFury(fx_thrust_regular, true);
			} else
			{

				ToggleFXFury(fx_thrust_regular, false);
			}
		}

		// if (GameController.Instance.Player.FuryAchieved && GameController.Instance.Player.furyMeter.fill < .2f && !isPlaying)
		// {
		// 	fx_shield.transform.GetComponent<Animation>().Play();
		// 	isPlaying = true;
		// }

	}

	public void ToggleFXFury(ParticleSystem[] ps, bool b)
	{
		if (b)
		{
			ps[0].Play();
			ps[1].Play();
		}
		else
		{
			ps[0].Stop();
			ps[1].Stop();
		}
	}
}