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
			List<Node> newPath = _grid.CalculatePath(_playerTransform.position, _ghost.transform.position);

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

	protected abstract void ChooseTargetLocation();

    public abstract void EnterState();

    public abstract void ExitState();

    public abstract GhostStateBase RunState(Grid.SampleGridXY _grid, Ghost ghost);

    public abstract GhostStateBase CheckForSwitchState();

}
