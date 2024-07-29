using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrightenedState : GhostStateBase
{
	[SerializeField] private Transform[] _pathUpdateTriggers;

	private Vector3 _lastTarget;

	public override GhostStateBase CheckForSwitchState()
	{
		return this;
	}

	public override void EnterState()
	{
		_ghost.PlayAnimation("Frightened");
	}

	public override void ExitState()
	{
	}

	protected override Vector3 ChooseTargetLocation()
	{
		Vector3 target = Vector3.zero;
		do
		{
			target = _pathUpdateTriggers[Random.Range(0, _pathUpdateTriggers.Length - 1)].position;
		}
		while (target == _lastTarget);

		_lastTarget = target;
		return target;
	}
}
