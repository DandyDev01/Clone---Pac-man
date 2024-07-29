using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffPellete : Collectable
{
	private Timer _timer = new Timer(10f, false);
	private Player _player;
	private GhostCordinator _ghostCordinator;
	private CircleCollider2D _circleCollider;
	private SpriteRenderer _spriteRenderer;

	private void Awake()
	{
		_player = FindAnyObjectByType<Player>();
		_ghostCordinator = FindObjectOfType<GhostCordinator>();
		_spriteRenderer = GetComponent<SpriteRenderer>();
		_circleCollider = GetComponent<CircleCollider2D>();

		_timer.OnTimerEnd += End;
	}

	private void Update()
	{
		_timer.Tick(Time.deltaTime);
	}

	protected override void Collect(Collider2D collision)
	{
		_ghostCordinator.FrightenedMode();

		Disable();

		_timer.Reset(10f);
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

	public void Enable()
	{
		_spriteRenderer.enabled = true;
		_circleCollider.enabled = true;
	}

	private void Disable()
	{
		_spriteRenderer.enabled = false;
		_circleCollider.enabled = false;
	}
}
