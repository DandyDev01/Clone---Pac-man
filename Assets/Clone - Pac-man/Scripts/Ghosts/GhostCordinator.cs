using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GhostCordinator : MonoBehaviour
{
    [SerializeField] private Ghost _redGhost;
    [SerializeField] private Ghost _blueGhost;
    [SerializeField] private Ghost _orangeGhost;
    [SerializeField] private Ghost _pinkGhost;

	private Timer _redReleaseTimer;
	private Timer _blueReleaseTimer;
	private Timer _orangeReleaseTimer;
	private Timer _pinkReleaseTimer;

	private void Awake()
	{
		_blueGhost.SetSpeedModifier(0f);
		_orangeGhost.SetSpeedModifier(0f);
		_pinkGhost.SetSpeedModifier(0f);

		_redReleaseTimer = new Timer(0f, false);
		_blueReleaseTimer = new Timer(5f);
		_orangeReleaseTimer = new Timer(10);
		_pinkReleaseTimer = new Timer(15);

		_blueReleaseTimer.Play();
		_orangeReleaseTimer.Play();
		_pinkReleaseTimer.Play();

		_redReleaseTimer.OnTimerEnd += EnableRed;
		_blueReleaseTimer.OnTimerEnd += EnableBlue;
		_orangeReleaseTimer.OnTimerEnd += EnableOrange;
		_pinkReleaseTimer.OnTimerEnd += EnablePink;
	}

	private void Update()
	{
		_redReleaseTimer.Tick(Time.deltaTime);
		_blueReleaseTimer.Tick(Time.deltaTime);
		_orangeReleaseTimer.Tick(Time.deltaTime);
		_pinkReleaseTimer.Tick(Time.deltaTime);
	}

	public void FrightenedMode()
	{
		if (_redGhost.CurrentState is not DeadState)
			_redGhost.SwitchState(_redGhost.FrightenedState);

		if (_blueGhost.CurrentState is not DeadState)
			_blueGhost.SwitchState(_blueGhost.FrightenedState);

		if (_orangeGhost.CurrentState is not DeadState)
			_orangeGhost.SwitchState(_orangeGhost.FrightenedState);

		if (_pinkGhost.CurrentState is not DeadState)
			_pinkGhost.SwitchState(_pinkGhost.FrightenedState);
	}

	public void ScatterMode()
	{
		if (_redGhost.CurrentState is not DeadState)
			_redGhost.SwitchState(_redGhost.ScatterState);

		if (_blueGhost.CurrentState is not DeadState)
			_blueGhost.SwitchState(_blueGhost.ScatterState);

		if (_orangeGhost.CurrentState is not DeadState)
			_orangeGhost.SwitchState(_orangeGhost.ScatterState);

		if (_pinkGhost.CurrentState is not DeadState)
			_pinkGhost.SwitchState(_pinkGhost.ScatterState);
	}

	public void ChaseMode()
	{
		if (_redGhost.CurrentState is not DeadState)
			_redGhost.SwitchState(_redGhost.ChaseState);

		if (_blueGhost.CurrentState is not DeadState)
			_blueGhost.SwitchState(_blueGhost.ChaseState);

		if (_orangeGhost.CurrentState is not DeadState)
			_orangeGhost.SwitchState(_orangeGhost.ChaseState);

		if (_pinkGhost.CurrentState is not DeadState)
			_pinkGhost.SwitchState(_pinkGhost.ChaseState);
	}

	public void EnableRed()
	{
		_redGhost.SetSpeedModifier(1);
	}

	public void EnableBlue()
	{
		_blueGhost.SetSpeedModifier(1);
	}

	public void EnableOrange()
	{
		_orangeGhost.SetSpeedModifier(1);
	}

	public void EnablePink()
	{
		_pinkGhost.SetSpeedModifier(1);
	}
}
