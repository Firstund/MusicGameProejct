using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Parent class responsible for extracting beats from..
/// ..spectrum value given by AudioSpectrum.cs
/// </summary>
public class AudioSyncer : MonoBehaviour {
	protected AudioSource audioSource = null;
	[SerializeField]
	protected AudioSpectrum audioSpectrum = null;
	/// <summary>
	/// Inherit this to cause some behavior on each beat
	/// </summary>
	public virtual void OnBeat()
	{
		//Debug.Log("beat");
		m_timer = 0;
		m_isBeat = true;
	}

	/// <summary>
	/// Inherit this to do whatever you want in Unity's update function
	/// Typically, this is used to arrive at some rest state..
	/// ..defined by the child class
	/// </summary>
	public virtual void OnUpdate()
	{ 
		// update audio value
		m_previousAudioValue = m_audioValue;
		m_audioValue = audioSpectrum.spectrumValue;

		//Debug.Log(m_audioValue);

		// if audio value went below the bias during this frame
		if (m_previousAudioValue > bias &&
			m_audioValue <= bias)
		{
			// if minimum beat interval is reached
			if (m_timer > timeStep)
				OnBeat();
		}

		// if audio value went above the bias during this frame
		if (m_previousAudioValue <= bias &&
			m_audioValue > bias)
		{
			// if minimum beat interval is reached
			if (m_timer > timeStep)
				OnBeat();
		}

		m_timer += Time.deltaTime;
	}

	protected virtual void Start()
    {
		audioSource = audioSpectrum.gameObject.GetComponent<AudioSource>();
	}
	protected virtual void Update()
	{
		OnUpdate();
	}

	public float bias;
	public float timeStep;
	public float timeToBeat;
	public float restSmoothTime;

	protected float m_previousAudioValue;
	protected float m_audioValue;
	protected float m_timer;

	protected bool m_isBeat;
}
