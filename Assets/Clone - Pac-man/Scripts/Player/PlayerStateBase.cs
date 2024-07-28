using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerStateBase 
{
	protected readonly Player _player;

	public PlayerStateBase(Player player)
	{
		_player = player;
	}

	public abstract PlayerStateBase RunState();

	public abstract void EnterState();

	public abstract void ExitState();

	public abstract PlayerStateBase CheckForSwitchState();
}
