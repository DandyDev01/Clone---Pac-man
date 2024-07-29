using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameResetState : GameStateBase
{
	public GameResetState(GameController gameController) : base(gameController)
	{
	}

	public override void EnterStart()
	{
	}

	public override void ExitStart()
	{
	}

	public override GameStateBase RunState()
	{
		return _gameController.GameRunState;
	}

	protected override GameStateBase CheckForStateSwitch()
	{
		throw new System.NotImplementedException();
	}
}
