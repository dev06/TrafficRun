using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class MenuPage : Page
{
	void OnEnable ()
	{
		EventManager.OnStateChange += OnStateChange;
		ButtonEventHandler.OnButtonEvent += OnButtonEvent;
	}

	void OnDisable ()
	{
		EventManager.OnStateChange -= OnStateChange;
		ButtonEventHandler.OnButtonEvent -= OnButtonEvent;
	}

	public TextMeshProUGUI goldText;
	public Sprite[] vibrationSprites;
	public Image vibrationImage;
	void Start()
	{
		Toggle(true);
	}

	void OnStateChange(State s)
	{
		Toggle(false);
		if (s == State.Menu)
		{
			Toggle(true);
			Haptic.Enabled = PlayerPrefs.HasKey("VIBRATION") ? bool.Parse(PlayerPrefs.GetString("VIBRATION")) : true;
			vibrationImage.sprite = Haptic.Enabled ? vibrationSprites[0] : vibrationSprites[1];
		}
	}

	void OnButtonEvent(ButtonEvent _event, ButtonID _id)
	{
		if (_event == ButtonEvent.BUTTON_CLICK && _id == ButtonID.B_Vibration)
		{
			Haptic.Enabled = !Haptic.Enabled;
			vibrationImage.sprite = Haptic.Enabled ? vibrationSprites[0] : vibrationSprites[1];
			if (Haptic.Enabled)
			{
				Haptic.Vibrate(HapticIntensity.Light);
			}
			PlayerPrefs.SetString("VIBRATION", Haptic.Enabled.ToString());
		}

		if (_event == ButtonEvent.BUTTON_DOWN && _id == ButtonID.B_Play)
		{
			if (GameController.Instance.state != State.Game)
			{
				GameController.Instance.SetState(State.Game);
			}
		}
	}

	public override void Toggle(bool b)
	{
		base.Toggle(b);
		if (GameController.Instance != null)
		{
			goldText.text = "x" + GameController.Instance.Gold;
		}
	}
}
