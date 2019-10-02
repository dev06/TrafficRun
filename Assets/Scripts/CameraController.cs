using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public static CameraController Instance;

	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
	}

	public void Detach()
	{
		transform.SetParent(null);

	}
}
