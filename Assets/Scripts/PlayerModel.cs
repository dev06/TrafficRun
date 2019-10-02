using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{

	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "section/point_trigger")
		{
			if (EventManager.OnSectionTriggerHit != null)
			{
				EventManager.OnSectionTriggerHit();
			}
		}

		if (col.gameObject.tag == "section/finish")
		{
			if (EventManager.OnGameEvent != null)
			{
				EventManager.OnGameEvent(EventID.FINISH);
			}
		}
	}
}
