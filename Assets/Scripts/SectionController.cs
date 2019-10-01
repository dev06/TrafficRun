using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionController : MonoBehaviour
{
	public static SectionController Instance;

	public float velocity = 10;
	public List<Section> sections = new List<Section>();
	public Reserved reserved;

	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
	}


	public void AlignSections()
	{

		for (int i = 0; i < transform.childCount; i++)
		{
			Vector3 _pos = Vector3.zero;
			if (i > 0)
			{
				Section _previous = transform.GetChild(i - 1).GetComponent<Section>();
				float z = _previous.transform.position.z + _previous.length;
				_pos = new Vector3(0f, 0f, z);
			}

			Section s = transform.GetChild(i).GetComponent<Section>();
			s.Init();
			s.transform.position = _pos;
		}
	}

	void Start()
	{
		AlignSections();
	}

	public void Pool(Section _deactiveSection)
	{
		Section _last = transform.GetChild(transform.childCount - 1).GetComponent<Section>(); // gets the last section in list
		Section _reserved = null;
		if (_last.type != SectionType.NO_LANE)
		{
			_reserved = reserved.GetSectionInReservedByType(SectionType.NO_LANE); //gets random section from reserved list
		} else
		{
			_reserved = reserved.GetSectionInReservedByType(SectionType.LANE); //gets random section from reserved list
		}
		_reserved.Init();
		_reserved.Connect(_last);
		_deactiveSection.transform.SetParent(reserved.transform);
	}
}
