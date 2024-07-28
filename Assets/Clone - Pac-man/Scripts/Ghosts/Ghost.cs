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

	private GhostStateBase _currentState;

	public GameObject Marker => _marker;
	public float Speed => _speed;

	private void Awake()
	{
		_chaseState = new PinkGhostChaseState(_grid, this);

		_currentState = _chaseState;
	}

	private void Start()
	{
		_chaseState.EnterState();
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
}
