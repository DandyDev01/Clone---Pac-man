using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuView : MonoBehaviour
{
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _exitButton;

	private void Awake()
	{
		_playButton.onClick.AddListener(Play);
		_exitButton.onClick.AddListener(Exit);
	}

	private void Exit()
	{
		Application.Quit();
	}

	private void Play()
	{
		SceneManager.LoadScene(1);
	}
}
