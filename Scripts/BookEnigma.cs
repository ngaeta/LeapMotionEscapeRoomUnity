using UnityEngine;
using System.Collections;

public class BookEnigma : MonoBehaviour {

	public PalmLeftColliderManager palm;
	public Transform transformBook;
	private Transform m_BookInPosition=null;
	private float current=0;
	public float time = 4f;
	public Transform panelToRotate;
	public Vector3 quaternionEnd;
	private Quaternion quaternionStart;
	private float m_Axis_W;
	public GameObject keypad;
	public bool IsKeypadInPosition=false;

	void Start() {
		quaternionStart = panelToRotate.localRotation;
		m_Axis_W = panelToRotate.localRotation.w;
	}

	void OnTriggerEnter() {
		Transform objectInTrigger = palm.ObjectInHand;

		if (objectInTrigger != null && objectInTrigger.tag.Equals ("Book") && m_BookInPosition == null) {

			if (objectInTrigger.rotation.x <= 0.6f) {  // o 0.5
				keypad.SetActive(true);
				objectInTrigger.GetComponent<BookPickable> ().InTrigger = true;
				objectInTrigger.transform.position = transformBook.position;
				objectInTrigger.rotation = transformBook.rotation;
				objectInTrigger.parent = transformBook;    // non va perchè??? fa il parent e poi ritorna come se non avesse genitore
				m_BookInPosition = objectInTrigger;
			}
		}
	}

	void Update() {
		
		if (m_BookInPosition != null) {
			RotatePanel ();
		}
	}

	private void RotatePanel()  {
		if (panelToRotate.localRotation.y > -0.70) {
			panelToRotate.Rotate (0, -2, 0, 0);
		} else {
			m_BookInPosition.SetParent (transformBook);
			m_BookInPosition = null;
			IsKeypadInPosition = true;

		}
	}
		
		
}
