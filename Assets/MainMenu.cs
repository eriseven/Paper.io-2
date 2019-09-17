using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
	public GameObject gameOver;

	private void Awake()
	{
		GameManager.gm.mm = this;
	}

	public void Retry()
	{
		GameManager.gm.RestartScene();
	}

	public void GameOver()
	{
		gameOver.SetActive(true);
	}
}