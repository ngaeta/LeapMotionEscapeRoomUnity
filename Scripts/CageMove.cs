using UnityEngine;
using System.Collections;

public class CageMove : MonoBehaviour {

	private bool m_CanRun=true;
	private float current=0f;
	public float time = 6f;
	public Vector3 PositionEnd;
	private Vector3 m_PositonStart;

	// Use this for initialization
	void Start () {
		m_PositonStart = transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {
			current += Time.deltaTime;
			if (current < time) {
				float perc = current / time;
				transform.localPosition = Vector3.Lerp (m_PositonStart, PositionEnd, perc);
			}
			else {
				Destroy(GetComponent<CageMove>());
			}
	}
}
