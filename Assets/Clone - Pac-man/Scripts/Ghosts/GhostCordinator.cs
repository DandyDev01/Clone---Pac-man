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
		_blueGhost.enabled = false;
		_orangeGhost.enabled = false;
		_pinkGhost.enabled = false;

		_redReleaseTimer = new Timer(0f, false);
		_blueReleaseTimer = new Timer(1005f);
		_orangeReleaseTimer = new Timer(1000f);
		_pinkReleaseTimer = new Timer(1005f);

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
		_redGhost.SwitchState(_redGhost.ScatterState);
		_blueGhost.SwitchState(_blueGhost.ScatterState);
		_orangeGhost.SwitchState(_orangeGhost.ScatterState);
		_pinkGhost.SwitchState(_pinkGhost.ScatterState);
	}

	public void ChaseMode()
	{
		_redGhost.SwitchState(_redGhost.ChaseState);
		_blueGhost.SwitchState(_blueGhost.ChaseState);
		_orangeGhost.SwitchState(_orangeGhost.ChaseState);
		_pinkGhost.SwitchState(_pinkGhost.ChaseState);
	}

	public void EnableRed()
	{
		_redGhost.enabled = true;
	}

	public void EnableBlue()
	{
		_blueGhost.enabled = true;
	}

	public void EnableOrange()
	{
		_orangeGhost.enabled = true;
	}

	public void EnablePink()
	{
		_pinkGhost.enabled = true;
	}
}
