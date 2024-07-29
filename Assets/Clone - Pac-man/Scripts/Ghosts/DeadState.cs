using Grid;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeadState : GhostStateBase
{
	[SerializeField] private Transform _ghostHouse;

	public override GhostStateBase CheckForSwitchState()
	{
		return this;
	}

	public override void EnterState()
	{
		List<Node> pathToGhostHouse = _grid.CalculatePath(_ghostHouse.position, transform.position);

		_path.AddRange(pathToGhostHouse);

		foreach (Node node in pathToGhostHouse)
		{
			GameObject g = Instantiate(_ghost.Marker, node._worldPosition, Quaternion.identity);
			_markers.Add(g);
		}

		_currentTarget = _path.First();

		_ghost.PlayAnimation("Dead");
	}

	public override void ExitState()
	{
		foreach (var item in _markers)
		{
			GameObject.Destroy(item.gameObject);
		}

		_markers.Clear();
	}

	protected override Vector3 ChooseTargetLocation()
	{
		return Vector3.zero;
	}
}
