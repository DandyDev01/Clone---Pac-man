using Grid;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class GhostStateBase : MonoBehaviour
{
	protected List<Node> _path;
	protected Transform _playerTransform;
	protected SampleGridXY _grid;
	protected Ghost _ghost;
	protected Node _currentTarget;
	protected int _index = 0;
	private Vector2 _currentDirection = Vector2.zero;

	protected List<GameObject> _markers = new();

	public Action OnTargetReached;

	public void Init(SampleGridXY grid, Ghost ghost)
	{
		_grid = grid;
		_ghost = ghost;
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
		_path = new List<Node>();
	}

	public void UpdatePath()
	{
		foreach (var item in _markers)
		{
			GameObject.Destroy(item.gameObject);
		}

		_markers.Clear();

		List<Node> newPath = _grid.CalculatePath(ChooseTargetLocation(), _ghost.transform.position);

		if (newPath.Count > 0)
		{
			_path.Clear();
			_path.AddRange(newPath);
		}

		_index = 0;

		_currentTarget = _path.First();

		foreach (Node node in _path)
		{
			GameObject marker = GameObject.Instantiate(_ghost.Marker, node._worldPosition, Quaternion.identity);
			_markers.Add(marker);
		}
	}

	public GhostStateBase RunState(Grid.SampleGridXY _grid, Ghost ghost)
	{
		if (_index >= _path.Count)
		{
			OnTargetReached?.Invoke();
			UpdatePath();
			return this;
		}

		_ghost.Move(_currentTarget._worldPosition);

		if (Vector3.Distance(_ghost.transform.position, _currentTarget._worldPosition) < 0.05f)
		{
			_index += 1;

			if (_index >= _path.Count)
			{
				OnTargetReached?.Invoke();
				UpdatePath();
				return this;
			}

			ChooseAnimation();

			_currentTarget = _path[_index];
		}

		if (Vector3.Distance(ghost.transform.position, _path[_path.Count - 1]._worldPosition) < 0.05f)
		{
			OnTargetReached?.Invoke();
			UpdatePath();
		}

		return CheckForSwitchState();
	}

	protected abstract Vector3 ChooseTargetLocation();

    public abstract void EnterState();

    public abstract void ExitState();

    public abstract GhostStateBase CheckForSwitchState();

	private void ChooseAnimation()
	{
		if (this is DeadState || this is FrightenedState)
			return;

		Vector2 direction = _ghost.CalculateMoveDirection();

		if (direction == Vector2.up)
		{
			_ghost.PlayAnimation("Move_Up");
		}
		else if (direction == Vector2.down)
		{
			_ghost.PlayAnimation("Move_Down");
		}
		else if (direction == Vector2.right)
		{
			_ghost.PlayAnimation("Move_Right");
		}
		else if (direction == Vector2.left)
		{
			_ghost.PlayAnimation("Move_Left");
		}
	}
}
