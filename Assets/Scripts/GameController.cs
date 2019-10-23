using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameAnalyticsSDK;
public enum State
{
	Menu,
	Game,
	GameOver,
	Store,
}
public class GameController : MonoBehaviour
{

	public bool debug = true;
	public bool infiniteLevel = true;

	public static GameController Instance;
	public static int Score;
	public int Best;

	public int Gold;
	public int GoldCollected;

	public State state = State.Menu;

	public LevelController levelController;
	public PlayerMovement player;

	public bool hasDoneVehicleAnim;
	void Awake ()
	{
		if (Instance == null)
		{
			Instance = this;
		}

		GameAnalytics.Initialize();
		Load ();
	}


	void OnEnable()
	{
		EventManager.OnGameEvent += OnGameEvent;
		EventManager.OnStateChange += OnStateChange;
	}
	void OnDisable()
	{
		EventManager.OnGameEvent -= OnGameEvent;
		EventManager.OnStateChange -= OnStateChange;
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

			case EventID.VEHICLE_HIT:
			{
				var parameters = new Dictionary<string, object> ();
				parameters["Level"] = LevelController.Instance.Level;
				parameters["Zone"] =  LevelController.Instance.Zone;
				parameters["Progress"] =  LevelController.Instance.RatioProgress.ToString("F2");
				FacebookManager.Instance.EventSent ("Death", 1, parameters);
				//Debug.Log("Death -> " + LevelController.Instance.Level + " " + LevelController.Instance.Zone + " " + LevelController.Instance.RatioProgress.ToString("F2"));
				break;
			}

			case EventID.FINISH:
			{
				var parameters = new Dictionary<string, object> ();
				parameters["Level"] = LevelController.Instance.Level;
				parameters["Score"] = Score;
				parameters["Best"] = Best;
				//Debug.Log("Game Over -> " + Score + " " + Best);
				FacebookManager.Instance.EventSent ("Game Finish", 1, parameters);
				break;
			}
		}
	}

	void OnStateChange(State s)
	{
		switch (s)
		{
			case State.Game:
			{
				var parameters = new Dictionary<string, object> ();
				parameters["Money"] = Gold;
				parameters["Level"] = LevelController.Instance.Level;
				parameters["Zone"] =  LevelController.Instance.Zone;
				parameters["Vehicle"] = PurchaseableVehicle.active.ID;
				FacebookManager.Instance.EventSent ("Game Start", 1, parameters);
				//Debug.Log("Game Start -> " + LevelController.Instance.Level + " " + PurchaseableVehicle.active.ID);


				break;
			}

			case State.GameOver:
			{

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
		hasDoneVehicleAnim = false;
		levelController.Load ();
		Gold = PlayerPrefs.HasKey("GOLD") ? PlayerPrefs.GetInt("GOLD") : 0;
		Best = PlayerPrefs.HasKey("BEST") ? PlayerPrefs.GetInt("BEST") : 0;
		GoldCollected = 0;
	}

	public void Restart(bool _wasDead = false)
	{
		CalculateScore();
		if (_wasDead)
		{
			Score = 0;
		}
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

	public void CalculateScore()
	{
		if (Score > Best)
		{
			Best = Score;
		}

		PlayerPrefs.SetInt("BEST", Best);
	}

	public void ModifyCoins(int v)
	{
		Gold += v;
		PlayerPrefs.SetInt("GOLD", Gold);
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