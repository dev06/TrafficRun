using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VehicleBuyButton : ButtonEventHandler
{
	public PurchaseButton parentButton;

	public override void OnPointerClick(PointerEventData data)
	{
		if (parentButton.vehicle.IsPurchased) { return; }
		base.OnPointerClick(data);
		parentButton.ButtonClick();
	}
}
