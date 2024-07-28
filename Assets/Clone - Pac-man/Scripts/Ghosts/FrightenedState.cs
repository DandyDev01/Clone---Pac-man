using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrightenedState : GhostStateBase
{
	public override GhostStateBase CheckForSwitchState()
	{
		throw new System.NotImplementedException();
	}

	public override void EnterState()
	{
	}

	public override void ExitState()
	{
	}

	protected override Vector3 ChooseTargetLocation()
	{
		// pick a psuedorandom location.
		return Vector3.zero;
	}
}
