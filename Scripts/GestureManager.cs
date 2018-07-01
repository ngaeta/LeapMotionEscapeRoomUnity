//#define LEAPDEBUG

using UnityEngine;
using System.Collections;
using Leap;
using System.Collections.Generic;
using Leap.Unity;
using System;


public class GestureManager : MonoBehaviour {

	public ColliderPlayer cp;
	public float HandGrabAngleToDetectOpenHand = 0.5f, HandGrabAngleToDetectGrabHand = 3.0f;
	public float HandPinchStrenghtToDetectGrabHand = 0.50f;
	public float HandPinchStrenghtToDetectOnlyThumbHorizontal = 0.3f, HandPinchStrenghtToDetectPinch = 0.50f;
	public float HandPinchDistanceToDetectPinch = 25f;
	public float TimeToDetectMovement = 0.8f, TimeToDetectCrouch=0.8f;
	public float m_SpeedWalk = 1;
	public delegate void OpenHandEvent();
	public static event OpenHandEvent OnOpenHandRight, OnOpenHandLeft;
	public float YCrouched=0.014f;
	public static bool PlayerInTrigger=false;
	private AudioSource source;
	public AudioClip[] clips;

	private float m_YCrouchedDefault;
	private bool IsCrouched=false, m_NextAction=true;
	private bool m_CrouchDetection=false;
	private Camera m_Camera;
	private float m_TimeToNextAction;
	private LeapProvider m_Provider;
	private Controller m_Controller;
	private List<Hand> m_CurrentHands;
	private Utility.GESTURE m_GestureRightHand, m_GestureLeftHand;
	private Vector3  m_DirectionLefttHand;  // per il movimento
	private float m_FingerLengthLeftHand;  
	private Hand m_LeftHand, m_RightHand;
	private int m_IdLeftHand, m_IdRightHand;
	private float m_TimeToWalk, m_TimeToCrouch=0f;
	private bool m_MovementGestureDetected;
	private Vector3 m_PivotRotation, m_CrouchPosition;
	private GameObject m_Player;
	private Dictionary<long, Utility.GESTURE> m_FramesPrevGestureLeftHand, m_FramesPrevGestureRightHand;
	private float m_PrevPalmPositionX = Mathf.Infinity;
	private long m_LastFrameID;
	private bool m_LeftHandIsVisible, m_RightHandIsVisible;
	private int m_ClipSteepToPlay;

	void Awake() {
		m_Provider = FindObjectOfType<LeapProvider> () as LeapProvider;
		source = GetComponent<AudioSource> ();
	}

	void Start() {
		m_Controller = new Controller ();
		m_RightHand=m_LeftHand=null;
		m_LeftHandIsVisible = m_RightHandIsVisible = false;
		m_TimeToWalk = 0;
		m_TimeToNextAction = 0;
		m_MovementGestureDetected = false;
		m_PivotRotation = GameObject.FindGameObjectWithTag ("V").transform.localPosition;
		m_Player = GameObject.FindGameObjectWithTag ("Player");
		m_FramesPrevGestureLeftHand = new Dictionary<long, Utility.GESTURE> ();
		m_FramesPrevGestureRightHand = new Dictionary<long, Utility.GESTURE> ();
		m_LastFrameID = 0;
		m_Camera = Camera.main;
		m_YCrouchedDefault = m_Player.transform.localPosition.y;
		m_ClipSteepToPlay = 0;
	}

	#if LEAPDEBUG
	void OnGUI()
	{
		GUI.color = Color.black;

		if (m_LeftHandIsVisible) {
			Utility.GESTURE value;
			m_FramesPrevGestureLeftHand.TryGetValue (m_Provider.CurrentFrame.Id, out value);
			GUI.Label (new Rect (10f, 10f, 500f, 100f), "Angle: " + m_LeftHand.GrabAngle + "\n" + "Current Gesture left: " + m_GestureLeftHand + "\n" + "Prev Gesture Left: " + value);
			//GUI.Label (new Rect (10f, 80f, 350f, 50f), "Palm ora " + m_LeftHand.PalmPosition.ToVector3().x + "\n" + "Palm prima " +  m_Controller.Frame (1).Hand (m_IdLeftHand).PalmPosition.ToVector3 ().x + "\n" + "PinchStrenght " + m_LeftHand.PinchStrength);
		}
		if (m_RightHandIsVisible) {
			Utility.GESTURE value;
			m_FramesPrevGestureRightHand.TryGetValue (m_Provider.CurrentFrame.Id, out value);
			GUI.Label (new Rect (600, 10f, 500f, 100f), "Angle: " + m_RightHand.GrabAngle + "\n" + "Current Gesture Right: " + m_GestureRightHand + "\n" + "Prev Gesture Right: " + value);
		}

	}
	#endif

