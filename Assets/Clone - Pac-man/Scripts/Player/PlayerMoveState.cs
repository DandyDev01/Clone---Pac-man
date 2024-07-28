using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerStateBase
{
	public PlayerMoveState(Player player) : base(player)
	{
	}

	public override PlayerStateBase CheckForSwitchState()
	{
		if (_player.Input.currentActionMap.actions[0].ReadValue<Vector2>() == Vector2.zero)
		{
			return _player.IdleState;
		}
		else
		{
			return this;
		}
	}

	public override void EnterState()
	{
		Debug.Log("Enter Player Move");
	}

	public override void ExitState()
	{
		Debug.Log("Exit Player Move");
	}

	public override PlayerStateBase RunState()
	{
		_player.Move(_player.Input.currentActionMap.actions[0].ReadValue<Vector2>());

		return CheckForSwitchState();
	}
}
