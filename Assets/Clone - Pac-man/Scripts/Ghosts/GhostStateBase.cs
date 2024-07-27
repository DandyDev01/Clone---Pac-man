using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GhostStateBase : MonoBehaviour
{
    protected Vector2 _targetPosition;
    protected Transform _playerTransform;

	private void Awake()
	{
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
	}

	protected abstract void ChooseTargetLocation();

    public abstract void EnterState();

    public abstract void ExitState();

    public abstract GhostStateBase RunState();

    public abstract GhostStateBase CheckForSwitchState();
}
