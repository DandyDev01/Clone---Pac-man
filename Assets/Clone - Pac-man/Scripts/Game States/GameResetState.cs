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
		Debug.Log("Enter Game Reset");
	}

	public override void ExitStart()
	{
		Debug.Log("Exit Game Reset");
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
