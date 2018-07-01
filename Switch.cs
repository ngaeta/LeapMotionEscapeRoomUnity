using UnityEngine;
using System.Collections;

public class Switch : MonoBehaviour {

	public GameObject lightToSwitch;
	public Material textureOff;

	private Light m_Light;
	private Material m_TextureOn;

	// Use this for initialization
	void Start () {
		m_Light = lightToSwitch.GetComponentInChildren<Light> ();
		m_TextureOn = m_Light.transform.parent.GetComponent<MeshRenderer> ().material;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SwitchLight() {
		if(m_Light.enabled) {
			m_Light.enabled = false;
			m_Light.transform.parent.GetComponent<MeshRenderer> ().material = textureOff;
		}
		else {
			m_Light.enabled = true;
			m_Light.transform.parent.GetComponent<MeshRenderer> ().material= m_TextureOn;
		}
	}
}
