using Grid;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinkGhostChaseState : GhostStateBase
{
	public PinkGhostChaseState(SampleGridXY grid, Ghost ghost) : base(grid, ghost)
	{
	}

	public override GhostStateBase CheckForSwitchState()
	{
		throw new System.NotImplementedException();
	}

	public override void EnterState()
	{
		_ghost.StartCoroutine(PathUpdater());
	}

	public override void ExitState()
	{
		_ghost.StopCoroutine(PathUpdater());
	}

	public override GhostStateBase RunState(Grid.SampleGridXY _grid, Ghost ghost)
	{
		if (_index >= _path.Count)
			return this;

		ghost.transform.position = Vector2.MoveTowards(ghost.transform.position, _currentTarget._worldPosition,
			ghost.Speed * Time.deltaTime);

		if (ghost.transform.position == _currentTarget._worldPosition)
		{
			_index += 1;

			if (_index >= _path.Count)
				return this;

			_currentTarget = _path[_index];
		}

		return this;
	}

	protected override Vector3 ChooseTargetLocation()
	{
		Rigidbody2D playerRigidBody = _playerTransform.GetComponent<Rigidbody2D>();	
		Vector2 playerVelocity = playerRigidBody.velocity;
		Vector2 playerMoveDirection = playerVelocity.normalized;

		Vector3 target = _playerTransform.position + (Vector3)(playerMoveDirection * 4);

		if (_grid.Grid.IsInRange(target) == false)
			return _playerTransform.position;

		return target;
	}
}
