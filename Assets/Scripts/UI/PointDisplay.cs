using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
//1 = 5, 7
//2 = 10,15
//3 = 20,25
public class PointDisplay : MonoBehaviour
{
	public bool isNearMissText = false;
	private List<Animation> animations = new List<Animation> ();
	private List<TextMeshProUGUI> texts = new List<TextMeshProUGUI> ();
	int index;

	void OnEnable ()
	{
		EventManager.OnSectionTriggerHit += OnSectionTriggerHit;
		EventManager.OnGameEvent += OnGameEvent;
	}

	void OnDisable ()
	{
		EventManager.OnSectionTriggerHit -= OnSectionTriggerHit;
		EventManager.OnGameEvent -= OnGameEvent;

	}

	void Start ()
	{
		for (int i = 0; i < transform.childCount; i++)
		{

			if (isNearMissText)
			{
				animations.Add (transform.GetChild (i).GetChild (0).GetComponent<Animation> ());
				texts.Add (transform.GetChild (i).GetChild (0).GetComponent<TextMeshProUGUI> ());
			}
			else
			{
				animations.Add (transform.GetChild (i).GetComponent<Animation> ());
				texts.Add (transform.GetChild (i).GetComponent<TextMeshProUGUI> ());

			}
		}
	}

	void OnSectionTriggerHit ()
	{
		if (!GameController.Instance.Player.isAlive) { return; }
		if (!isNearMissText)
		{
			GameController.Score += 10;
			animations[index].Play ();
			texts[index].text = "+1";
			index++;
			if (index > animations.Count - 1)
			{
				index = 0;
			}
		}

	}

	void OnGameEvent (EventID id)
	{
		if (!GameController.Instance.Player.isAlive) { return; }
		texts[index].transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, Random.Range(-15f, 15f)));
		if (id == EventID.NEAR_MISS && isNearMissText && !GameController.Instance.Player.FuryAchieved)
		{
			GameController.Score += 20;
			Haptic.Vibrate (HapticIntensity.Light);
			animations[index].Play ();
			texts[index].text = "Near Miss!";
			index++;
			if (index > animations.Count - 1)
			{
				index = 0;
			}
		} else if (id == EventID.FURY_VEHICLE_HIT && isNearMissText)
		{
			animations[index].Play ();
			texts[index].text = "Hit!";
			index++;
			if (index > animations.Count - 1)
			{
				index = 0;
			}
		}
	}

	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Space))
		{

		}
	}
}