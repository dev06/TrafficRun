using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{
	public int targetZValue = 78;
	public GameObject fx_shield;
	public ParticleSystem fx_fire, fx_shield_wind, fx_shield_par;
	public ParticleSystem[] fx_thrust_regular, fx_thrust_fury;
	private MeshRenderer _mesh;
	private Collider _collider;
	private Rigidbody _rigidbody;
	private bool _vehicleHit;
	private Vector3 _defaultPos;
	private Quaternion _defaultRot;

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

		transform.position = Vector3.zero;
		_defaultRot = transform.rotation;
		_defaultPos = transform.position;
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

			case EventID.RESTART:
			{
				_vehicleHit = false;
				fx_fire.transform.gameObject.SetActive(false);
				_rigidbody.isKinematic = true;
				_rigidbody.velocity = Vector3.zero;
				transform.position = _defaultPos;
				transform.rotation = _defaultRot;
				break;
			}

		}
	}

	void OnTriggerEnter (Collider col)
	{
		if (col.gameObject.tag == "section/road_start")
		{
			LevelController.Instance.Progress++;
			GameController.Instance.Player.currentSection = col.gameObject.transform.parent.parent.GetComponent<Section>();
			//Debug.Log(GameController.Instance.Player.currentSection);
		}
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

		if (col.gameObject.tag == "vehicle/nearMiss_f")
		{
			AudioController.Instance.Play(SFX.CAR_HORN);
		}

		if (col.gameObject.tag == "objects/coin")
		{
			if (EventManager.OnGameEvent != null)
			{
				EventManager.OnGameEvent(EventID.COIN_PICKUP);
			}
			Haptic.Vibrate(HapticIntensity.Light);
			col.gameObject.GetComponent<Coin>().Pickup();
			GameController.Instance.GoldCollected++;
			AudioController.Instance.Play(SFX.COIN_PICKUP);
		}

		if (!GameController.Instance.Player.EnableProtection && !_vehicleHit)
		{
			if (col.gameObject.tag == "section/vehicle")
			{
				if (_mesh.enabled && GameController.Instance.debug == false)
				{
					if (EventManager.OnGameEvent != null)
					{
						EventManager.OnGameEvent (EventID.VEHICLE_HIT);
					}
					AudioController.Instance.Play(SFX.CAR_CRASH);
					Haptic.Instance.VibrateTwice(.1f, HapticIntensity.Medium);
					_vehicleHit = true;
					fx_fire.transform.gameObject.SetActive(true);
					fx_fire.Play ();
					_rigidbody.isKinematic = false;
					_rigidbody.AddForce (Vector3.up * 500f, ForceMode.Force);
					_rigidbody.AddTorque (new Vector3 (Random.value, Random.value, Random.value) * 2000f);

					Vehicle v = col.gameObject.GetComponent<Vehicle>();
					v.Explode(500f, 500f);
				}
				//_mesh.enabled = _collider.enabled = false;
			}
		} else
		{
			if (col.gameObject.tag == "section/vehicle")
			{
				Vehicle v = col.gameObject.GetComponent<Vehicle>();
				v.Explode(500f, 3000f);
				Haptic.Vibrate(HapticIntensity.Medium);
				AudioController.Instance.Play(SFX.CAR_CRASH_FURY);
				if (EventManager.OnGameEvent != null)
				{
					EventManager.OnGameEvent (EventID.FURY_VEHICLE_HIT);
				}
			}
		}


	}

	void OnTriggerExit(Collider col)
	{
		if (col.gameObject.tag == "vehicle/nearMiss" || col.gameObject.tag == "vehicle/nearMiss_f")
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

		if (!GameController.Instance.hasDoneVehicleAnim)
		{
			if (transform.position.z < targetZValue)
			{
				transform.Translate(Vector3.forward * 75f * Time.deltaTime, Space.World);
			} else {
				GameController.Instance.hasDoneVehicleAnim = true;
			}
		}

		//	transform.position = Vector3.Lerp(transform.position, new Vector3(0f, 0f, targetZValue), Time.deltaTime * 10f);
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