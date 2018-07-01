using UnityEngine;
using System.Collections;

public class CameraRotation : MonoBehaviour {

	private Transform m_Player;
	private Quaternion m_QuaternionDefault;
	public GestureManager gesture;
	public static bool Rotating = false;
	private Vector3 newRotation;
	public float SpeedRotation = 0.5f; 
	private Vector3 center= new Vector3(0f, 0f, 0f);

	// Use this for initialization
	void Start () {
	
		//m_Player = GameObject.FindGameObjectWithTag ("Player").transform;
		m_Player=Camera.main.transform;
		m_QuaternionDefault = transform.localRotation;
	}


	void OnTriggerStay(Collider collision) {

		if (collision.gameObject.tag.Equals ("PalmRight")) {

			if (gesture.GestureRightHand == Utility.GESTURE.GRAB_HAND)
			{
				newRotation = Vector3.zero;
				float z = m_Player.localRotation.z;
				float yDifference = collision.gameObject.transform.localPosition.y - transform.localPosition.y;
				float xDifference = collision.gameObject.transform.localPosition.x - transform.localPosition.x;
				//Debug.Log ("x" + xDifference + " y " + yDifference);

				if (xDifference < 0) {
					newRotation.y = -SpeedRotation;
				} else if (xDifference > 0.1f) {
					
					newRotation.y = SpeedRotation;
				}

				if (yDifference < 0.25f) {
					newRotation.x = SpeedRotation;


				} else if (yDifference > 0.35f) {

					newRotation.x = -SpeedRotation;

				}

				if (newRotation != Vector3.zero) {
					
					Rotating = true;
				} else
					Rotating = false;
			} 
			/*else if(gesture.GestureRightHand == Utility.GESTURE.PINCH) 
			{
				//posizione default
			}*/
			else 
				Rotating=false;	
		}
			
	}

	void Update() {
	
		if (Rotating) {
			if (gesture.RightHandIsVisible ) {
				m_Player.Rotate (0f, newRotation.y, 0f);
				m_Player.Rotate (newRotation.x, 0f, 0f);  //perche ruota su z???

				Vector3 m=m_Player.eulerAngles;
				m.z = 0f;
				m_Player.eulerAngles = m;
			} else
				Rotating = false;
		}
	}
}
