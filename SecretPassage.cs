using UnityEngine;
using System.Collections;

public class SecretPassage : MonoBehaviour {

	public TextMesh textKeypad;
	private Vector3 positionStart;
	public Vector3 positionHorizontalEnd, positionVerticalEnd;
	private float current=0f;
	private float time = 2f;
	private bool m_Next=false, m_CanRun=true;


	// Use this for initialization
	void Start () {
		positionStart = transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {

		if (!m_Next && m_CanRun) {
			current += Time.deltaTime;
			if (current < time) {
				float perc = current / time;
				transform.localPosition = Vector3.Lerp (positionStart, positionVerticalEnd, perc);
			} else {
				current = 0f;
				time = 4f;
				m_Next = true;
				positionStart = transform.localPosition;
			}
		} else {
			current += Time.deltaTime;
			if (current < time) {
				float perc = current / time;
				transform.localPosition = Vector3.Lerp (positionStart, positionHorizontalEnd, perc);
			} else {
				m_CanRun = false;
				textKeypad.text = "";
			}
		}
	}
}
