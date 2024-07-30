using System.Collections;
using System.Collections.Generic;
using Grid;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class GhostFrightenedStateTests 
{
    GameObject redGhost;

	[SetUp]
    public void Setup()
    {
	}

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator EnterStateTest()
    {
		SceneManager.LoadScene("GamePlayTest");

        yield return new WaitUntil(() => SceneManager.GetSceneByName("GamePlayTest").isLoaded);

		redGhost = GameObject.FindObjectOfType<Ghost>().gameObject;
		var frightenedState = redGhost.GetComponentInChildren<FrightenedState>();
        var animator = redGhost.GetComponentInChildren<Animator>();
        var ghost = redGhost.GetComponent<Ghost>();

        yield return new WaitForFixedUpdate();

		ghost.SwitchState(ghost.FrightenedState);


        Assert.AreEqual(0.5, ghost.SpeedModifier);
        //Assert.AreEqual("Frightened", animator.GetCurrentAnimatorClipInfo(0).clip.name);

        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }

	[UnityTest]
	public IEnumerator ExitStateTest()
	{
		SceneManager.LoadScene("GamePlayTest");

		yield return new WaitUntil(() => SceneManager.GetSceneByName("GamePlayTest").isLoaded);

		redGhost = GameObject.FindObjectOfType<Ghost>().gameObject;
		var frightenedState = redGhost.GetComponentInChildren<FrightenedState>();
		var animator = redGhost.GetComponentInChildren<Animator>();
		var ghost = redGhost.GetComponent<Ghost>();

		yield return new WaitForFixedUpdate();

		ghost.SwitchState(ghost.FrightenedState);
		Assert.AreEqual(1, ghost.SpeedModifier);

		yield return null;
	}

	[UnityTest]
	public IEnumerator SwitchToDeadTest()
	{
		SceneManager.LoadScene("GamePlayTest");

		yield return new WaitUntil(() => SceneManager.GetSceneByName("GamePlayTest").isLoaded);

		redGhost = GameObject.FindObjectOfType<Ghost>().gameObject;
		var frightenedState = redGhost.GetComponentInChildren<FrightenedState>();
		var animator = redGhost.GetComponentInChildren<Animator>();
		var ghost = redGhost.GetComponent<Ghost>();

		ghost.SwitchState(ghost.FrightenedState);

		yield return new WaitForFixedUpdate();

		yield return new WaitForEndOfFrame();

		var playerTransform = ghost.PlayerTransform.position = ghost.transform.position;

		GhostStateBase newState = ghost.FrightenedState.CheckForSwitchState();
		Assert.AreEqual(newState, ghost.DeadState);


		yield return null;
	}
}
