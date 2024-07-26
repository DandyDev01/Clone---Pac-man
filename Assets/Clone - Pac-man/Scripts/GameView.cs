using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;

	internal void UpdateScore(int score)
	{
		_scoreText.text = score.ToString();
	}
}
