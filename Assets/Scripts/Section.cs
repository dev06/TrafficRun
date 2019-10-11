using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SectionType
{
	STARTER,
	LANE,
	NO_LANE,
	FINISH,
}

public enum TrafficIntensity
{
	NONE,
	LIGHT,
	MEDIUM,
	HEAVY,
}

public class Section : MonoBehaviour
{
	public SectionType type;
	public TrafficIntensity trafficIntensity;
	public LevelType levelType;
	public bool exclude;
	public Transform propSets;

	private SectionController _sectionController;
	private bool _activate;
	private Section _lastSection;
	private Vector3 _targetPosition;
	public Transform _endConnector;
	private Component[] _vehicleSpawners;
	private SectionPickup _pickups;
	public void Init ()
	{
		_sectionController = SectionController.Instance;
		_activate = true;
		_targetPosition = Vector3.zero;
		_endConnector = transform.GetChild(0).GetChild(0).transform;
		_pickups = GetComponent<SectionPickup>();
		_vehicleSpawners = GetComponentsInChildren<VehicleSpawner>();
	}

	void Update ()
	{
		if (!_activate)
		{
			return;
		}
		transform.Translate (-Vector3.forward * Time.deltaTime * _sectionController.velocity, Space.World);
		if (_lastSection != null && transform.GetSiblingIndex () > 0)
		{
			transform.position = _lastSection._endConnector.transform.position;
		}

		if (hasReachedEnd ())
		{
			Pool();
			Deactivate();
			_activate = false;
		}
	}


	public void Activate()
	{
		transform.gameObject.SetActive (true);
		transform.SetParent (_sectionController.transform);
		if (propSets != null)
		{
			for (int i = 0; i < propSets.childCount; i++)
			{
				propSets.transform.GetChild(i).gameObject.SetActive(false);
			}

			propSets.transform.GetChild(Random.Range(0, propSets.childCount)).gameObject.SetActive(true);
		}
		_activate = true;
		foreach (VehicleSpawner v in _vehicleSpawners)
		{
			v.Activate();
		}

		if (_pickups != null)
		{
			_pickups.Activate();
		}
	}


	public void Connect (Section s)
	{
		if (_sectionController == null) { _sectionController = SectionController.Instance; }
		_lastSection = s;
		transform.position = _lastSection._endConnector.transform.position;
		Activate();
	}

	public void SetLastSection(Section s)
	{
		_lastSection = s;
		transform.position = _lastSection._endConnector.transform.position;
	}

	public void SetToLastSectionPosition()
	{
		transform.position = _lastSection._endConnector.transform.position;
	}

	private bool hasReachedEnd ()
	{
		return transform.position.z < -250;
	}

	public void Deactivate()
	{
		Hide();

		_activate = false;
	}

	public void Pool ()
	{
		if (_sectionController == null) { _sectionController = SectionController.Instance; }
		_sectionController.Pool (this);
		foreach (VehicleSpawner v in _vehicleSpawners)
		{
			v.Deactivate();
		}

		if (_pickups != null)
		{
			_pickups.Deactivate();
		}
	}

	public void Show ()
	{
		transform.gameObject.SetActive (true);
	}

	public void Hide ()
	{
		transform.gameObject.SetActive (false);
		if (propSets != null)
		{
			for (int i = 0; i < propSets.childCount; i++)
			{
				propSets.GetChild(i).gameObject.SetActive(false);
			}

		}
	}
}