using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedGhostChaseState : GhostStateBase
{
	public override GhostStateBase CheckForSwitchState()
	{
		throw new System.NotImplementedException();
	}

	public override void EnterState()
	{
		throw new System.NotImplementedException();
	}

	public override void ExitState()
	{
		throw new System.NotImplementedException();
	}

	public override GhostStateBase RunState()
	{
		throw new System.NotImplementedException();
	}

	protected override void ChooseTargetLocation()
	{
		_targetPosition = _playerTransform.position;
	}
}
