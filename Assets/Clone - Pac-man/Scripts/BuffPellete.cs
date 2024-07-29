using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffPellete : Collectable
{
	private Player _player;
	private Timer _timer;
	GhostCordinator _ghostCordinator;

	private void Awake()
	{
		_timer = new Timer(10);
		_timer.OnTimerEnd += End;
		_player = FindAnyObjectByType<Player>();
		 _ghostCordinator = FindObjectOfType<GhostCordinator>();
	}

	private void Update()
	{
		_timer.Tick(Time.deltaTime);
	}

	protected override void Collect(Collider2D collision)
	{
		_ghostCordinator.FrightenedMode();

		_timer.Play();
		_player.SetSpeedModifier(1.5f);
	}

	private void End()
	{
		_timer.Stop();
		_timer.Reset(10);
		_player.SetSpeedModifier(1f);
		_ghostCordinator.ChaseMode();
	}
}
