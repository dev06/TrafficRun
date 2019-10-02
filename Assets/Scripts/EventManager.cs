﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventID
{
	FINISH,
	ZONE_COMPLETE,
	LEVEL_COMPLETE,
}
public class EventManager : MonoBehaviour
{
	public delegate void SectionTrigger();
	public static SectionTrigger OnSectionTriggerHit;

	public delegate void GameEvent(EventID id);
	public static GameEvent OnGameEvent;

	public delegate void LevelControllerEvents(string _id, int level, int zone);
	public static LevelControllerEvents OnComplete;


}