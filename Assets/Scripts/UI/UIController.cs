using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
	public static UIController Instance;

	public Component[] pages;
	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
	}

	void Start()
	{
		pages = GetComponentsInChildren<Page>();
		ShowPage(PageType.Game);
	}


	public void ShowPage(PageType type)
	{
		TogglePagesOff();
		TogglePage(type);
	}

	private void TogglePage(PageType type)
	{
		foreach (Page p in pages)
		{
			if (p.type == type)
			{
				p.Toggle(true);
			}
		}
	}

	private void TogglePagesOff()
	{
		foreach (Page p in pages)
		{
			p.Toggle(false);
		}
	}
}
