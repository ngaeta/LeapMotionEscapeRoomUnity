using UnityEngine;
using System.Collections;

public class DoorManager : MonoBehaviour {

	public bool IsOpened=false;
	public GameObject dust;
	private AudioSource m_AudioSource;
	public AudioClip clipDoorOpened, clipDoorClosed;
	private Animator m_Animator;


	void Awake() {
		m_Animator = GetComponent<Animator> ();
		m_AudioSource = GetComponent<AudioSource> ();
		m_AudioSource.clip = clipDoorClosed;
	}

	public bool Opened {
	
		get {
			return IsOpened;
		}

		set {
			IsOpened = value;
			dust.SetActive (true);
		}
	}

	public void TryOpenDoor() {
	
		if (IsOpened) {
			m_Animator.Play ("Door_Open");						
			GetComponent<Collider> ().isTrigger = true;
			m_AudioSource.clip = clipDoorOpened;
		}
		m_AudioSource.Play ();
	}
}
