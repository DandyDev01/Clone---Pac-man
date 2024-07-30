using Grid;
using PlasticGui;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerDeadState : PlayerStateBase
{
	private List<Node> _path;
	private Node _currentTarget;
	private int _index = 0;

	public PlayerDeadState(Player player) : base(player)
	{
	}

	public override PlayerStateBase CheckForSwitchState()
	{
		if (Vector3.Distance(_player.transform.position, _currentTarget._worldPosition) < 0.05f)
		{
			_index += 1;

			if (_index >= _path.Count)
			{
				return _player.IdleState;
			}

			_currentTarget = _path[_index];
		}

		return this;
	}

	public override void EnterState()
	{
		_path = _player.PathBuilder.CalculatePath(_player.Spawn, _player.transform.position);
		_player.PlayAnimation("Dead");
		_player.GetComponent<Collider2D>().enabled = false;

		if (_path.Count <= 0)
			_player.SwitchState(_player.IdleState);
		else
			_currentTarget = _path.First();
	}

	public override void ExitState()
	{
		_player.StartCoroutine(ResetCoroutine());

		_player.GetComponent<Collider2D>().enabled = true;
		_player.HasBeenHit = false;
		_player.IsInvincible = true;

		_index = 0;
	}

	public override PlayerStateBase RunState()
	{
		_player.transform.position = Vector3.MoveTowards(_player.transform.position, _currentTarget._worldPosition, (_player.Speed / 50) * Time.deltaTime);

		return CheckForSwitchState();
	}

	private IEnumerator ResetCoroutine()
	{
		yield return new WaitForSeconds(5f);
		_player.IsInvincible = false;

	}
}
