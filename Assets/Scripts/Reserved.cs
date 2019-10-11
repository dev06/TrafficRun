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


		// for (int i = 0; i < transform.childCount; i++)
		// {
		// 	Section s = transform.GetChild(i).GetComponent<Section>();
		// 	if (s.type != type || s.exclude)
		// 	{
		// 		continue;
		// 	}

		// 	return s;
		// }



		Section s = transform.GetChild(Random.Range(0, transform.childCount)).GetComponent<Section>();
		int _breakCounter = 0;
		do
		{
			s = transform.GetChild(Random.Range(0, transform.childCount)).GetComponent<Section>();
			_breakCounter++;
			if (_breakCounter > 1000)
			{
				//	Debug.Log("Break -> returning " + s.transform.gameObject.name);
				return s;
				_breakCounter = 0;
			}
		} while (s.type != type || s.exclude);

		return s;
	}

	// public Section GetSectionInReservedByType(SectionType type, TrafficIntensity intensity)
	// {
	// 	LevelType l = LevelController.Instance.levelType;
	// 	Section s = transform.GetChild(Random.Range(0, transform.childCount)).GetComponent<Section>();
	// 	int _breakCounter = 0;
	// 	do
	// 	{
	// 		s = transform.GetChild(Random.Range(0, transform.childCount)).GetComponent<Section>();
	// 		_breakCounter++;
	// 		if (_breakCounter > 1000)
	// 		{
	// 			Debug.Log("Break -> returning " + s.transform.gameObject.name);
	// 			return s;
	// 			_breakCounter = 0;
	// 		}
	// 	} while (s.type != type || s.exclude || s.trafficIntensity != intensity || s.levelType != l);

	// 	return s;
	// }


	public Section GetSectionInReservedByType(SectionType type, TrafficIntensity intensity)
	{

		// for (int i = 0; i < transform.childCount; i++)
		// {
		// 	Section s = transform.GetChild(i).GetComponent<Section>();
		// 	if (s.type != type || s.exclude || s.trafficIntensity != intensity)
		// 	{
		// 		continue;
		// 	}

		// 	return s;
		// }

		LevelType l = LevelController.Instance.levelType;
		Section s = transform.GetChild(Random.Range(0, transform.childCount)).GetComponent<Section>();
		int _breakCounter = 0;
		do
		{
			s = transform.GetChild(Random.Range(0, transform.childCount)).GetComponent<Section>();
			_breakCounter++;
			if (_breakCounter > 1000)
			{
				//Debug.Log("Break -> returning " + s.transform.gameObject.name);
				return s;
				_breakCounter = 0;
			}
		} while (s.type != type || s.exclude || s.trafficIntensity != intensity);

		//return null;
		return s;
	}
}
