using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

	public bool debug = true;

	public static GameController Instance;
	public static int Score;

	public LevelController levelController;
	public PlayerMovement player;
	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}

		Load();
	}

	public void Load()
	{
		levelController.Load();
	}

	void Start()
	{

	}

	public PlayerMovement Player
	{
		get {return player; }
	}
}
