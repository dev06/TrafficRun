using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ProgressMeter : MonoBehaviour
{
	public Image foreground;
	public float fill;
	public float smoothTime = 30f;

	public bool lerpColor;
	public Color startColor;
	public Color lerpToColor;

	void Start()
	{
		foreground.color = startColor;
	}
	void Update()
	{
		foreground.fillAmount = Mathf.Lerp(foreground.fillAmount, fill, Time.deltaTime * smoothTime);
		if (lerpColor)
		{
			foreground.color = Color.Lerp(startColor, lerpToColor, fill);
		}
	}
}
