using Grid;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScatterState : GhostStateBase
{
	[SerializeField] Transform[] _homePath;

	public Transform[] HomePath => _homePath;

	private Timer _timer;

	private void Awake()
	{
		_timer = new Timer(10f);
	}

	private void Update()
	{
		_timer.Tick(Time.deltaTime);
	}

	public override GhostStateBase CheckForSwitchState()
	{
		if (_timer.Finished)
			return _ghost.ChaseState;
		else
			return this;
	}

	public override void EnterState()
	{
		List<Node> pathToHome = _grid.CalculatePath(_homePath.First().position, _ghost.transform.position);

		_path.AddRange(pathToHome);

		_timer.Play();

		_currentTarget = _path.First();

		OnTargetReached += SetPathToHomeRoute;
	}

	public override void ExitState()
	{
		_timer.Stop();
		_timer.Reset(10f);

		foreach (var item in _markers)
		{
			GameObject.Destroy(item.gameObject);
		}

		_markers.Clear();

		OnTargetReached -= SetPathToHomeRoute;
	}

	protected override Vector3 ChooseTargetLocation()
	{
		return _homePath.First().position;
	}
	
	private void SetPathToHomeRoute()
	{
		_path.Clear();
		foreach (var item in _homePath)
		{
			Vector2 cell = _grid.Grid.GetCellPosition(item.position);
			Vector3 world = _grid.Grid.GetWorldPosition((int)cell.x, (int)cell.y);
			item.transform.position = world;
			_path.Add(new Node(world));
		}

		_currentTarget = _path.First();
		_index = 0;
	}
}
