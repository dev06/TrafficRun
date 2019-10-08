using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reserved : MonoBehaviour
{
	public Section GetSectionInReserved()
	{
		return transform.GetChild(Random.Range(0, transform.childCount)).GetComponent<Section>();
	}

	public Section GetSectionInReservedByType(SectionType type)
	{
		Section s = transform.GetChild(Random.Range(0, transform.childCount)).GetComponent<Section>();
		int _breakCounter = 0;
		do
		{
			s = transform.GetChild(Random.Range(0, transform.childCount)).GetComponent<Section>();
			_breakCounter++;
			if (_breakCounter > 1000)
			{
				return s;
				_breakCounter = 0;
			}
		} while (s.type != type || s.exclude);

		return s;
	}

	public Section GetSectionInReservedByType(SectionType type, TrafficIntensity intensity)
	{
		Section s = transform.GetChild(Random.Range(0, transform.childCount)).GetComponent<Section>();
		int _breakCounter = 0;
		do
		{
			s = transform.GetChild(Random.Range(0, transform.childCount)).GetComponent<Section>();
			_breakCounter++;
			if (_breakCounter > 1000)
			{
				return s;
				_breakCounter = 0;
			}
		} while (s.type != type || s.exclude || s.trafficIntensity != intensity);

		return s;
	}
}
