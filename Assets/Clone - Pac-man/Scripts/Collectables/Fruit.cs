using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : Collectable
{
	private void Awake()
	{
		StartCoroutine(Deactivate());
	}

	protected override void Collect(Collider2D collision)
	{
		if (collision.tag != "Player")
			return;

		var player = collision.GetComponent<Player>();
		player.Pickup(50);
		StopCoroutine(Deactivate());
		gameObject.SetActive(false);
	}

	private IEnumerator Deactivate()
	{
		yield return new WaitForSeconds(10f);
		gameObject.SetActive(false);
	}
}
