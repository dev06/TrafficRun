using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trail : MonoBehaviour
{
	public LineRenderer[] _trails;

	private SectionController _sectionController;
	private bool _active;
	private Transform _defaultParent;
	private float _z;

	void Start()
	{
		_sectionController = SectionController.Instance;
		_defaultParent = transform.parent;
	}

	public void Activate(Vector3 _pos)
	{
		_canSetParent = false;
		transform.SetParent(_defaultParent);
		transform.position = _pos;
		_z = 0 ;
		_trails[0].SetPosition(1, new Vector3(0f, 0f, 0));
		_trails[1].SetPosition(1, new Vector3(0f, 0f, 0));
		foreach (LineRenderer t in _trails)
		{
			t.enabled = true;
		}
		_active = true;

		StartCoroutine("IWait");
	}

	IEnumerator IDeactivate()
	{
		yield return new WaitForSeconds(2);
		foreach (LineRenderer t in _trails)
		{
			t.enabled = false;
		}
		_active = false;
	}

	void Update()
	{
		if (!_active || transform.parent != _defaultParent) { return; }
		_z += Time.deltaTime * _sectionController.Velocity;
		_trails[0].SetPosition(1, new Vector3(0f, 0f, _z));
		_trails[1].SetPosition(1, new Vector3(0f, 0f, _z));

		// transform.Translate(-Vector3.forward * _sectionController.Velocity * Time.deltaTime, Space.World);
	}

	IEnumerator IWait()
	{
		yield return new WaitForSeconds(.3f);
		_canSetParent = true;
	}
	bool _canSetParent;
	public bool CanSetParent
	{	get
		{
			return _canSetParent;
		}
	}

	public bool Active
	{
		get { return _active;}
	}
}

