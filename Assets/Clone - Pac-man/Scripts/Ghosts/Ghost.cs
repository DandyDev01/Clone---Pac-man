using Grid;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    [SerializeField] private float _speed;

	[Header("States")]
	[SerializeField] private GhostStateBase _chaseState;
	private ScatterState _scatterState;
	private FrightenedState _frightenedState;
	private DeadState _deadState;

	private Transform _playerTransform;
    private SampleGridXY _grid;
	private GhostStateBase _currentState;
	private Rigidbody2D _rigidbody;
	private Animator _animator;
	private float _speedModifier = 1;

	public GhostStateBase CurrentState => _currentState;
	public GhostStateBase ScatterState => _scatterState;
	public GhostStateBase FrightenedState => _frightenedState;
	public GhostStateBase ChaseState => _chaseState;
	public GhostStateBase DeadState => _deadState;
	public Vector3 ScatterPoint => _scatterState.HomePath.First().position;
	public float Speed => _speed;
	public PathUpdateTrigger LastUpdateTrigger { get; set; }
	public PathUpdateTrigger CurrentUpdateTrigger { get; set; }

	private void Awake()
	{
		_rigidbody = GetComponent<Rigidbody2D>();
		_animator = GetComponentInChildren<Animator>();

		_grid = FindObjectOfType<SampleGridXY>();
		_playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
		_frightenedState = GetComponentInChildren<FrightenedState>();
		_scatterState = GetComponentInChildren<ScatterState>();
		_deadState = GetComponentInChildren<DeadState>();

		_chaseState.Init(_grid, this);
		_scatterState.Init(_grid, this);
		_frightenedState.Init(_grid, this);	
		_deadState.Init(_grid, this);

		_currentState = _chaseState;
	}

	private void Start()
	{
		_currentState.EnterState();
	}

	private void Update()
	{
		GhostStateBase nextState = _currentState.RunState();

		if (nextState != _currentState)
		{
			SwitchState(nextState);
		}
	}

	public void SwitchState(GhostStateBase newState)
	{
		_currentState.ExitState();
		_currentState = newState;
		_currentState.EnterState();
	}

	/// <summary>
	/// Move to a target location.
	/// </summary>
	/// <param name="targetPosition">Location to move to.</param>
	public void Move(Vector3 targetPosition)
	{
		Vector2 direction = targetPosition - transform.position;

		_rigidbody.velocity = direction.normalized * _speed * _speedModifier * Time.deltaTime;
	}

	/// <summary>
	/// Update the speed modifier.
	/// </summary>
	/// <param name="value">Value to set speed modifier.</param>
	public void SetSpeedModifier(float value)
	{
		_speedModifier = value;
	}

	/// <summary>
	/// Play an animation.
	/// </summary>
	/// <param name="animationName">Name of animation to play.</param>
	public void PlayAnimation(string animationName)
	{
		_animator.Play(animationName);
	}

	/// <summary>
	/// Determine which direction the ghost is moving.
	/// </summary>
	/// <returns>Direction of movement.</returns>
	public Vector2 CalculateMoveDirection()
	{
		if (_rigidbody.velocity.x > 0)
		{
			return Vector2.right;
		}
		else if (_rigidbody.velocity.x < 0) 
		{
			return Vector2.left;
		}
		else if (_rigidbody.velocity.y > 0)
		{
			return Vector2.up;
		}
		else if (_rigidbody.velocity.y < 0)
		{
			return Vector2.down;
		}

		return Vector2.zero;
	}
}
