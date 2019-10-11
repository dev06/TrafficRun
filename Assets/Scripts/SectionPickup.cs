using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionPickup : MonoBehaviour
{

	public bool isFirst;
	public Transform pickUp;

	private void togglePickup(bool b)
	{
		if (pickUp == null) { return; }
		pickUp.gameObject.SetActive(b);
		if (!b)
		{
			return;
		}
		for (int i = 0; i < pickUp.childCount; i++)
		{
			pickUp.GetChild(i).rotation = Quaternion.Euler(new Vector3(0f, i * 15f, 0f));
		}
	}


	public void Activate()
	{
		togglePickup(true);
		for (int i = 0; i < pickUp.childCount; i++)
		{
			pickUp.GetChild(i).GetChild(0).GetComponent<Coin>().Activate();
		}
	}

	public void Deactivate()
	{
		for (int i = 0; i < pickUp.childCount; i++)
		{
			pickUp.GetChild(i).GetChild(0).GetComponent<Coin>().Deactivate();
		}

		togglePickup(false);
		isFirst = false;
	}
}
