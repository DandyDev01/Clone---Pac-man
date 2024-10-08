using Grid;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinkGhostChaseState : GhostStateBase
{
	public override GhostStateBase CheckForSwitchState()
	{
		return this;
	}

	public override void EnterState()
	{
		UpdatePath();
	}

	public override void ExitState()
	{
	}

	protected override Vector3 ChooseTargetLocation()
	{
		Rigidbody2D playerRigidBody = _playerTransform.GetComponent<Rigidbody2D>();	
		Vector2 playerVelocity = playerRigidBody.velocity;
		Vector2 playerMoveDirection = playerVelocity.normalized;

		Vector3 target = _playerTransform.position + (Vector3)(playerMoveDirection * 4);

		if (_grid.Grid.IsInRange(target) == false)
			return _playerTransform.position;

		if (_grid.Grid.GetElement(target) == false)
		{
			return _playerTransform.position;
		}

		return target;
	}
}
