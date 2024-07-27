using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverState : GameStateBase
{
	public GameOverState(GameController gameController) : base(gameController)
	{
	}

	public override void EnterStart()
	{
		_gameController.GameOverView.gameObject.SetActive(true);
	}

	public override void ExitStart()
	{
		_gameController.GameOverView.gameObject.SetActive(false);
	}

	public override GameStateBase RunState()
	{
		return this;
	}

	protected override GameStateBase CheckForStateSwitch()
	{
		throw new System.NotImplementedException();
	}
}
