using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameStateBase
{
	protected readonly GameController _gameController;

	public GameStateBase(GameController gameController)
	{
		_gameController = gameController;
	}

	public abstract void EnterStart();

	public abstract void ExitStart();

	public abstract GameStateBase RunState();

	protected abstract GameStateBase CheckForStateSwitch();
}
