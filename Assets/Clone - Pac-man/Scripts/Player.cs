using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed;

	private PlayerInput _input;
    private Rigidbody2D _rigidbody;

    public Action OnDealth;
	public Action<int> OnPickup;

	private void Awake()
	{
		_rigidbody = GetComponent<Rigidbody2D>();   
		_input = GetComponent<PlayerInput>();
	}

	private void Update()
	{
		Move(_input.currentActionMap.actions[0].ReadValue<Vector2>());	
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.transform.tag == "Ghost")
		{
			OnDealth?.Invoke();
		}
	}

	private void Move(Vector2 direction)
    {
        _rigidbody.velocity = Time.deltaTime * _speed * direction;
    }

	public void Pickup(int amount)
	{
		OnPickup?.Invoke(amount);
	}
}
