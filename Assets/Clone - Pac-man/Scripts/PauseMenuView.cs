using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuView : MonoBehaviour
{
	[SerializeField] private Button _resumeButton;
	[SerializeField] private Button _mainMenuButton;

	private void Awake()
	{
		_resumeButton.onClick.AddListener(Resume);
		_mainMenuButton.onClick.AddListener(MainMenu);
	}

	private void MainMenu()
	{
		SceneManager.LoadScene(0);
	}

	private void Resume()
	{
	}
}
