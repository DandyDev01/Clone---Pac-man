using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrightenedState : GhostStateBase
{
	[SerializeField] private Transform[] _pathUpdateTriggers;

	private Vector3 _lastTarget;

	public override GhostStateBase CheckForSwitchState()
	{
		// if player catches, enter dead state
		float distance = Vector3.Distance(_playerTransform.position, transform.position);
		if (distance < 0.5f)
			return _ghost.DeadState;
		else
			return this;
	}

	public override void EnterState()
	{
		_ghost.PlayAnimation("Frightened");
		_ghost.SetSpeedModifier(0.5f);
	}

	public override void ExitState()
	{
		_ghost.SetSpeedModifier(1f);
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
