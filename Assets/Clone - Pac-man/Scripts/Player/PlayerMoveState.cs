using System;
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
		_player.PlayAnimation("Move");
	}

	public override void ExitState()
	{
	}

	public override PlayerStateBase RunState()
	{
		_player.Move(_player.Input.currentActionMap.actions[0].ReadValue<Vector2>());
		_player.Rotate(MovementDirectionToRotation());

		return CheckForSwitchState();
	}

	private float MovementDirectionToRotation()
	{
		Vector2 direction = _player.Input.currentActionMap.actions[0].ReadValue<Vector2>();
		if (direction.x > 0)
		{
			return 0;
		} 
		else if (direction.y > 0) 
		{
			return 90;
		}
		else if (direction.x < 0)
		{
			return -180;
		}
		else if (direction.y < 0)
		{
			return 270;
		}

		return 0;
	}
}
