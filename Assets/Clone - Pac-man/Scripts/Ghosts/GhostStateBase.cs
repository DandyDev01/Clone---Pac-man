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
	private Vector2 _lastDirection = Vector2.zero;

	public Action OnTargetReached;

	/// <summary>
	/// Initilize dependancies
	/// </summary>
	/// <param name="grid">Grid that is used for pathfinding.</param>
	/// <param name="ghost">Ghost this state belongs too.</param>
	public void Init(SampleGridXY grid, Ghost ghost)
	{
		_grid = grid;
		_ghost = ghost;
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
		_path = new List<Node>();
	}

	/// <summary>
	/// Calculate a path from the Ghost that owns this to their target.
	/// </summary>
	public void UpdatePath()
	{
		List<Node> newPath = _grid.CalculatePath(ChooseTargetLocation(), _ghost.transform.position);

		if (newPath.Count > 0)
		{
			_path.Clear();
			_path.AddRange(newPath);
		}

		_index = 0;

		if (_path.Count > 0)
			_currentTarget = _path.First();
	}

	/// <summary>
	/// Follow path to target.
	/// </summary>
	/// <returns></returns>
	public GhostStateBase RunState()
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

		if (Vector3.Distance(_ghost.transform.position, _path[_path.Count - 1]._worldPosition) < 0.05f)
		{
			OnTargetReached?.Invoke();
			UpdatePath();
		}

		return CheckForSwitchState();
	}

	/// <summary>
	/// Calculate where the target position is.
	/// </summary>
	/// <returns>World position to move to.</returns>
	protected abstract Vector3 ChooseTargetLocation();

    public abstract void EnterState();

    public abstract void ExitState();

    public abstract GhostStateBase CheckForSwitchState();

	/// <summary>
	/// Chooses which directional movement animation to play
	/// </summary>
	private void ChooseAnimation()
	{
		if (this is DeadState || this is FrightenedState)
			return;

		Vector2 direction = _ghost.CalculateMoveDirection();

		if (direction == _lastDirection)
			return;

		_lastDirection = _currentDirection;
		_currentDirection = direction;

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
