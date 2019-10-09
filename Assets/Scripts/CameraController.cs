using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public static CameraController Instance;
	private bool _isDetached;
	private Vector3 _defaultPosition, _target;

	public float backForce;
	public float backSmoothTime = 5f;
	public float zeroSmoothTime = 2f;

	void Awake ()
	{
		if (Instance == null)
		{
			Instance = this;
		}
	}

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
		if (_isDetached) return;
		transform.position = Vector3.Lerp (transform.position, _target, Time.deltaTime * backSmoothTime);

		if (FlickInput.IS_HOLDING == false)
			_target = Vector3.Lerp (_target, _defaultPosition, Time.deltaTime * zeroSmoothTime);
	}

	public void SetPosition (Vector3 _direction)
	{
		_target = transform.position + _direction * backForce;
	}
}