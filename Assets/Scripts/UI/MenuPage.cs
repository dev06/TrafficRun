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
	public Sprite[] audioSprites;

	public Image audioImage;
	public Image vibrationImage;
	void Start()
	{
		Toggle(true);

	}

	void OnStateChange(State s)
	{
		if (s != State.Menu)
		{
			StopCoroutine("ITurnoff");
			StartCoroutine("ITurnoff");
		}
		if (s == State.Menu)
		{
			GetComponent<Animation>().Stop();
			Toggle(true);
			Haptic.Enabled = PlayerPrefs.HasKey("VIBRATION") ? bool.Parse(PlayerPrefs.GetString("VIBRATION")) : true;
			vibrationImage.sprite = Haptic.Enabled ? vibrationSprites[0] : vibrationSprites[1];

			AudioController.Enabled = PlayerPrefs.HasKey("AUDIO") ? bool.Parse(PlayerPrefs.GetString("AUDIO")) : true;
			audioImage.sprite = AudioController.Enabled ? audioSprites[0] : audioSprites[1];
		}
	}

	IEnumerator ITurnoff()
	{
		GetComponent<Animation>().Play("menu_out");
		yield return new WaitForSeconds(1f);
		//Toggle(false);
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

		if (_event == ButtonEvent.BUTTON_CLICK && _id == ButtonID.B_Audio)
		{
			AudioController.Enabled = !AudioController.Enabled;
			audioImage.sprite = AudioController.Enabled ? audioSprites[0] : audioSprites[1];
			PlayerPrefs.SetString("AUDIO", AudioController.Enabled.ToString());
		}

		if (_event == ButtonEvent.BUTTON_CLICK && _id == ButtonID.B_Store)
		{
			UIController.Instance.ShowPage(PageType.Store);
			GameController.Instance.SetState(State.Store);
		}

		if (_event == ButtonEvent.BUTTON_DOWN && _id == ButtonID.B_Play && GameController.Instance.hasDoneVehicleAnim)
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
