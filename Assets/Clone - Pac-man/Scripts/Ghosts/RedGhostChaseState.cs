using Grid;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RedGhostChaseState : GhostStateBase
{
	public RedGhostChaseState(SampleGridXY grid, Ghost ghost) : base(grid, ghost)
	{
	}

	public override GhostStateBase CheckForSwitchState()
	{
		throw new System.NotImplementedException();
	}

	public override void EnterState()
	{
		_currentTarget = new Node(_playerTransform.position);
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
		return _playerTransform.position;
	}

	
}
