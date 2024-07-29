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
		if (_index >= _path.Count && _timer.IsPlaying == false)
			_timer.Play();

		if (_timer.Finished)
			return _ghost.ChaseState;
		else 
			return this;
	}

	public override void EnterState()
	{
		List<Node> pathToGhostHouse = _grid.CalculatePath(_ghostHouse.position, _ghost.transform.position);

		_path.AddRange(pathToGhostHouse);

		_currentTarget = _path.First();

		_index = 0;

		_ghost.PlayAnimation("Dead");
	}

	public override void ExitState()
	{
		_timer.Stop();
		_timer.Reset(10f);
	}

	protected override Vector3 ChooseTargetLocation()
	{
		throw new System.Exception("Target location error");
	}
}
