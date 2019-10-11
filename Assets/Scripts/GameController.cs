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

	public int Gold;
	public int GoldCollected;

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


	void OnEnable()
	{
		EventManager.OnGameEvent += OnGameEvent;
	}
	void OnDisable()
	{
		EventManager.OnGameEvent -= OnGameEvent;
	}

	void OnGameEvent(EventID id)
	{
		switch (id)
		{
			case EventID.RESTART:
			{
				SetState(State.Menu);
				Load();
				break;
			}
		}
	}

	// public float scale = .1f;
	// public int input = 1;
	// public float minRange = 10;
	// public float maxRange = 15;
	// void OnValidate()
	// {
	// 	float s = (Random.Range(minRange, maxRange) * input * scale) + 10;
	// 	s = Mathf.Clamp(s, 10f, 30f);
	// 	Debug.Log(s);
	// }

	public void Load ()
	{
		levelController.Load ();
		Gold = PlayerPrefs.HasKey("GOLD") ? PlayerPrefs.GetInt("GOLD") : 0;
		GoldCollected = 0;
	}

	public void Restart()
	{
		Gold += GoldCollected;
		PlayerPrefs.SetInt("GOLD", Gold);
		levelController.Save();
		StartCoroutine("ILoadScene");
	}



	IEnumerator ILoadScene()
	{
		AsyncOperation asyncLoad =  UnityEngine.SceneManagement.SceneManager.LoadSceneAsync (0);
		while (!asyncLoad.isDone)
		{
			yield return null;
		}
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