	void Update() {
		m_LeftHandIsVisible = false;
		m_RightHandIsVisible = false;

		Frame frame =m_Provider.CurrentFrame;
		if (frame.Id != m_LastFrameID && frame.Hands.Count > 0) {
			
			m_CurrentHands = frame.Hands;

			foreach (Hand hand in m_CurrentHands) {
				//mano sinistra
				if (hand.IsLeft) {
					m_LeftHand = hand;
					m_IdLeftHand = hand.Id;
					m_LeftHandIsVisible = true;

					if (hand.GrabAngle <= HandGrabAngleToDetectOpenHand) {
						m_GestureLeftHand = Utility.GESTURE.OPEN_HAND;
						if (OnOpenHandLeft != null) {
							OnOpenHandLeft ();
							OnOpenHandLeft = null;
						}
					} 
					else if (hand.GrabAngle >= HandGrabAngleToDetectGrabHand && hand.PinchStrength > HandPinchStrenghtToDetectGrabHand) {  // secondo parametro serve per distinguere da only thumb horizontale grab hand
						m_GestureLeftHand = Utility.GESTURE.GRAB_HAND;
					} 
					else if (hand.GrabAngle >= HandGrabAngleToDetectGrabHand && hand.PinchStrength < HandPinchStrenghtToDetectOnlyThumbHorizontal) {
						m_GestureLeftHand = Utility.GESTURE.ONLY_THUMB_HORIZONTAL;
					}
					else if (hand.PinchStrength > HandPinchStrenghtToDetectPinch && hand.PinchDistance < HandPinchDistanceToDetectPinch) {
						m_GestureLeftHand = Utility.GESTURE.PINCH;
					} 
					else  {
						List<Finger> fingers = hand.Fingers;
						if (!fingers [0].IsExtended  && !fingers [4].IsExtended && fingers [1].IsExtended) {
							m_GestureLeftHand = Utility.GESTURE.MOVE_PLAYER;
							Movem_Player (fingers);
						}
					}
				} 

				// mano destra
				else if(hand.IsRight)
				{
					
					m_RightHand = hand;
					m_IdRightHand = hand.Id;
					m_RightHandIsVisible = true;

					if (hand.GrabAngle <= HandGrabAngleToDetectOpenHand) {
						m_GestureRightHand = Utility.GESTURE.OPEN_HAND;
						if (OnOpenHandRight != null) {
							OnOpenHandRight ();
							OnOpenHandRight = null;
						}
						/*if (m_objectInRightHand != null) {
							Pickable pickable = m_objectInRightHand.gameObject.GetComponent<Pickable> ();
							m_objectInRightHand.parent = null;
							if(pickable != null)
								pickable.OnLeave (3f);   // in FixedUpdate?
							m_objectInRightHand = null;
						} */
					} 
					else if (hand.GrabAngle >= HandGrabAngleToDetectGrabHand && hand.PinchStrength > HandPinchStrenghtToDetectGrabHand) {
						m_GestureRightHand = Utility.GESTURE.GRAB_HAND;
					} 
					else if (hand.GrabAngle >= HandGrabAngleToDetectGrabHand && hand.PinchStrength < HandPinchStrenghtToDetectOnlyThumbHorizontal) {
						m_GestureRightHand = Utility.GESTURE.ONLY_THUMB_HORIZONTAL;
					}   
					else if (hand.PinchStrength > 0.50f && hand.PinchDistance < 25f) {
						m_GestureRightHand = Utility.GESTURE.PINCH;
					}

					else {
						List<Finger> fingers = hand.Fingers;
						if (!fingers [0].IsExtended && !fingers [4].IsExtended && fingers [1].IsExtended && fingers [2].IsExtended && fingers [3].IsExtended) {
							m_GestureRightHand = Utility.GESTURE.CROUCH;
							Crouch ();
						} else if (Utility.IsOnlyIndexExtended (fingers))
							m_GestureRightHand = Utility.GESTURE.MOVE_PLAYER;
					}
						
				}

				if (m_MovementGestureDetected && m_GestureLeftHand != Utility.GESTURE.MOVE_PLAYER) {
					m_MovementGestureDetected = false;
					m_TimeToWalk = 0;
				}

				if (m_CrouchDetection && m_GestureRightHand != Utility.GESTURE.CROUCH) {
					m_CrouchDetection = false;
					m_TimeToCrouch = 0f;
				}
			}
				
			m_FramesPrevGestureLeftHand.Add (frame.Id, m_GestureLeftHand);
			m_FramesPrevGestureRightHand.Add (frame.Id, m_GestureRightHand);
			m_LastFrameID = frame.Id;
		}

		if (!m_LeftHandIsVisible && OnOpenHandLeft != null) {
			OnOpenHandLeft ();
			OnOpenHandLeft = null;
		}
		if (!m_RightHandIsVisible && OnOpenHandRight != null) {
			OnOpenHandRight ();
			OnOpenHandRight = null;
		}

		//Debug.Log ("GESTURE LEFT HAND " + m_GestureLeftHand);
		//Debug.Log("GESTURE RIGHT HAND " + m_GestureRightHand);
		//StampaGestoPrecedente();
	}
		


