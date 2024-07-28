using Grid;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScatterState : GhostStateBase
{
	[SerializeField] Transform[] _homePath;

	public override GhostStateBase CheckForSwitchState()
	{
		throw new System.NotImplementedException();
	}

	public override void EnterState()
	{
		List<Node> pathToHome = _grid.CalculatePath(_homePath.First().position, _ghost.transform.position);

		_path.AddRange(pathToHome);

		_currentTarget = _path.First();

		OnTargetReached += UpdatePath;
	}


	public override void ExitState()
	{
		OnTargetReached -= UpdatePath;
	}

	protected override Vector3 ChooseTargetLocation()
	{
		throw new System.NotImplementedException();
	}
	
	private void UpdatePath()
	{
		_path.Clear();
		foreach (var item in _homePath)
		{
			_path.Add(new Node(item.position));
		}

		_currentTarget = _path.First();
		_index = 0;
	}
}
