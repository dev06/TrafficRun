using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPage : Page
{
	void OnEnable ()
	{
		EventManager.OnStateChange += OnStateChange;
	}

	void OnDisable ()
	{
		EventManager.OnStateChange -= OnStateChange;
	}

    void Start()
    {
        Toggle(true);
    }

	void OnStateChange(State s)
	{
		Toggle(false); 
		if(s == State.Menu)
		{
			Toggle(true); 
		}
	}
}
