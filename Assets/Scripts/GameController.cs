using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

	public static GameController Instance;
	public static int Score;

	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
	}

	void Start()
	{

	}
}
