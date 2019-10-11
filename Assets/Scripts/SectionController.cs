using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionController : MonoBehaviour
{
	public static SectionController Instance;

	public float velocity = 30;
	public List<Section> sections = new List<Section> ();

	public Reserved beachReserved, townReserved;
	private Reserved reserved;

	private int remaining;

	void Awake ()
	{
		if (Instance == null)
		{
			Instance = this;
		}

		Application.targetFrameRate = 60;
	}

	public void AlignSections ()
	{

		for (int i = 0; i < transform.childCount; i++)
		{
			Section s = transform.GetChild (i).GetComponent<Section> ();
			s.Init ();
			if (i > 0)
			{
				Section _previous = transform.GetChild (i - 1).GetComponent<Section> ();
				s.SetLastSection (_previous);
				s.SetToLastSectionPosition ();
			}
		}
	}

	void OnEnable ()
	{

		EventManager.OnGameEvent += OnGameEvent;
	}

	void OnDisable ()
	{

		EventManager.OnGameEvent -= OnGameEvent;
	}

	void OnGameEvent(EventID id)
	{
		switch (id)
		{
			case EventID.RESTART:
			{
				velocity = 0;
				GenerateInitialRoad();
				break;
			}
		}
	}

	void Start ()
	{
		GenerateInitialRoad ();
	}

	public void GenerateInitialRoad ()
	{
		reserved = getReservedByLevel();

		do
		{
			Section s = transform.GetChild (0).GetComponent<Section> ();
			s.transform.SetParent (s.levelType == LevelType.TOWN ? townReserved.transform : beachReserved.transform);
			s.Deactivate ();
		} while (transform.childCount > 0);

		int _zone = LevelController.Instance.Zone;
		remaining = _zone == 4 ? 5 : 30;

		for (int i = 0; i < 8; i++)
		{
			Section _sectionToAdd = null;
			if (i == 0)
			{
				_sectionToAdd = reserved.GetSectionInReservedByType (SectionType.NO_LANE);
				_sectionToAdd.Init ();
				_sectionToAdd.transform.position = Vector3.zero;
				_sectionToAdd.transform.SetParent (transform);
				_sectionToAdd.Activate ();

				if (_sectionToAdd.transform.GetComponent<SectionPickup>() != null)
				{
					_sectionToAdd.transform.GetComponent<SectionPickup>().Deactivate();
				}
				continue;
			}

			Section _last = transform.GetChild (transform.childCount - 1).GetComponent<Section> (); // gets the last section in list
			if (_last.type != SectionType.NO_LANE)
			{
				_sectionToAdd = reserved.GetSectionInReservedByType (SectionType.NO_LANE);
			}
			else
			{
				SectionType _type = _zone == 4 ? SectionType.NO_LANE : SectionType.STARTER;
				TrafficIntensity _traffic = _zone == 4 ? TrafficIntensity.NONE : TrafficIntensity.LIGHT;
				_sectionToAdd = reserved.GetSectionInReservedByType (_type, _traffic);
			}

			_sectionToAdd.Init ();
			_sectionToAdd.Connect (_last);
			if (_sectionToAdd.transform.GetComponent<SectionPickup>() != null)
			{
				_sectionToAdd.transform.GetComponent<SectionPickup>().Activate();
			}
		}
	}

	public void Pool (Section _deactiveSection)
	{
		if (transform.childCount < 1) { return; }
		Section _last = transform.GetChild (transform.childCount - 1).GetComponent<Section> (); // gets the last section in list
		Section _reserved = null;

		if (remaining <= 0)
		{
			_reserved = reserved.GetSectionInReservedByType (SectionType.FINISH);
		}
		else
		{
			if (_last.type != SectionType.NO_LANE && remaining >= 1)
			{
				_reserved = reserved.GetSectionInReservedByType (SectionType.NO_LANE); //gets random section from reserved list
			}
			else
			{
				_reserved = reserved.GetSectionInReservedByType (getLane(), getTrafficIntensity ()); //gets random section from reserved list
			}
		}
		if (remaining >= 0)
		{
			_reserved.Init ();
			_reserved.Connect (_last);
			if (GameController.Instance.infiniteLevel == false)
			{
				remaining--;
			}
		}
		_deactiveSection.transform.SetParent (_deactiveSection.levelType == LevelType.TOWN ? townReserved.transform : beachReserved.transform);
	}

	public float Velocity
	{
		get { return velocity; }
		set { this.velocity = value; }
	}

	private SectionType getLane ()
	{
		int _zone = LevelController.Instance.Zone;
		switch (_zone)
		{
			case 4:
			{
				return SectionType.NO_LANE;
			}
		}

		return SectionType.LANE;
	}

	private TrafficIntensity getTrafficIntensity ()
	{
		int _zone = LevelController.Instance.Zone;
		int _level = LevelController.Instance.Level;
		if (_level < 5 && _zone != 4)
		{
			return TrafficIntensity.LIGHT;
		}

		switch (_zone)
		{
			case 1:
			{
				return TrafficIntensity.LIGHT;
			}
			case 2:
			{
				float _p = Random.value;
				return _p < .5f ? TrafficIntensity.MEDIUM : TrafficIntensity.LIGHT;
			}

			case 3:
			{
				float _p = Random.value;
				return _p < .5f ? TrafficIntensity.HEAVY : ((Random.value < .5f) ? TrafficIntensity.MEDIUM : TrafficIntensity.LIGHT);
			}
		}
		return TrafficIntensity.NONE;
	}

	private Reserved getReservedByLevel()
	{
		return LevelController.Instance.levelType == LevelType.BEACH ? beachReserved : townReserved;
	}
}