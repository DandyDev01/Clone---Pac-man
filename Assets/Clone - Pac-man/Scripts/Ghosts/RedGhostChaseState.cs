using Grid;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RedGhostChaseState : GhostStateBase
{
	public override GhostStateBase CheckForSwitchState()
	{
		throw new System.NotImplementedException();
	}

	public override void EnterState()
	{
		 _ghost.StartCoroutine(PathUpdater());
	}

	public override void ExitState()
	{
		_ghost.StopCoroutine(PathUpdater());
	}

	protected override Vector3 ChooseTargetLocation()
	{
		return _playerTransform.position;
	}

	
}
