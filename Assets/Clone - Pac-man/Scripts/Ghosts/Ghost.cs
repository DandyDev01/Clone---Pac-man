using Grid;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    [SerializeField] private float _speed;
	[SerializeField] private GameObject _marker;

	[Header("States")]
	[SerializeField] private GhostStateBase _chaseState;
	private ScatterState _scatterState;
	private FrightenedState _frightenedState;

	private Transform _target;
    private SampleGridXY _grid;
	private GhostStateBase _currentState;
	private Rigidbody2D _rigidbody;
	private Animator _animator;

	public GameObject Marker => _marker;
	public GhostStateBase CurrentState => _currentState;
	public GhostStateBase ScatterState => _scatterState;
	public GhostStateBase FrightenedState => _frightenedState;
	public GhostStateBase ChaseState => _chaseState;
	public Vector3 ScatterPoint => _scatterState.HomePath.First().position;
	public float Speed => _speed;

	private void Awake()
	{
		_rigidbody = GetComponent<Rigidbody2D>();
		_animator = GetComponentInChildren<Animator>();

		_grid = FindObjectOfType<SampleGridXY>();
		_target = GameObject.FindGameObjectWithTag("Player").transform;
		_frightenedState = GetComponentInChildren<FrightenedState>();
		_scatterState = GetComponentInChildren<ScatterState>();

		_chaseState.Init(_grid, this);
		_scatterState.Init(_grid, this);
		_frightenedState.Init(_grid, this);	

		_currentState = _scatterState;

	}

	private void Start()
	{
		_currentState.EnterState();
		_currentState.UpdatePath();
	}

	private void Update()
	{
		GhostStateBase nextState = _currentState.RunState(_grid, this);

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

	public void Move(Vector3 targetPosition)
	{
		// cannot reach target
		Vector2 direction = targetPosition - transform.position;

		_rigidbody.velocity = direction.normalized * _speed * Time.deltaTime;

		//_rigidbody.MovePosition(_rigidbody.position + (_speed * direction.normalized) *  Time.deltaTime);

		// collisions are not triggered
		//transform.position = Vector2.MoveTowards(transform.position, targetPosition,
		//	Speed * Time.deltaTime);
	}

	public void PlayAnimation(string animationName)
	{
		_animator.Play(animationName);
	}

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
