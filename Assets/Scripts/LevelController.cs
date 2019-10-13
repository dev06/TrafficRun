using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LevelType
{
	NONE,
	BEACH,
	TOWN,
}
public class LevelController : MonoBehaviour
{

	public static LevelController Instance;

	public LevelType staringLevelType = LevelType.BEACH;
	public int startingLevel = 1;
	public int startingZone = 1;

	[HideInInspector]
	public int Level = 1;
	[HideInInspector]
	public int Zone = 1;
	[HideInInspector]
	public LevelType levelType = LevelType.BEACH;

	[HideInInspector]
	public int Progress;
	[HideInInspector]
	public int TotalSections;
	private int levelIndex = 1;

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

	public void Load()
	{
		levelIndex = PlayerPrefs.HasKey("LEVEL_INDEX") ? PlayerPrefs.GetInt("LEVEL_INDEX") : (int)staringLevelType;
		levelType = (LevelType)levelIndex;
		Level = PlayerPrefs.HasKey("LEVEL") ? PlayerPrefs.GetInt("LEVEL") : startingLevel;
		Zone = PlayerPrefs.HasKey("ZONE") ? PlayerPrefs.GetInt("ZONE") : startingZone;
		Progress = 0;
	}

	public void Save()
	{

	}

	void Start()
	{
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

		levelDisplay.ZoneProgress = RatioProgress;
	}

	void OnComplete(string _id, int level, int zone)
	{
		completePage.UpdateData(GameController.Score, GameController.Instance.Best, Zone == 4 ? 1 : 0);
		if (Zone == 4)
		{
			Zone = 1;
			Level++;
			levelIndex++;
			if (levelIndex > 2)
			{
				levelIndex = 1;
			}

		} else
		{
			Zone++;
		}
		PlayerPrefs.SetInt("LEVEL", Level);
		PlayerPrefs.SetInt("ZONE", Zone);
		PlayerPrefs.SetInt("LEVEL_INDEX", levelIndex);
	}

	public float RatioProgress
	{
		get
		{
			return (float)Progress / (float)TotalSections;
		}
	}
}
