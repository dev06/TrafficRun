﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LevelDisplay : MonoBehaviour
{
	public TextMeshProUGUI currentLevel, nextLevel;
	public Image[] connectors;
	public Image[] zoneIncomplete;
	public TextMeshProUGUI[] progress;

	[Header("Connector Settings")]
	public Color completedConnectorColor;
	public Color defaultConnectorColor;

	[Header("Zone Settings")]
	public Color completedZoneColor;
	public Color defaultZoneColor;

	public int level;
	public int zoneIndex;

	private float _zoneProgress;
	private TextMeshProUGUI _activeProgressText;
	private float _currentProgress;

	void Update()
	{
		if (_activeProgressText == null) { return; }
		_currentProgress = Mathf.Lerp(_currentProgress, ZoneProgress, Time.deltaTime * 10f);
		_activeProgressText.text = _currentProgress.ToString("F0") + "%";
	}

	//zoneIndex must start at 1;
	public void UpdateData(int level, int zoneIndex)
	{
		zoneIndex = Mathf.Clamp(zoneIndex, 1, zoneIncomplete.Length + 1);
		currentLevel.text = level.ToString();
		nextLevel.text = (level + 1).ToString();
		UpdateConnectors(zoneIndex);
		UpdateZone(zoneIndex);
		UpdateProgress(zoneIndex);
	}

	private void UpdateConnectors(int index)
	{
		for (int i = 0; i < connectors.Length; i++)
		{
			connectors[i].color = defaultConnectorColor;
		}
		for (int i = 0; i < index; i++)
		{
			connectors[i].color = completedConnectorColor;
		}
	}

	private void UpdateZone(int index)
	{
		for (int i = 0; i < zoneIncomplete.Length; i++)
		{
			zoneIncomplete[i].enabled = false;
		}
		for (int i = 0; i < index; i++)
		{
			if (i > zoneIncomplete.Length - 1)
			{
				continue;
			}
			zoneIncomplete[i].enabled = true;
			zoneIncomplete[i].color = completedZoneColor;
			zoneIncomplete[i].transform.localScale = Vector3.one;
		}

		if (index > zoneIncomplete.Length)	{ return; }
		zoneIncomplete[index - 1].enabled = true;
		zoneIncomplete[index - 1].color = defaultZoneColor;
		zoneIncomplete[index - 1].transform.localScale = Vector3.one * .50f;
	}

	private void UpdateProgress(int zoneIndex)
	{

		for (int i = 0; i < progress.Length; i++)
		{
			progress[i].enabled = false;
		}

		if (zoneIndex == 4) { return; }
		progress[zoneIndex - 1].enabled = true;
		_activeProgressText = progress[zoneIndex - 1];
	}

	public float ZoneProgress
	{
		get {return _zoneProgress;}
		set {_zoneProgress = value * 100f; }
	}
}
