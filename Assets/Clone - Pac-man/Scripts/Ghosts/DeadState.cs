using Grid;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeadState : GhostStateBase
{
	[SerializeField] private Transform _ghostHouse;

	private Timer _timer;

	private void Awake()
	{
		_timer = new Timer(10f);
	}

	private void Update()
	{
		_timer.Tick(Time.deltaTime);
	}

	public override GhostStateBase CheckForSwitchState()
	{
		if (_timer.Finished)
			return _ghost.ChaseState;
		else 
			return this;
	}

	public override void EnterState()
	{
		List<Node> pathToGhostHouse = _grid.CalculatePath(_ghostHouse.position, transform.position);

		_path.AddRange(pathToGhostHouse);

		foreach (Node node in pathToGhostHouse)
		{
			GameObject g = Instantiate(_ghost.Marker, node._worldPosition, Quaternion.identity);
			_markers.Add(g);
		}

		_currentTarget = _path.First();

		_timer.Play();

		_ghost.PlayAnimation("Dead");
	}

	public override void ExitState()
	{
		_timer.Stop();
		_timer.Reset(10f);

		foreach (var item in _markers)
		{
			GameObject.Destroy(item.gameObject);
		}

		_markers.Clear();
	}

	protected override Vector3 ChooseTargetLocation()
	{
		return Vector3.zero;
	}
}
