using UnityEngine;
using System.Collections;

public class PaintingScript : MonoBehaviour {

	private TextMesh m_TextMesh;
	public PalmLeftColliderManager pm;
	public Light light;
	private bool m_LanternInTrigger=false;
	private Color m_InitialColor;
	public Color FinalColor;

	void Awake() {
		m_TextMesh = GetComponentInChildren<TextMesh> ();
		m_InitialColor = m_TextMesh.color;
		m_TextMesh.gameObject.SetActive (false);
	}


	void Update() {
	
		if (m_LanternInTrigger && m_TextMesh.color != FinalColor) 
		{
			
			m_TextMesh.color = Color.Lerp (m_TextMesh.color, FinalColor, Time.deltaTime);
			m_TextMesh.gameObject.SetActive (true); 

		} 
		else if(!m_LanternInTrigger && m_TextMesh.color != m_InitialColor) 
		{
		
			m_TextMesh.color = Color.Lerp (m_TextMesh.color, m_InitialColor, Time.deltaTime);
			if(m_TextMesh.color == m_InitialColor)
			   m_TextMesh.gameObject.SetActive (false); 
		}
	
	}


	void OnTriggerStay(Collider collision) {
		if (collision.gameObject.name.Equals("palm") && pm.ObjectInHand != null &&  pm.ObjectInHand.gameObject.tag.Equals("Lantern") && !light.enabled && !m_LanternInTrigger) {
			m_LanternInTrigger = true;
		}
	}

	void OnTriggerExit(Collider collision) {
		if(m_LanternInTrigger)
		   m_LanternInTrigger = false;
	}
}
