using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
	Menu,
	Game,
	GameOver,
}
public class GameController : MonoBehaviour
{

	public bool debug = true;
	public bool infiniteLevel = true;

	public static GameController Instance;
	public static int Score;

	public State state = State.Menu;

	public LevelController levelController;
	public PlayerMovement player;
	void Awake ()
	{
		if (Instance == null)
		{
			Instance = this;
		}

		Load ();
	}

	public void Load ()
	{
		levelController.Load ();
	}

	void Start ()
	{
		SetState (State.Menu);
	}

	public void SetState (State s)
	{
		state = s;
		if (EventManager.OnStateChange != null)
		{
			EventManager.OnStateChange (s);
		}
	}

	public PlayerMovement Player
	{
		get { return player; }
	}
}