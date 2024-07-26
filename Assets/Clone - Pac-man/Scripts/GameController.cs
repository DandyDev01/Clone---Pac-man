using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private GameView _gameView;
    [SerializeField] private Transform _spawn;

	private PlayerInput _input;

	private int _maxLives;
    private int _remainingLives;
    private int _score;

    public int Score => _score;

	private void Awake()
	{
		_input = GetComponent<PlayerInput>();
	}

	// Start is called before the first frame update
	void Start()
    {
		_player.OnDealth += HandleDeath;
        _player.OnPickup += AddToScore;
        _player.transform.position = _spawn.position;
    }

	private void Update()
	{
		_player.Move(_input.currentActionMap.actions[0].ReadValue<Vector2>());
	}

	private void HandleDeath()
	{
        _remainingLives -= 1;

        if (_remainingLives <= 0)
        {
            // game over
            return;
        }

        _player.transform.position = _spawn.position;
	}

	private void AddToScore(int amount)
	{
		_score += amount;
		_gameView.UpdateScore(_score);
	}

}
