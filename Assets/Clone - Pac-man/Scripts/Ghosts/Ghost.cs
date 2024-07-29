using Grid;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private SampleGridXY _grid;
	[SerializeField] private Transform _target;
	[SerializeField] private GameObject _marker;

	[Header("States")]
	[SerializeField] private GhostStateBase _chaseState;
	[SerializeField] private ScatterState _scatterState;
	[SerializeField] private FrightenedState _frightenedState;

	private GhostStateBase _currentState;
	private Rigidbody2D _rigidbody;
	private Animator _animator;

	public Vector3 ScatterPoint => _scatterState.HomePath.First().position;
	public GameObject Marker => _marker;
	public float Speed => _speed;
	public GhostStateBase CurrentState => _currentState;

	private void Awake()
	{
		_rigidbody = GetComponent<Rigidbody2D>();
		_animator = GetComponentInChildren<Animator>();

		_chaseState.Init(_grid, this);
		_scatterState.Init(_grid, this);
		_frightenedState.Init(_grid, this);	

		_currentState = _chaseState;

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
			_currentState.ExitState();
			_currentState = nextState;
			_currentState.EnterState();
		}
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
