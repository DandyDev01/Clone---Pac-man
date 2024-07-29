using Grid;
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
		_path = _player.Grid.CalculatePath(_player.Spawn, _player.transform.position);
		_currentTarget = _path.First();
		_player.PlayAnimation("Dead");
		_player.GetComponent<Collider2D>().enabled = false;
	}

	public override void ExitState()
	{
		_player.GetComponent<Collider2D>().enabled = true;
	}

	public override PlayerStateBase RunState()
	{
		_player.transform.position = Vector3.MoveTowards(_player.transform.position, _currentTarget._worldPosition, (_player.Speed / 50) * Time.deltaTime);

		return CheckForSwitchState();
	}
}
