using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] Teleporter _other;

	private Transform _player;

	public bool IsActive = true;

	private void Awake()
	{
		_player = GameObject.FindGameObjectWithTag("Player").transform;
	}

	private void Update()
	{
		float distance = Vector3.Distance(_player.position, transform.position);
	
		if (distance < 3 && IsActive)
		{
			_other.IsActive = false;
			_player.position = _other.transform.position;
		}

		if (distance > 3.5 && IsActive == false)
		{
			IsActive = true;
		}
	}
}
