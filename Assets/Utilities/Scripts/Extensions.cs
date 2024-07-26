using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Extensions
{

    public static T GetRandom<T>(this T[] array)
	{
		return array[Random.Range(0, array.Length)];
	}

	public static void AddEventTrigger(this Transform transform, EventTriggerType triggerType,
	   UnityEngine.Events.UnityAction<BaseEventData> function)
	{
		var onEnterTrigger = new EventTrigger.Entry();
		EventTrigger trigger = transform.gameObject.AddComponent<EventTrigger>();
		onEnterTrigger.eventID = triggerType;
		onEnterTrigger.callback.AddListener(function);
		trigger.triggers.Add(onEnterTrigger);
	}

	/// <summary>
	/// Coroutine that reduces the alpha value of color property over the intervale time
	/// </summary>
	/// <param name="spriteRenderer">sprite renderer to fade out</param>
	/// <param name="time">time until color alpha value is 0</param>
	/// <returns></returns>
	public static IEnumerator FadeOut(this SpriteRenderer spriteRenderer, float time)
	{
		Timer timer = new Timer(time);
		timer.Play();
		Color loweredAlpha;
		float ratio = 1 / (time * 10);
		while (!timer.Finished)
		{
			timer.Tick(0.01f);
			loweredAlpha = spriteRenderer.color;
			loweredAlpha.a -= ratio;
			spriteRenderer.color = loweredAlpha;
			yield return Helpers.GetWait(0.1f);
		}
	}

	/// <summary>
	/// Coroutine that reduces the alpha value of color property over the intervale time
	/// </summary>
	/// <param name="spriteRenderer">sprite renderer to fade out</param>
	/// <param name="time">time until color alpha value is 0</param>
	/// <returns></returns>
	public static IEnumerator Fadein(this SpriteRenderer spriteRenderer, float time)
	{
		Timer timer = new Timer(time);
		timer.Play();
		Color raised;
		float ratio = 1 / (time * 10);
		while (!timer.Finished)
		{
			timer.Tick(0.1f);
			raised = spriteRenderer.color;
			raised.a += ratio;
			spriteRenderer.color = raised;
			yield return Helpers.GetWait(0.1f);
		}
	}

	/// <summary>
	/// Coroutine that reduces the number of particles emmited to 0 in specified time
	/// </summary>
	/// <param name="particleSystem">particleSystem to fade out</param>
	/// <param name="time">time to fade out</param>
	/// <returns></returns>
	public static IEnumerator FadeOut(this ParticleSystem particleSystem, float time)
	{
		const float waitForSeconds = 0.1f;
		var emissionModule = particleSystem.emission;
		float timeEllapsed = 0;
		float emissionRate = emissionModule.rateOverTime.constant;
		float reduceRate = (emissionRate / time) * waitForSeconds;
		float currRate = emissionRate;

		while (timeEllapsed < time)
		{
			yield return Helpers.GetWait(waitForSeconds);
			timeEllapsed += waitForSeconds;
			currRate -= reduceRate;
			emissionModule.rateOverTime = currRate;
		}

		particleSystem.Stop();
	}

	/// <summary>
	/// Conoutine that increases the number of particles emmited to emissionModule rateOverTime constant
	/// in specified time
	/// </summary>
	/// <param name="particleSystem">partive ssystem to fade in</param>
	/// <param name="time">time to fade in</param>
	/// <returns></returns>
	public static IEnumerator FadeIn(this ParticleSystem particleSystem, float time)
	{
		const float waitForSeconds = 0.1f;
		var emissionModule = particleSystem.emission;
		float timeEllapsed = 0;
		float emissionRate = emissionModule.rateOverTime.constant;
		emissionModule.rateOverTime = 0;
		float increaseRate = (emissionRate / time) * waitForSeconds;
		float currRate = 0;

		while (timeEllapsed < time)
		{
			yield return Helpers.GetWait(waitForSeconds);
			timeEllapsed += waitForSeconds;
			currRate += increaseRate;
			emissionModule.rateOverTime = currRate;
		}
	}

	/// <summary>
	/// scales a transform
	/// </summary>
	/// <param name="transform">transform to scale</param>
	/// <param name="scale">desired scale of transform</param>
	/// <param name="time">time it takes to scale</param>
	/// <returns></returns>
	/// <exception cref="System.NotImplementedException"></exception>
	public static IEnumerator ScaleUp(this Transform transform, Vector3 scale, float time)
	{
		throw new System.NotImplementedException("not implemented");
	}

	/// <summary>
	/// fades out the volume
	/// </summary>
	/// <param name="audioSource">audio source to fade volume of</param>
	/// <param name="time">time to fade out</param>
	/// <returns></returns>
	public static IEnumerator FadeOut(this AudioSource audioSource, float time)
	{
		Timer timer = new Timer(time);
		timer.Play();
		float lowered;
		float ratio = audioSource.volume / (time * 10);
		while (!timer.Finished)
		{
			timer.Tick(0.1f);
			lowered = audioSource.volume;
			lowered -= ratio;
			audioSource.volume = lowered;
			yield return Helpers.GetWait(0.1f);
		}
	}

	/// <summary>
	/// fades in the volume from 0 to 1
	/// </summary>
	/// <param name="audioSource">audio source to fade volume of</param>
	/// <param name="time">time to fade in volume</param>
	/// <returns></returns>
	public static IEnumerator FadeIn(this AudioSource audioSource, float time)
	{
		audioSource.volume = 0;
		Timer timer = new Timer(time);
		timer.Play();
		float lowered;
		float ratio = 1 / (time * 10);
		while (!timer.Finished)
		{
			timer.Tick(0.1f);
			lowered = audioSource.volume;
			lowered += ratio;
			audioSource.volume = lowered;
			yield return Helpers.GetWait(0.1f);
		}
	}
}
