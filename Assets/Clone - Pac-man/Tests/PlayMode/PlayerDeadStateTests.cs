using System.Collections;
using System.Collections.Generic;
using Grid;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class PlayerDeadStateTests : MonoBehaviour
{
	Player player;

	[UnityTest]
	public IEnumerator EnterStateTest()
	{
		SceneManager.LoadScene("GamePlayTest");

		yield return new WaitUntil(() => SceneManager.GetSceneByName("GamePlayTest").isLoaded);

		player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
		player.transform.position = new Vector3(6.5f, 3.5f, 0);

		yield return new WaitForFixedUpdate();

		var deadState = player.DeadState;

		player.SwitchState(deadState);

		yield return new WaitForSeconds(5);

		Assert.AreNotEqual(player.transform.position, new Vector3(6.5f, 3.5f, 0));
	}
}
