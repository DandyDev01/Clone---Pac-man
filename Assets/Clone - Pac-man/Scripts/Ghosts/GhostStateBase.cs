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

	private List<GameObject> _markers = new();

	public Action OnTargetReached;

	public void Init(SampleGridXY grid, Ghost ghost)
	{
		_grid = grid;
		_ghost = ghost;
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
		_path = new List<Node>();
	}

	protected IEnumerator PathUpdater()
	{
		List<GameObject> markers = new();
		while (true)
		{
			List<Node> newPath = _grid.CalculatePath(ChooseTargetLocation(), _ghost.transform.position);

			if (newPath.Count > 0)
			{
				_path.Clear();
				_path.AddRange(newPath);

			}

			foreach (Node node in _path)
			{
				GameObject marker = GameObject.Instantiate(_ghost.Marker, node._worldPosition, Quaternion.identity);
				markers.Add(marker);
			}

			_index = 0;
			Debug.Log("update");

			_currentTarget = _path.First();

			yield return new WaitForSeconds(1f);

			foreach (var item in markers)
			{
				GameObject.Destroy(item.gameObject);
			}

			markers.Clear();
		}
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
			return this;
		}

		_ghost.Move(_currentTarget._worldPosition);

		if (Vector3.Distance(_ghost.transform.position, _currentTarget._worldPosition) < 0.05f)
		{
			_index += 1;

			if (_index >= _path.Count)
			{
				OnTargetReached?.Invoke();
				return this;
			}

			_currentTarget = _path[_index];
		}

		if (ghost.transform.position.Approx(_path[_path.Count - 1]._worldPosition))
		{
			OnTargetReached?.Invoke();
		}

		return this;
	}

	protected abstract Vector3 ChooseTargetLocation();

    public abstract void EnterState();

    public abstract void ExitState();

    public abstract GhostStateBase CheckForSwitchState();

}
