using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePauseState : GameStateBase
{
	public GamePauseState(GameController gameController) : base(gameController)
	{
	}

	public override void EnterStart()
	{
		_gameController.PauseView.gameObject.SetActive(true);
		Time.timeScale = 0;
	}

	public override void ExitStart()
	{
		_gameController.PauseView.gameObject.SetActive(false);
		Time.timeScale = 1;
	}

	public override GameStateBase RunState()
	{
		if (_gameController.Input.currentActionMap.actions[1].ReadValue<float>() > 0)
		{
			return _gameController.GameRunState;
		}

		return this;
	}

	protected override GameStateBase CheckForStateSwitch()
	{
		throw new System.NotImplementedException();
	}
}
