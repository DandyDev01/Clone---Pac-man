using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerStateBase
{
	public PlayerIdleState(Player player) : base(player)
	{
	}

	public override PlayerStateBase CheckForSwitchState()
	{
		if (_player.Input.currentActionMap.actions[0].ReadValue<Vector2>() == Vector2.zero)
		{
			return this;
		}
		else
		{
			return _player.MoveState;
		}
	}

	public override void EnterState()
	{
		_player.PlayAnimation("Idle");
	}

	public override void ExitState()
	{
	}

	public override PlayerStateBase RunState()
	{
		return CheckForSwitchState();
	}
}
