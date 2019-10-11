using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class LoadingHandler : MonoBehaviour
{

	public TextMeshProUGUI progressText;
	void Awake()
	{
		StartCoroutine("LoadYourAsyncScene");
	}

	IEnumerator LoadYourAsyncScene ()
	{
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync (1);
		while (!asyncLoad.isDone)
		{
			progressText.text = "Loading.." + (Mathf.Clamp01(asyncLoad.progress / 0.9f) * 100f).ToString("F0") + "%";
			yield return null;
		}
	}
}