using Grid;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueGhostChaseState : GhostStateBase
{
	[SerializeField] private Ghost _redGhost;

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
		//StopCoroutine(PathUpdater());
	}

	protected override Vector3 ChooseTargetLocation()
	{
		Rigidbody2D playerRigidBody = _playerTransform.GetComponent<Rigidbody2D>();
		Vector2 playerVelocity = playerRigidBody.velocity;
		Vector2 playerMoveDirection = playerVelocity.normalized;

		Vector3 target = _playerTransform.position + (Vector3)(playerMoveDirection * 2);

		if (_grid.Grid.IsInRange(target) == false)
			target = _playerTransform.position;

		Vector2 direction = target - _redGhost.transform.position;
		Vector2 normalizedDirection = direction.normalized;
		float doubleLength = direction.magnitude * 2;
		target = normalizedDirection * doubleLength;

		if (_grid.Grid.IsInRange(target) == false)
			target = _playerTransform.position;

		return target;
	}
}
