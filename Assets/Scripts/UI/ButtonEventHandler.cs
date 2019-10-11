using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum ButtonEvent
{
	BUTTON_CLICK,
	BUTTON_UP,
	BUTTON_DOWN,
}
public enum ButtonID
{
	B_None,
	B_Vibration,
	B_Continue,
	B_Play,
}

public class ButtonEventHandler : MonoBehaviour, IPointerClickHandler, IPointerUpHandler, IPointerDownHandler
{
	public ButtonID buttonID;

	public delegate void ButtonEvents(ButtonEvent _event, ButtonID _id);
	public static ButtonEvents OnButtonEvent;


	public virtual void OnPointerClick(PointerEventData data)
	{
		if (ButtonEventHandler.OnButtonEvent != null)
		{
			ButtonEventHandler.OnButtonEvent(ButtonEvent.BUTTON_CLICK, buttonID);
		}
	}

	public virtual void OnPointerUp(PointerEventData data)
	{
		if (ButtonEventHandler.OnButtonEvent != null)
		{
			ButtonEventHandler.OnButtonEvent(ButtonEvent.BUTTON_UP, buttonID);
		}
	}

	public virtual void OnPointerDown(PointerEventData data)
	{
		if (ButtonEventHandler.OnButtonEvent != null)
		{
			ButtonEventHandler.OnButtonEvent(ButtonEvent.BUTTON_DOWN, buttonID);
		}
	}
}
