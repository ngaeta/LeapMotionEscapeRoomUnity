using UnityEngine;
using System.Collections;

public class DoorLocker : MonoBehaviour {

	public PalmLeftColliderManager palm;
	public GestureManager gm;
	public Transform NewPositionKey;
	public HUDManager manager;
	private Transform m_KeyInPosition;
	public Sprite image;
	private AudioSource m_AudioSource;
	public AudioClip clipKeyOpen;

	void Awake() {
		m_AudioSource = GetComponent<AudioSource> ();
		m_AudioSource.clip = clipKeyOpen;
	}

	void OnTriggerEnter(Collider collision) {
		if (collision.gameObject.name.Equals("palm")) { 
			Transform objectInHand=palm.ObjectInHand;
			if (objectInHand != null && objectInHand.gameObject.tag.Equals ("Key")) {
				
				m_KeyInPosition = objectInHand;
				Destroy (objectInHand.GetComponent<Pickable> ());
				objectInHand.transform.position = NewPositionKey.position;
				objectInHand.transform.rotation = NewPositionKey.rotation;
				objectInHand.transform.parent = NewPositionKey;
				manager.ActiveHint ("Per aprire la porta ruota la chiave verso destra");
				manager.ActiveLineLeft (image, "To Rotate Key");
			}
		}
	}

	void OnTriggerStay(Collider collision) {
		Transform objectInHand=palm.ObjectInHand;
		if (m_KeyInPosition != null && !objectInHand && (gm.GestureLeftHand == Utility.GESTURE.PINCH || gm.GestureLeftHand == Utility.GESTURE.GRAB_HAND)) {
		 

			if (gm.LeftHand.Rotation.z < -0.1) {  // o -0.1
				m_AudioSource.Play ();
				m_KeyInPosition.GetChild(0).Rotate(new Vector3 (0, 0, 90f));
				transform.parent.GetComponent<DoorManager> ().Opened = true;
				m_KeyInPosition.parent = transform.parent.Find ("Door_Wood");
				m_KeyInPosition = null;
				manager.DeactiveLineLeft ();
				manager.DeactiveHint ();
			}
		}
	}

	public bool IsKeyInPosition {
		get {
			if (m_KeyInPosition != null)
				return true;
			return false;
		}
	}
}
