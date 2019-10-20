using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class StorePage : Page
{
	public Transform buttons;
	public TextMeshProUGUI goldText;
	void OnEnable ()
	{
		EventManager.OnStateChange += OnStateChange;
		ButtonEventHandler.OnButtonEvent += OnButtonEvent;
		EventManager.OnVehiclePurchase += OnVehiclePurchase;
	}

	void OnDisable ()
	{
		EventManager.OnStateChange -= OnStateChange;
		ButtonEventHandler.OnButtonEvent -= OnButtonEvent;
		EventManager.OnVehiclePurchase -= OnVehiclePurchase;
	}

	void OnStateChange (State s)
	{
		Toggle (false);
		if (s == State.Store)
		{
			Toggle (true);
			goldText.text = "x" + GameController.Instance.Gold;
			for (int i = 0; i < buttons.childCount; i++)
			{
				buttons.GetChild(i).GetComponent<PurchaseButton>().updateData();
			}
		}
	}

	void OnVehiclePurchase(PurchaseableVehicle v)
	{
		for (int i = 0; i < buttons.childCount; i++)
		{
			buttons.GetChild(i).GetComponent<PurchaseButton>().updateData();
		}
	}

	void Update()
	{
		goldText.text = "x" + GameController.Instance.Gold;
	}

	void OnButtonEvent(ButtonEvent _event, ButtonID _id)
	{
		switch (_event)
		{
			case ButtonEvent.BUTTON_CLICK:
			{

				switch (_id)
				{
					case ButtonID.B_Back:
					{
						UIController.Instance.ShowPage(PageType.Menu);
						GameController.Instance.SetState(State.Menu);
						break;
					}
				}
				break;
			}
		}
	}
}
