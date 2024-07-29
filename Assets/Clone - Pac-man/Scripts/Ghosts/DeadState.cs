using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : GhostStateBase
{
	public override GhostStateBase CheckForSwitchState()
	{
		throw new System.NotImplementedException();
	}

	public override void EnterState()
	{
		_ghost.PlayAnimation("Dead");
	}

	public override void ExitState()
	{
		throw new System.NotImplementedException();
	}

	protected override Vector3 ChooseTargetLocation()
	{
		throw new System.NotImplementedException();
	}
}
