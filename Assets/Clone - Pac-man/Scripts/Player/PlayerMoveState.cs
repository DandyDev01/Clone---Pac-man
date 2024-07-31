using Grid;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerStateBase
{
	private readonly GridXY<bool> _grid;

	private Vector2 _currentDirection;
	private float _currentAngle;

	private Vector2 _direction;

	public PlayerMoveState(Player player, GridXY<bool> grid) : base(player)
	{
		_currentDirection = Vector2.zero;
		_grid = grid;
	}

	public override PlayerStateBase CheckForSwitchState()
	{
		return this;
	}

	public override void EnterState()
	{
		_player.PlayAnimation("Move");
	}

	public override void ExitState()
	{
		_currentDirection = Vector2.zero;
	}

	public override PlayerStateBase RunState()
	{
		_direction = _player.Input.currentActionMap.actions[0].ReadValue<Vector2>();

		if (_grid.GetElement(_player.transform.position + (Vector3)_direction) == false)
			return CheckForSwitchState();

		_player.Rotate(MovementDirectionToRotation(_direction));
		
		_currentDirection = _direction;

		_player.Move(_currentDirection);

		return CheckForSwitchState();
	}

	private float MovementDirectionToRotation(Vector2 direction)
	{
		// keep player rotation the same until thy start moving in a different direction
		if (direction == _currentDirection)
			return _currentAngle;

		if (direction.x > 0)
		{
			_currentAngle = 0;
		} 
		else if (direction.y > 0) 
		{
			_currentAngle = 90;
		}
		else if (direction.x < 0)
		{
			_currentAngle = -180;
		}
		else if (direction.y < 0)
		{
			_currentAngle = 270;
		}

		return _currentAngle;
	}
}
