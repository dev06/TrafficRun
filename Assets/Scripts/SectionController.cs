using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionController : MonoBehaviour
{
	public static SectionController Instance;

	public float velocity = 30;
	public List<Section> sections = new List<Section> ();
	public Reserved reserved;


	private int remaining = 4;

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
			s.Init();
			if (i > 0)
			{
				Section _previous = transform.GetChild (i - 1).GetComponent<Section> ();
				s.SetLastSection(_previous);
				s.SetToLastSectionPosition();
			}
		}
	}

	void Start ()
	{
		GenerateInitialRoad();
	}

	public void GenerateInitialRoad()
	{
		do
		{
			Section s = transform.GetChild(0).GetComponent<Section>();
			s.transform.SetParent(reserved.transform);
			s.Deactivate();
		} while (transform.childCount > 0);

		for (int i = 0; i < 6; i++)
		{
			Section _sectionToAdd = null;
			if (i == 0)
			{
				_sectionToAdd = reserved.GetSectionInReservedByType(SectionType.STARTER);
				_sectionToAdd.Init();
				_sectionToAdd.transform.position = Vector3.zero;
				_sectionToAdd.transform.SetParent(transform);
				_sectionToAdd.Activate();
				continue;
			}

			Section _last = transform.GetChild (transform.childCount - 1).GetComponent<Section> (); // gets the last section in list
			_sectionToAdd = reserved.GetSectionInReservedByType(SectionType.LANE);
			_sectionToAdd.Init ();
			_sectionToAdd.Connect (_last);
		}
	}



	public void Pool (Section _deactiveSection)
	{
		if (transform.childCount < 1) {return; }
		Section _last = transform.GetChild (transform.childCount - 1).GetComponent<Section> (); // gets the last section in list
		Section _reserved = null;

		if (remaining <= 0)
		{
			_reserved =  reserved.GetSectionInReservedByType (SectionType.FINISH);
		} else
		{
			if (_last.type != SectionType.NO_LANE && remaining >= 1)
			{
				_reserved = reserved.GetSectionInReservedByType (SectionType.NO_LANE); //gets random section from reserved list
			}
			else
			{
				_reserved = reserved.GetSectionInReservedByType (SectionType.LANE); //gets random section from reserved list
			}
		}
		if (remaining >= 0)
		{
			_reserved.Init ();
			_reserved.Connect (_last);
			remaining--;
		}
		_deactiveSection.transform.SetParent (reserved.transform);
	}

	public float Velocity
	{
		get {return velocity; }
		set { this.velocity = value; }
	}
}