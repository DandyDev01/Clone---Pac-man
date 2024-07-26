using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pellet : Collectable
{
	protected override void Collect(Collider2D collision)
	{
		var player = collision.gameObject.GetComponent<Player>();

		if (player is null)
			return;

		player.Pickup(1);
	}

}
