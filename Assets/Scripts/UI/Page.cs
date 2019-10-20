using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum PageType
{
	Game,
	Menu,
	Complete,
	GameOver,
	Store,
}
[RequireComponent(typeof(CanvasGroup))]
public class Page : MonoBehaviour
{
	private  CanvasGroup _cg;

	protected Animation _animation;
	public PageType type;
	public bool showInEdit;

	void OnValidate()
	{
		Toggle(showInEdit);
	}

	public virtual void Toggle(bool _b)
	{
		if (_cg == null)
		{
			_cg = GetComponent<CanvasGroup>();
		}

		_cg.alpha = _b ? 1f : 0f;
		_cg.blocksRaycasts = _b;

		if (_animation == null)
		{
			_animation = GetComponent<Animation>();
		}
		if (_animation == null)
		{
			return;
		}
		_animation.Play();
	}
}
