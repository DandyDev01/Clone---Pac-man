using Grid;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed;

	private Transform _spawn;
	private SampleGridXY _grid;
	private Animator _animator;
	private SpriteRenderer _spriteRenderer;
	private Rigidbody2D _rigidbody;
	private PlayerStateBase _currentState;

	public PlayerInput Input { get; private set; }
	public SampleGridXY Grid => _grid;
	public PlayerStateBase IdleState { get; private set; }
	public PlayerStateBase MoveState { get; private set; }
	public PlayerStateBase DeadState { get; private set; }
	public Vector3 Spawn => _spawn.position;
	public float Speed => _speed;
	public bool HasBeenHit { get; set; }

    public Action OnDealth;
	public Action<int> OnPickup;

	private void Awake()
	{
		_spriteRenderer = GetComponentInChildren<SpriteRenderer>();	
		_animator = GetComponentInChildren<Animator>();
		_rigidbody = GetComponent<Rigidbody2D>();   
		Input = GetComponent<PlayerInput>();
		_spawn = GameObject.FindGameObjectWithTag("Respawn").transform;
		_grid = FindObjectOfType<SampleGridXY>();

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

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.transform.tag == "Ghost")
		{
			if (collision.GetComponent<Ghost>().CurrentState is FrightenedState)
				return; 

			HasBeenHit = true;
			OnDealth?.Invoke();
			SwitchState(DeadState);
		}
	}

	public void Move(Vector2 direction)
    {
        _rigidbody.velocity = Time.deltaTime * _speed * direction;
    }

	public void Rotate(float angle)
	{
		Transform transform = _spriteRenderer.transform;
		transform.eulerAngles = new Vector3(0, 0, angle);
	}

	public void PlayAnimation(string animationName)
	{
		_animator.Play(animationName);
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
