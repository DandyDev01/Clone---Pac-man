using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour
{
	[Header("UI")]
    [SerializeField] private GameView _gameView;
    [SerializeField] private PauseMenuView _pauseView;
    [SerializeField] private PauseMenuView _gameOverView;
    [SerializeField] private PauseMenuView _winView;


    private Player _player;
    private Transform _spawn;
	private PelletGenerator _pelletGenerator;
	private GhostCordinator _ghostCordinator;
	private const int _maxLives = 3;
    private int _remainingLives = 3;
    private int _score;
	
	public PlayerInput Input { get; private set; }
	public GameStateBase CurrentState { get; private set; }
	public GameStateBase GameRunState { get; private set; }
	public GameStateBase GamePauseState { get; private set; }
	public GameStateBase GameResetState { get; private set; }
	public GameStateBase GameOverState { get; private set; }
	public GameStateBase GameWinState{ get; private set; }

	public GameView GameView => _gameView;
	public PauseMenuView PauseView => _pauseView;
	public PauseMenuView GameOverView => _gameOverView;
	public PauseMenuView WinView => _winView;

	public Player Player => _player;
	public PelletGenerator PelletGenerator => _pelletGenerator;

    public int Score => _score;
	public int RemainingLives => _remainingLives;

	private void Awake()
	{
		_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
		_spawn = GameObject.FindGameObjectWithTag("Respawn").transform;
		_ghostCordinator = FindObjectOfType<GhostCordinator>();
		_pelletGenerator = FindObjectOfType<PelletGenerator>();

		_gameOverView.gameObject.SetActive(false);
		_pauseView.gameObject.SetActive(false);

		GameRunState = new GameRunState(this);
		GamePauseState = new GamePauseState(this);
		GameResetState = new GameResetState(this);
		GameOverState = new GameOverState(this);
		GameWinState = new GameWinState(this);

		CurrentState = GameRunState;

		Input = Player.GetComponent<PlayerInput>();

		_pauseView.SetGameController(this);
	}

	// Start is called before the first frame update
	private void Start()
    {
		_player.OnDealth += HandleDeath;
        _player.OnPickup += AddToScore;
        _player.transform.position = _spawn.position;

		_pelletGenerator.OnAllPelletPickedup += Win;
	}

	private void Update()
	{
		GameStateBase nextState = CurrentState.RunState();
		if (nextState != CurrentState)
		{
			SwitchState(nextState);
		}
	}

	private void Win()
	{
		SwitchState(GameWinState);

		if (_remainingLives < _maxLives)
			_remainingLives += 1;
	}

	private void HandleDeath()
	{
		_ghostCordinator.ScatterMode();

        _remainingLives -= 1;

        if (_remainingLives <= 0)
        {
            // game over
            return;
        }
	}

	private void AddToScore(int amount)
	{
		_score += amount;
		_gameView.UpdateScore(_score);
	}

	internal void SwitchState(GameStateBase newState)
	{
		CurrentState.ExitStart();
		CurrentState = newState;
		CurrentState.EnterStart();
	}
}
