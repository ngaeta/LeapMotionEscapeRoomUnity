using UnityEngine;
using System.Collections;

public class LighterManager : MonoBehaviour {


	private AudioSource m_AudioSource;
	public AudioClip clipsOn, clipsOff, clipLoop;
	private ParticleSystem particle;

	// Use this for initialization
	void Start () {
		m_AudioSource = GetComponent<AudioSource> ();
		particle= transform.Find ("TubeFire").GetComponentInChildren<ParticleSystem> ();
	}
	

	public void SwitchOn() {
		particle.Play ();
		m_AudioSource.clip = clipsOn;
		m_AudioSource.Play ();
		StartCoroutine(WaitForLoop());
	}

	public void SwitchOff() {
		particle.Stop ();
		m_AudioSource.Stop ();
		m_AudioSource.loop = false;
	}

	private IEnumerator WaitForLoop() {
		while (m_AudioSource.isPlaying)
			yield return null;
		m_AudioSource.clip = clipLoop;
		m_AudioSource.Play ();
		m_AudioSource.loop = true;
	}

	public bool IsPlay {
	
		get {
			return particle.isPlaying;
		}
	}
}