	private void Movem_Player(List<Finger> fingers) {
		if (m_MovementGestureDetected ) {
			m_DirectionLefttHand = fingers [1].Direction.ToVector3 ();
			m_FingerLengthLeftHand = fingers [1].Length * 10;
			if (fingers [3].IsExtended) 
				m_DirectionLefttHand = -m_DirectionLefttHand;
			m_DirectionLefttHand.y = 0f;

			ControlAudio ();
			Vector3 movement = m_DirectionLefttHand * m_FingerLengthLeftHand * Time.deltaTime * m_SpeedWalk;
			m_Player.transform.position += movement;
		}
		else {
			m_TimeToWalk += Time.deltaTime;
			if (m_TimeToWalk >= TimeToDetectMovement)
				m_MovementGestureDetected = true;
		}	
			
	}

	private void ControlAudio() {
		if (!source.isPlaying) {
			source.clip = clips [m_ClipSteepToPlay];
			source.Play ();
			m_ClipSteepToPlay = (m_ClipSteepToPlay + 1) % clips.Length;
		}
	}

	private void Crouch() {
		if (m_CrouchDetection && m_NextAction) {
			m_NextAction = false;
			if (!IsCrouched) {
				cp.colliderCrouch.enabled = true;
				cp.colliderStanding.enabled = false;
				m_Player.transform.localPosition = new Vector3 (m_Player.transform.localPosition.x, YCrouched, m_Player.transform.localPosition.z);
			} else {
				

				m_Player.transform.localPosition = new Vector3 (m_Player.transform.localPosition.x, m_YCrouchedDefault, m_Player.transform.localPosition.z);
				cp.colliderCrouch.enabled = true;
				cp.colliderStanding.enabled = true;

			}

			IsCrouched = !IsCrouched;
			StartCoroutine (WaitForNextAction ());
		} else {
			m_TimeToCrouch += Time.deltaTime;
			if (m_TimeToCrouch >= TimeToDetectCrouch)
				m_CrouchDetection = true;
		}
	}

	private IEnumerator WaitForNextAction() {
		yield return new WaitForSeconds (1f);
		m_NextAction = true;
	}

	/*

	private void ControlHandRight() {

		if (m_objectInRightHand.gameObject.tag.Equals ("Remote Control") && m_GestureRightHand == Utility.GESTURE.GRAB_HAND) {

			if (m_Next) {
				for (int i = 1; i < 59; i++) {
					m_FramesPrevGestureRightHand.TryGetValue (m_Controller.Frame (i).Id, out gesturePrev);
					if (gesturePrev != null && gesturePrev == Utility.GESTURE.ONLY_THUMB_HORIZONTAL) {
						m_Next = false;
						m_objectInRightHand.gameObject.GetComponent<RemoteControllerPickable> ().Switch ();
						StartCoroutine (function ());
						break;
					}
				}
			}
		}
	}

	private IEnumerator function() {
		Debug.Log ("inizio");
		yield return new WaitForSeconds(0.5f);
		m_Next = true;
		Debug.Log ("fine");
	}*/

	private void StampaGestoPrecedente() {
		Utility.GESTURE gesturePrev;
		Frame framePrev = m_Controller.Frame (2);
		if (framePrev != null) {
			m_FramesPrevGestureLeftHand.TryGetValue (framePrev.Id, out gesturePrev);
			if (gesturePrev != null)
				Debug.Log ("Gesture left prev " + gesturePrev);
		}
	}

	public Utility.GESTURE GestureRightHand {
		get {
			return m_GestureRightHand; 
		}
	}

	public Hand RightHand {
		get {
			return m_RightHand;
		}
	}

	public Utility.GESTURE GestureLeftHand {
		get {
			return m_GestureLeftHand; 
		}
	}

	public Hand LeftHand {
		get {
			return m_LeftHand;
		}
	}

	public Utility.GESTURE getGestureRightFramePrev(int idFrame) {
		Utility.GESTURE gesturePrev;
		m_FramesPrevGestureRightHand.TryGetValue (m_Controller.Frame(idFrame).Id, out gesturePrev);
		return gesturePrev;
	}

	public Utility.GESTURE getGestureLeftFramePrev(int idFrame) {
		Utility.GESTURE gesturePrev;
		m_FramesPrevGestureLeftHand.TryGetValue (m_Controller.Frame(idFrame).Id, out gesturePrev);
		return gesturePrev;
	}

	public Hand HandLeft {
		get {
			return m_LeftHand;
		}
	}    

	public bool RightHandIsVisible {
	
		get {
			return m_RightHandIsVisible;
		}
	}
}

