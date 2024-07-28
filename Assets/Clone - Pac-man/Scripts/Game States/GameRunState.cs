using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRunState : GameStateBase
{
	public GameRunState(GameController gameController) : base(gameController)
	{
	}

	public override void EnterStart()
	{
		Debug.Log("Enter Run State");
	}

	public override void ExitStart()
	{
		Debug.Log("Exit Run State");
	}

	public override GameStateBase RunState()
	{
		//_gameController.Player.Move(_gameController.Input.currentActionMap.actions[0].ReadValue<Vector2>());

		return CheckForStateSwitch();
	}

	protected override GameStateBase CheckForStateSwitch()
	{
		if (_gameController.RemainingLives <= 0)
		{
			return _gameController.GameOverState;
		}
		else if (_gameController.Input.currentActionMap.actions[1].ReadValue<float>() > 0)
		{
			return _gameController.GamePauseState;
		}
		else if (_gameController.Player.HasBeenHit)
		{
			_gameController.Player.HasBeenHit = false;
			return _gameController.GameResetState;
		}
		else
			return this;
	}
}
