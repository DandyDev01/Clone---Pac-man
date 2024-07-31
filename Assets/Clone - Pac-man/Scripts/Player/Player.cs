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
	private PathBuilder _pathBuilder;
	private Vector2 _currentDirection;
	private float _speedModifier = 1f;

	public PlayerInput Input { get; private set; }
	public SampleGridXY Grid => _grid;
	public PlayerStateBase IdleState { get; private set; }
	public PlayerStateBase MoveState { get; private set; }
	public PlayerStateBase DeadState { get; private set; }
	public PathBuilder PathBuilder => _pathBuilder;
	public bool IsInvincible { get; set; }
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
		DeadState = new PlayerDeadState(this);

		_currentState = IdleState;
		_currentState.EnterState();

		_currentDirection = Vector2.zero;
	}

	private void Start()
	{
		MoveState = new PlayerMoveState(this, _grid.Grid);
		_pathBuilder = new PathBuilder(_grid.Grid, _grid.Marker);
	}

	public void RunCurrentState()
	{
		PlayerStateBase nextState = _currentState.RunState();

		if (nextState != _currentState)
		{
			SwitchState(nextState);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.transform.tag == "Ghost" && IsInvincible == false)
		{
			GhostStateBase state = collision.GetComponent<Ghost>().CurrentState;
			if (state is FrightenedState || state is DeadState)
				return; 

			HasBeenHit = true;
			OnDealth?.Invoke();
			SwitchState(DeadState);
		}
	}

	/// <summary>
	/// Move the player in a direction.
	/// </summary>
	/// <param name="direction">Direction to move the player.</param>
	public void Move(Vector2 direction)
    {
		// makes it so player is always moving after input is registered.
		if (direction == Vector2.zero) 
			return;

		if (direction != _currentDirection)
		{
			CenterToCell(direction);
			_currentDirection = direction;
		}

        _rigidbody.velocity = Time.deltaTime * _speed * _speedModifier * direction;
    }

	private void CenterToCell(Vector2 direction)
	{
		Vector3 target = _grid.Grid.GetCellPosition(transform.position);
		target = _grid.Grid.GetWorldPosition((int)target.x, (int)target.y);
		transform.position = target;
	}

	/// <summary>
	/// Set the z rotation of the player.
	/// </summary>
	/// <param name="angle">Angle to set the z rotation.</param>
	public void Rotate(float angle)
	{
		Transform transform = _spriteRenderer.transform;
		transform.eulerAngles = new Vector3(0, 0, angle);
	}

	/// <summary>
	/// Update the speed modifier of the player.
	/// </summary>
	/// <param name="value">Value of the speed modifier</param>
	public void SetSpeedModifier(float value)
	{
		_speedModifier = value;
	}

	/// <summary>
	/// Play an animation
	/// </summary>
	/// <param name="animationName">Name of animation</param>
	public void PlayAnimation(string animationName)
	{
		_animator.Play(animationName);
	}

	public void Pickup(int amount)
	{
		OnPickup?.Invoke(amount);
	}

	public void SwitchState(PlayerStateBase newState)
	{
		_currentState.ExitState();
		_currentState = newState;
		_currentState.EnterState();
	}
}
