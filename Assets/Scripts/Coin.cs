using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{

	private Animation _animation;
	private bool _pickedUp;

	private Vector3 _defaultPos, _defaultScale;

	void Update()
	{
		if (_pickedUp) { return; }
		transform.parent.Rotate(Vector3.up, Time.deltaTime * 250f, Space.World);
	}

	public void Pickup()
	{
		_pickedUp = true;
		if (_animation == null)
		{
			_animation = GetComponent<Animation>();
		}
		_animation.Play();
	}

	public void Activate()
	{
		if (_animation == null)
		{
			_animation = GetComponent<Animation>();
		}
		_animation = GetComponent<Animation>();
		_defaultPos = transform.localPosition;
		_defaultScale = transform.localScale;
	}

	public void Deactivate()
	{
		_pickedUp = false;
		transform.localPosition = _defaultPos;
		transform.localScale = _defaultScale;
	}
}
