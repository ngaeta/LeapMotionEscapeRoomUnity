using UnityEngine;
using System.Collections;

public class StatueEnigma : MonoBehaviour {

	public PalmLeftColliderManager palm;
	private Transform m_FlowerInPosition;
	public Transform transformFlower;
	private float current = 0f;
	public float time=4;
	public Vector3 positionEnd;
	private Transform parent;
	private Vector3 positionStart;

	void Start() {
		parent = transform.parent;
		positionStart = parent.transform.localPosition;

	}

	void OnTriggerStay(Collider collision) {
		Transform objectInTrigger = palm.ObjectInHand;
		if (objectInTrigger != null && objectInTrigger.tag.Equals ("Flower") && m_FlowerInPosition == null) {
			objectInTrigger.GetComponent<FlowerPickable> ().InTrigger = true;
			objectInTrigger.transform.position = transformFlower.position;
			objectInTrigger.rotation = transformFlower.rotation;
			objectInTrigger.parent = transformFlower;    // non va perchè??? fa il parent e poi ritorna come se non avesse genitore
			m_FlowerInPosition = objectInTrigger;
		}
	}

	void Update() {
		if (m_FlowerInPosition != null) {
			
			if (m_FlowerInPosition.childCount <= 3) {
				MoveStatue ();
			}
		}
	}

	private void MoveStatue()  {
		current += Time.deltaTime;
		if (current < time) {
			float perc = current / time;
			parent.localPosition = Vector3.Lerp (positionStart, positionEnd, perc);
			m_FlowerInPosition.parent = parent;
		} else
			Destroy (GetComponent<StatueEnigma> ());
	}
}
