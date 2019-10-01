using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SectionType
{
	STARTER,
	LANE,
	NO_LANE,
}
public class Section : MonoBehaviour
{
	public SectionType type;
	public int length;
	private SectionController _sectionController;
	private bool _activate;
	private Section _lastSection;
	private Vector3 _targetPosition;

	public void Init()
	{
		_sectionController = SectionController.Instance;
		_activate = true;
	}

	void Update()
	{
		if (!_activate)
		{
			return;
		}
		//transform.Translate(-Vector3.forward * Time.deltaTime * _sectionController.velocity);

		if (_lastSection != null && transform.GetSiblingIndex() > 0)
		{
			_targetPosition = new Vector3(0f, 0f, _lastSection.transform.position.z + _lastSection.length);
			transform.position = _targetPosition;
		}

		if (hasReachedEnd())
		{
			Pool();
			_activate = false;
		}
	}


	public void Connect(Section s)
	{
		if (_sectionController == null) { _sectionController = SectionController.Instance; }
		transform.gameObject.SetActive(true);
		transform.SetParent(_sectionController.transform);
		transform.position = _targetPosition = s.transform.position + new Vector3(0f, 0f, s.length);
		_lastSection = s;
		_activate = true;
	}



	private bool hasReachedEnd()
	{
		return transform.position.z < -100;
	}

	public void Pool()
	{

		_sectionController.Pool(this);
		Hide();
		_activate = false;
	}

	public void Show()
	{
		transform.gameObject.SetActive(true);
	}

	private void Hide()
	{
		transform.gameObject.SetActive(false);
	}
}
