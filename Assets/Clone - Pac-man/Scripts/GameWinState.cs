using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWinState : GameStateBase
{
	public GameWinState(GameController gameController) : base(gameController)
	{
	}

	public override void EnterStart()
	{
		_gameController.WinView.gameObject.SetActive(true);
		Debug.Log("Enter Win");
	}

	public override void ExitStart()
	{
		_gameController.WinView.gameObject.SetActive(false);
		Debug.Log("Exit Win");
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
