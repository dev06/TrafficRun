using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SFX
{
	CAR_CRASH,
	COIN_PICKUP,
	CAR_HORN,
	CAR_BRAKE,
	CAR_CRASH_FURY,
}
public class AudioController : MonoBehaviour
{
	public static AudioController Instance;

	public static bool Enabled = true;
	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
	}

	public AudioClip[] hornClips;

	public AudioSource car_crash;
	public AudioSource coin_pickup;
	public AudioSource car_horn;
	public AudioSource car_brake;
	public AudioSource car_crash_fury;

	private float _pitchTimer;
	private bool _startPitchTimer;

	void OnEnable() {
		EventManager.OnGameEvent += OnGameEvent;
	}

	void OnDisable() {
		EventManager.OnGameEvent -= OnGameEvent;
	}

	void OnGameEvent(EventID id)
	{
		switch (id)
		{
			case EventID.COIN_PICKUP:
			{
				_pitchTimer = 0;
				_startPitchTimer = true;
				break;
			}
		}
	}

	void Update()
	{
		if (_startPitchTimer)
		{
			_pitchTimer += Time.deltaTime;
			if (_pitchTimer > .25f)
			{
				coin_pickup.pitch = 1f;
				_pitchTimer = 0;
				_startPitchTimer = false;
			}
		}
	}


	public void Play(SFX _sfx)
	{
		if (!Enabled) { return; }

		switch (_sfx)
		{
			case SFX.CAR_CRASH:
			{
				car_crash.Play();
				break;
			}

			case SFX.COIN_PICKUP:
			{
				coin_pickup.Play();
				coin_pickup.pitch += .05f;
				if (coin_pickup.pitch > 2f)
				{
					coin_pickup.pitch = 1f;
				}

				break;
			}

			case SFX.CAR_HORN:
			{
				car_horn.clip = hornClips[Random.Range(0, hornClips.Length)];
				car_horn.Play();
				break;
			}

			case SFX.CAR_BRAKE:
			{
				car_brake.Play();
				break;
			}

			case SFX.CAR_CRASH_FURY:
			{
				car_crash_fury.Play();
				break;
			}
		}
	}
}
