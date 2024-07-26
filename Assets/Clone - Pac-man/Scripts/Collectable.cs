using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Collectable : MonoBehaviour
{
    protected abstract void Collect(Collider2D collision);

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.transform.tag == "Player")
		{
			Collect(collision);
		}
	}
}
