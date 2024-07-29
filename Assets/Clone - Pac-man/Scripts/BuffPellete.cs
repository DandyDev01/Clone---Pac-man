using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffPellete : Collectable
{


	protected override void Collect(Collider2D collision)
	{
		GhostCordinator ghostCordinator = FindObjectOfType<GhostCordinator>();
		ghostCordinator.FrightenedMode();
	}
}
