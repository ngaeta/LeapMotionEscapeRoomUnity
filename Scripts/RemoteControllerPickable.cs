using UnityEngine;
using System.Collections;

public class RemoteControllerPickable : Pickable {

	public Texture texture;
	public GameObject objectToSwitch;
	public float TimeToSwitch = 10;
	public GameObject projector;

	private Light m_ProjectorLight;
	private float m_TimeToSwitch = 0;
	private bool m_CanSwitch = true;
	private int i=0;

	private bool m_SwitchOff;

	// Use this for initialization
	void Start () {
		m_SwitchOff = true;
		m_ProjectorLight = projector.GetComponentInChildren<Light> ();
		base.OnStart ();
	}
		
	void LateUpdate() {
		if (!m_CanSwitch)
			StartCoroutine ("Wait");
	}

	public void Switch() {

		if (m_CanSwitch) {
			m_CanSwitch = false;
			Debug.Log ("Tasto premuto " + ++i + "volte");
			if (m_SwitchOff) 
				objectToSwitch.GetComponent<MeshRenderer> ().materials [0].SetTexture ("_MainTex", texture);
			else
				objectToSwitch.GetComponent<MeshRenderer> ().materials [0].SetTexture ("_MainTex", null);
			m_SwitchOff = !m_SwitchOff;
			m_ProjectorLight.enabled = !m_ProjectorLight.enabled;
		}
	}

	public IEnumerator Wait() {
		yield return new WaitForSeconds (2);
		m_CanSwitch = true;
		Debug.Log ("Ora si");
	}
}
