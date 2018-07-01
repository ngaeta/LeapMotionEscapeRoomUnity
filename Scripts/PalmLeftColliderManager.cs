using UnityEngine;
using System.Collections;
using Leap;
using Leap.Unity;

public class PalmLeftColliderManager : MonoBehaviour {

	private Transform m_objectInHand;
	private float m_PrevPalmPositionX = Mathf.Infinity;
	private bool m_Next;
	private LeapProvider m_Provider;
	private Hand m_Hand;
	private Collider m_Collider;
	public HUDManager manager;
	public Sprite imageTurnOnLighter, imageTurnOffLighter;
	public string textImageLighterOff = "", textImageLighterOn="";
	public GestureManager gestureManager;

	// Use this for initialization
	void Start () {
		m_Collider = GetComponent<Collider> ();
		m_objectInHand = null;
		m_Next = true;
	}

	// Update is called once per frame
	void Update () {
		if (m_objectInHand != null)
			ControlHand ();
	}

	void OnTriggerStay(Collider collision)
	{
		Pickable pickable = collision.gameObject.GetComponent<Pickable> ();
		Switch objectSwitch= collision.gameObject.GetComponent<Switch>();
		Utility.GESTURE m_GestureHand = gestureManager.GestureLeftHand;
		Pullable pullable = collision.gameObject.GetComponent<Pullable> ();


		if (pullable != null && !m_objectInHand) {
			Hand hand = gestureManager.LeftHand;
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
		}

		if (pickable != null && !m_objectInHand) {
			if (m_GestureHand != null && m_GestureHand == pickable.howPick) {
				m_objectInHand = pickable.PickUp (transform);
				m_Collider.isTrigger = true;
				GestureManager.OnOpenHandLeft += LeaveObject;
			}
		} 

		if (objectSwitch != null && !m_objectInHand) {
			if (m_GestureHand != null && m_GestureHand == Utility.GESTURE.PINCH) {
				if (m_Next) {
					m_Next = false;
					objectSwitch.SwitchLight ();
					StartCoroutine (WaitForNextAction ());
				}
			}
		}

		ColliderCandleManager candle = collision.gameObject.GetComponent<ColliderCandleManager> ();
		if (candle != null) {
			if (m_GestureHand == Utility.GESTURE.PINCH)
				candle.FireOff ();
		}
	}

	void OnCollisionStay(Collision collision) {
		Pullable pullable = collision.gameObject.GetComponent<Pullable> ();
		Utility.GESTURE m_GestureHand = gestureManager.GestureLeftHand;
		Hand hand = gestureManager.LeftHand;

		if (pullable != null && !m_objectInHand) {

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
		} 

		else if (collision.gameObject.tag.Equals ("Door") && m_GestureHand == Utility.GESTURE.GRAB_HAND) {
			Debug.Log (hand.Rotation);
			if (collision.gameObject.GetComponent<DoorManager>().Opened && hand.Rotation.z < 0) {
				Animator anim = collision.gameObject.transform.parent.parent.gameObject.GetComponent<Animator> ();
				anim.Play ("Door_Open");						//collision.gameObject.transform.parent.parent.GetComponent<Collider> ().isTrigger = true;
			}
		}
	}

	private void ControlHand() {

		Utility.GESTURE gestureHand = gestureManager.GestureLeftHand;

		if (gestureHand != null && m_objectInHand.gameObject.tag.Equals ("Remote Control") && gestureHand == Utility.GESTURE.GRAB_HAND) {

			Utility.GESTURE gesturePrev;
			if (m_Next) {
				for (int i = 1; i < 59; i++) {
					gesturePrev = gestureManager.getGestureLeftFramePrev (i);
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
			
			LighterManager lighter = m_objectInHand.GetComponent<LighterManager> ();
			if (!lighter.IsPlay)
				manager.ActiveLineLeft (imageTurnOffLighter, textImageLighterOff);
			else 
				manager.ActiveLineLeft (imageTurnOnLighter, textImageLighterOn);
			
			if (!lighter.IsPlay && gestureHand == Utility.GESTURE.GRAB_HAND) {
				Utility.GESTURE gesturePrev;
				for (int i = 0; i < 40; i++) {
					gesturePrev = gestureManager.getGestureLeftFramePrev (i);
					if (gesturePrev == Utility.GESTURE.ONLY_THUMB_HORIZONTAL) {
						lighter.SwitchOn ();
						break;
					}
				}

			} else if (gestureHand != Utility.GESTURE.GRAB_HAND && lighter.IsPlay)
				lighter.SwitchOff ();
		}
	}

	private IEnumerator WaitForNextAction() {
		Debug.Log ("inizio");
		yield return new WaitForSeconds(0.3f); //0.5 con il telecomando
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

	IEnumerator SetTriggerDelay(float delay, bool trigger)
	{
		yield return new WaitForSeconds (delay);
		m_Collider.isTrigger = trigger;
	}
		
	/*void OnCollisionExit(Collision collision) {

		Utility.GESTURE m_GestureHand = gestureManager.GestureLeftHand;

		if (collision.gameObject.tag.Equals ("PalmRight") && m_GestureHand==Utility.GESTURE.GRAB_HAND) {
			Debug.Log ("collisone con palm right");
			Transform obj= collision.gameObject.GetComponent<PalmRightColliderManager> ().PassObjecInLeftHand ();
			m_objectInHand= obj.GetComponent<Pickable> ().PickUp (transform);
			GestureManager.OnOpenHandLeft += LeaveObject;
		}
	}*/

	public Transform ObjectInHand {
		get {
			return m_objectInHand;
		}
	}
}