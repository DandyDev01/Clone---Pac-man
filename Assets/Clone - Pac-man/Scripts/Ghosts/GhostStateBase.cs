using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GhostStateBase : MonoBehaviour
{
    protected abstract void ChooseTargetLocation();

    public abstract void EnterState();

    public abstract void ExitState();

    public abstract GhostStateBase RunState();

    public abstract GhostStateBase CheckForSwitchState();
}
