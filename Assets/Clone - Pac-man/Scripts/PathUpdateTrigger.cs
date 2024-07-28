using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathUpdateTrigger : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.enabled = false;
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
        if (collision.tag != "Ghost")
            return;

        Ghost ghost = collision.GetComponent<Ghost>();

        if (ghost.CurrentState is RedGhostChaseState || ghost.CurrentState is BlueGhostChaseState ||
            ghost.CurrentState is OrangeGhostChaseState || ghost.CurrentState is PinkGhostChaseState ||
            ghost.CurrentState is FrightenedState)
        {
           ghost.CurrentState.UpdatePath();
        }
	}
}
