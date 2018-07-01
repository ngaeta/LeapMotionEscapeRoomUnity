using UnityEngine;
using System.Collections;
using Leap;
using Leap.Unity;

public class PalmRightColliderManager : MonoBehaviour {

	private Transform m_objectInHand;
	private float m_PrevPalmPositionX = Mathf.Infinity;
	private bool m_Next;
	public HUDManager manager;
	private Collider m_Collider;

	public GestureManager gestureManager;

	// Use this for initialization
	void Start () {
		m_objectInHand = null;
		m_Next = true;
		m_Collider = GetComponent<Collider> ();
	}
	
	// Update is called once per frame
	void Update () {
	
		if (m_objectInHand != null)
			ControlHand ();
	}

	void OnTriggerStay(Collider collision)
	{

		FlowerPetalsPickable flowerPickable = collision.gameObject.GetComponent<FlowerPetalsPickable> ();
		Switch objectSwitch= collision.gameObject.GetComponent<Switch>();
		Utility.GESTURE m_GestureHand = gestureManager.GestureRightHand;
		Hand hand = gestureManager.RightHand;

		if (flowerPickable != null && !m_objectInHand) {
			if (m_GestureHand != null && m_GestureHand == flowerPickable.howPick) {
				m_objectInHand = flowerPickable.PickUp (transform);
				m_Collider.isTrigger = true;
				GestureManager.OnOpenHandRight += LeaveObject;
			}
		} 

		if (objectSwitch != null && !m_objectInHand) {
			if (m_GestureHand != null && m_GestureHand == Utility.GESTURE.PINCH ) {
				if (m_Next) {
					m_Next = false;
					objectSwitch.SwitchLight ();
					StartCoroutine (WaitForNextAction ());
				}
			}
		}

		else if (collision.gameObject.tag.Equals ("Door") && m_GestureHand == Utility.GESTURE.GRAB_HAND) {
			DoorManager door = collision.gameObject.transform.parent.parent.GetComponent<DoorManager> ();
			if (door.IsOpened && hand.Rotation.z < -0.70f) 
				door.TryOpenDoor ();
			else if (!door.IsOpened && hand.Rotation.z < -0.70f) {
				manager.ActiveHint ("La porta è chiusa a chiave");
				door.TryOpenDoor ();
				StartCoroutine (HideHint ());
			}
				
		}

		ColliderCandleManager candle = collision.gameObject.GetComponent<ColliderCandleManager> ();
		if (candle != null) {
			if (m_GestureHand == Utility.GESTURE.PINCH)
				candle.FireOff ();
		}
	}

	void OnCollisionStay(Collision collision) {
		//Pullable pullable = collision.gameObject.GetComponent<Pullable> ();
		Utility.GESTURE m_GestureHand = gestureManager.GestureRightHand;
		Hand hand = gestureManager.RightHand;

		/*if (pullable != null && !m_objectInHand && !CameraRotation.Rotating) {
			
			if (m_GestureHand != null && m_GestureHand == pullable.howPull) {


				float actualXPosition = hand.PalmPosition.ToVector3 ().x;

				if (m_PrevPalmPositionX == Mathf.Infinity) {
					m_PrevPalmPositionX = actualXPosition;
					return;
				}

				float direction = m_PrevPalmPositionX - actualXPosition;
				float differencePosition = Mathf.Abs (direction);	
				differencePosition = differencePosition * 1000;
				if (differencePosition > 1.2) {
					if (direction < 0)
						pullable.PullOut (differencePosition * 0.5f);
					else
						pullable.PullOut (-(differencePosition * 0.5f));
				}

				m_PrevPalmPositionX = actualXPosition;

			} else if (m_PrevPalmPositionX != Mathf.Infinity)
				m_PrevPalmPositionX = Mathf.Infinity;	
		} */

		if (collision.gameObject.tag.Equals ("Door") && m_GestureHand == Utility.GESTURE.GRAB_HAND) {
			if (collision.gameObject.transform.parent.parent.GetComponent<DoorManager>().Opened && hand.Rotation.z < -0.70f) {
				Animator anim = collision.gameObject.transform.parent.parent.gameObject.GetComponent<Animator> ();
				anim.Play ("Door_Open");						
				collision.gameObject.transform.parent.parent.GetComponent<Collider> ().isTrigger = true;
			}
		}
	}

	private void ControlHand() {

		Utility.GESTURE gestureHand = gestureManager.GestureRightHand;

		if (gestureHand != null && m_objectInHand.gameObject.tag.Equals ("Remote Control") && gestureHand == Utility.GESTURE.GRAB_HAND) {

			Utility.GESTURE gesturePrev;
			if (m_Next) {
				for (int i = 1; i < 59; i++) {
					gesturePrev = gestureManager.getGestureRightFramePrev (i);
					if (gesturePrev != null && gesturePrev == Utility.GESTURE.ONLY_THUMB_HORIZONTAL) {
						m_Next = false;
						m_objectInHand.gameObject.GetComponent<RemoteControllerPickable> ().Switch ();
						StartCoroutine (WaitForNextAction ());
						break;
					}
				}
			}
		}

		else if (gestureHand != null && m_objectInHand.gameObject.tag.Equals ("Lighter")) {
			ParticleSystem particle = m_objectInHand.Find ("TubeFire").GetComponentInChildren<ParticleSystem> ();
			if (!particle.isPlaying && gestureHand == Utility.GESTURE.GRAB_HAND) {
				Utility.GESTURE gesturePrev = gestureManager.getGestureRightFramePrev (59);
				if(gesturePrev == Utility.GESTURE.ONLY_THUMB_HORIZONTAL)
					particle.Play ();
			} else if (gestureHand == Utility.GESTURE.ONLY_THUMB_HORIZONTAL && particle.isPlaying)
				particle.Stop ();
		}
	}

	private IEnumerator WaitForNextAction() {
		Debug.Log ("inizio");
		yield return new WaitForSeconds(0.3f);
		m_Next = true;
		Debug.Log ("fine");
	}

	void LeaveObject() {
		if (m_objectInHand != null) {
			Pickable pickable = m_objectInHand.gameObject.GetComponent<Pickable> ();
			if (!pickable)
				pickable = m_objectInHand.gameObject.GetComponentInChildren<Pickable> ();
			m_objectInHand.parent = null;
			if(pickable != null)
				pickable.OnLeave (3f);   // in FixedUpdate?
			StartCoroutine(SetTriggerDelay(0.5f, false));
			m_objectInHand = null;
		} 
	}

	/*public Transform PassObjecInLeftHand() {
		Transform temp_obj = m_objectInHand;
		m_objectInHand = null;
		return temp_obj;
	}*/

	public Transform ObjectInHand {
		get {
			return m_objectInHand;
		}
	}

	private IEnumerator HideHint() {
	
		yield return new WaitForSeconds (3f);
		manager.DeactiveHint ();
	}

	IEnumerator SetTriggerDelay(float delay, bool trigger)
	{
		yield return new WaitForSeconds (delay);
		m_Collider.isTrigger = trigger;
	}

}
