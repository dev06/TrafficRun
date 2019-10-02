using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using  UnityEngine.UI;
using TMPro;
public class CompletePage : Page
{
	public Image[] titleImages;
	public TextMeshProUGUI score;


	void OnEnable()
	{
		ButtonEventHandler.OnButtonEvent += OnButtonEvent;
	}
	void OnDisable()
	{
		ButtonEventHandler.OnButtonEvent -= OnButtonEvent;
	}



	void OnButtonEvent(ButtonEvent _event, ButtonID _id)
	{
		if (_event == ButtonEvent.BUTTON_CLICK && _id == ButtonID.B_Continue)
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene(0);
		}
	}


	public void UpdateData(int score, int titleIndex)
	{
		this.score.text = score.ToString();
		titleImages[titleIndex].enabled = true;
	}
}
