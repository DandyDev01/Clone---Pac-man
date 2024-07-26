using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameStateBase : MonoBehaviour
{
	public abstract void EnterStart();

	public abstract void ExitStart();

	public abstract GameStateBase RunState();

	protected abstract GameStateBase CheckForStateSwitch();
}
