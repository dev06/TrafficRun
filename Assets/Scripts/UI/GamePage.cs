using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class GamePage : Page
{
	void OnEnable ()
	{
		EventManager.OnStateChange += OnStateChange;
	}

	void OnDisable ()
	{
		EventManager.OnStateChange -= OnStateChange;
	}

	void OnStateChange (State s)
	{
		Toggle (false);
		if (s == State.Game)
		{
			Toggle (true);
		}
	}
}