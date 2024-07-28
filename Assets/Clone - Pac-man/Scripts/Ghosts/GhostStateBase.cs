using Grid;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class GhostStateBase
{
	protected readonly List<Node> _path;
	protected readonly Transform _playerTransform;
	protected readonly SampleGridXY _grid;
	protected readonly Ghost _ghost;
	protected Node _currentTarget;
	protected int _index = 0;

	public GhostStateBase(SampleGridXY grid, Ghost ghost)
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
   
	public GhostStateBase RunState(Grid.SampleGridXY _grid, Ghost ghost)
	{
		if (_index >= _path.Count)
			return this;

		_ghost.Move(_currentTarget._worldPosition);

		if (ghost.transform.position == _currentTarget._worldPosition)
		{
			_index += 1;

			if (_index >= _path.Count)
				return this;

			_currentTarget = _path[_index];
		}

		return this;
	}

	protected abstract Vector3 ChooseTargetLocation();

    public abstract void EnterState();

    public abstract void ExitState();


    public abstract GhostStateBase CheckForSwitchState();

}
