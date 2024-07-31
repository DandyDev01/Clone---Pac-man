using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class GameView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
	[SerializeField] private GameObject[] _lives;

	internal void UpdateScore(int score)
	{
		_scoreText.text = score.ToString();
	}

	public void UpdateLives(int amount)
	{
		for (int i = 0; i < _lives.Length; i++)
		{
			_lives[i].GetComponent<Image>().enabled = false;
		}

		for (int i = 0; i < amount; i++)
		{
			_lives[i].GetComponent<Image>().enabled = true;
		}
	}
}
