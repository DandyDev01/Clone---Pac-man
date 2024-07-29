using Grid;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeGhostChaseState : GhostStateBase
{
	public override GhostStateBase CheckForSwitchState()
	{
		return this;
	}

	public override void EnterState()
	{
		UpdatePath();
	}

	public override void ExitState()
	{
	}

	protected override Vector3 ChooseTargetLocation()
	{
		float distanceFromPlayer = Vector3.Distance(_playerTransform.position, transform.position);
		
		if (distanceFromPlayer > 8f)
		{
			return _playerTransform.position;
		}

		return _ghost.ScatterPoint;
	}
}
