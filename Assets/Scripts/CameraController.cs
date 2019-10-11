using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class CameraController : MonoBehaviour
{
	public static CameraController Instance;
	private bool _isDetached;
	private Vector3 _defaultPosition, _target;


	public float backForce;
	public float backSmoothTime = 5f;
	public float zeroSmoothTime = 2f;
	public float shakeIntensity = 4f;

	public Light directionalLight;
	public PostProcessingProfile activeProfile;
	public PostProcessingProfile inactiveProfile;



	void OnEnable ()
	{
		EventManager.OnGameEvent += OnGameEvent;
	}

	void OnDisable ()
	{
		EventManager.OnGameEvent -= OnGameEvent;
	}

	void Awake ()
	{
		if (Instance == null)
		{
			Instance = this;
		}
	}

	private PostProcessingProfile ppProfile ;
	void Start ()
	{
		_defaultPosition = transform.position;
		_target = _defaultPosition;
	}

	public void Detach ()
	{
		transform.SetParent (null);
		_isDetached = true;
	}

	void Update ()
	{
		if (_isDetached) { return; }

		Vector3 _position = _target + (Vector3)getShake();
		transform.position = Vector3.Lerp (transform.position, _position, Time.deltaTime * backSmoothTime);

		if (FlickInput.IS_HOLDING == false && !GameController.Instance.Player.FuryAchieved)
		{
			_target = Vector3.Lerp (_target, _defaultPosition, Time.deltaTime * zeroSmoothTime);
		}
	}

	void OnGameEvent(EventID id)
	{
		switch (id)
		{
			case EventID.FURY_START:
			{
				setBloom(activeProfile);
				directionalLight.intensity = 1.1f;
				break;
			}

			case EventID.FURY_END:
			{
				setBloom(inactiveProfile);
				directionalLight.intensity = 1f;
				break;
			}
		}
	}

	public void SetPosition (Vector3 _direction)
	{
		_target = transform.position + _direction * backForce;
	}

	private Vector2 getShake()
	{
		return GameController.Instance.Player.FuryAchieved ? Random.insideUnitCircle * shakeIntensity : Vector2.zero;
	}

	private void setBloom(PostProcessingProfile pp)
	{
		GetComponent<PostProcessingBehaviour>().profile = pp;
		// BloomModel.Settings bloomSettings = ppProfile.bloom.settings;
		// bloomSettings.bloom.intensity = b;
		// bloomSettings.enabled.value = b > 0;
		// ppProfile.bloom.settings = bloomSettings;
	}
}







