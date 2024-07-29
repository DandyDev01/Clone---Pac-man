using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffPellete : Collectable
{
	private Player _player;
	private Timer _timer;

	private void Awake()
	{
		_timer = new Timer(10);
		_timer.OnTimerEnd += End;
		_player = FindAnyObjectByType<Player>();
	}

	private void Update()
	{
		_timer.Tick(Time.deltaTime);
	}

	protected override void Collect(Collider2D collision)
	{
		GhostCordinator ghostCordinator = FindObjectOfType<GhostCordinator>();
		ghostCordinator.FrightenedMode();

		_timer.Play();
		_player.SetSpeedModifier(1.5f);
	}

	private void End()
	{
		_timer.Stop();
		_timer.Reset(10);
		_player.SetSpeedModifier(1f);
	}
}
