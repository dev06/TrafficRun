using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//1 = 5, 7
//2 = 10,15
//3 = 20,25
public class PointDisplay : MonoBehaviour
{
	private List<Animation> animations = new List<Animation>();
	int index;

	void OnEnable()
	{
		EventManager.OnSectionTriggerHit += OnSectionTriggerHit;
	}

	void OnDisable()
	{
		EventManager.OnSectionTriggerHit -= OnSectionTriggerHit;
	}

	void Start()
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			animations.Add(transform.GetChild(i).GetComponent<Animation>());
		}
	}

	void OnSectionTriggerHit()
	{
		GameController.Score += 10;
		Haptic.Vibrate(HapticIntensity.Light);
		animations[index].Play();
		index++;
		if (index > animations.Count - 1)
		{
			index = 0;
		}
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{

		}
	}
}
