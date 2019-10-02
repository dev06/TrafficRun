using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{

	public static LevelController Instance;

	public int Level = 1;
	public int Zone = 1;


	public LevelDisplay levelDisplay;
	public CompletePage completePage;
	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
	}

	void OnEnable()
	{
		EventManager.OnComplete += OnComplete;
	}

	void OnDisable()
	{
		EventManager.OnComplete -= OnComplete;
	}

	void Start()
	{
		Level = PlayerPrefs.HasKey("LEVEL") ? PlayerPrefs.GetInt("LEVEL") : 1;
		Zone = PlayerPrefs.HasKey("ZONE") ? PlayerPrefs.GetInt("ZONE") : 1;

		levelDisplay.UpdateData(Level, Zone);
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.T)) {
			if (Zone == 4)
			{
				Zone = 1;
				Level++;
			} else
			{
				Zone++;
			}
			PlayerPrefs.SetInt("LEVEL", Level);
			PlayerPrefs.SetInt("ZONE", Zone);
			levelDisplay.UpdateData(Level, Zone);
		}
	}

	void OnComplete(string _id, int level, int zone)
	{
		completePage.UpdateData(GameController.Score, Zone == 4 ? 1 : 0);
		if (Zone == 4)
		{
			Zone = 1;
			Level++;
		} else
		{
			Zone++;
		}
		PlayerPrefs.SetInt("LEVEL", Level);
		PlayerPrefs.SetInt("ZONE", Zone);
	}

}
