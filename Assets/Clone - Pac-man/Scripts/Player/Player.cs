using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed;
	public PlayerInput Input { get; private set; }

	private Rigidbody2D _rigidbody;
	private PlayerStateBase _currentState;

	public PlayerStateBase IdleState { get; private set; }
	public PlayerStateBase MoveState { get; private set; }
	public PlayerStateBase DeadState { get; private set; }
	public bool HasBeenHit { get; set; }

    public Action OnDealth;
	public Action<int> OnPickup;


	private void Awake()
	{
		_rigidbody = GetComponent<Rigidbody2D>();   
		Input = GetComponent<PlayerInput>();

		IdleState = new PlayerIdleState(this);
		MoveState = new PlayerMoveState(this);
		DeadState = new PlayerDeadState(this);

		_currentState = IdleState;
		_currentState.EnterState();
	}

	private void Update()
	{
		PlayerStateBase nextState = _currentState.RunState();

		if (nextState != _currentState)
		{
			SwitchState(nextState);
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.transform.tag == "Ghost")
		{
			HasBeenHit = true;
			OnDealth?.Invoke();
		}
	}

	public void Move(Vector2 direction)
    {
        _rigidbody.velocity = Time.deltaTime * _speed * direction;
    }

	public void Pickup(int amount)
	{
		OnPickup?.Invoke(amount);
	}

	internal void SwitchState(PlayerStateBase newState)
	{
		_currentState.ExitState();
		_currentState = newState;
		_currentState.EnterState();
	}
}
