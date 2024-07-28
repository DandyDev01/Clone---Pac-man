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
	[SerializeField] private GhostStateBase _scatterState;

	private GhostStateBase _currentState;

	public GameObject Marker => _marker;
	public float Speed => _speed;

	private void Awake()
	{
		_chaseState.Init(_grid, this);
		_scatterState.Init(_grid, this);

		_currentState = _scatterState;
	}

	private void Start()
	{
		_currentState.EnterState();
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
		transform.position = Vector2.MoveTowards(transform.position, targetPosition,
			Speed * Time.deltaTime);
	}
}
