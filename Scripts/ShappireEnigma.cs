using UnityEngine;
using System.Collections;

public class ShappireEnigma : MonoBehaviour {

	private Material m_Material;
	public Color NewColor;
	private Color m_InitialColor;
	private bool m_NearFire=false;

	// Use this for initialization
	void Start () {
		m_Material= transform.GetComponent<MeshRenderer> ().material;
		m_InitialColor = m_Material.color;
	}
	
	// Update is called once per frame
	void Update () {

		if(m_NearFire && m_Material.color != NewColor)
			m_Material.SetColor("_Color", Color.Lerp (m_Material.color, NewColor, Time.deltaTime));
		
		else if(!m_NearFire && m_Material.color != m_InitialColor)
			m_Material.SetColor("_Color", Color.Lerp(m_Material.color, m_InitialColor, Time.deltaTime));
	}

	void OnTriggerStay(Collider collision) {

		if (collision.gameObject.layer == LayerMask.NameToLayer ("Fire") && collision.gameObject.GetComponent<ParticleSystem>().isPlaying) 
			m_NearFire = true;
	}

	void OnTriggerExit(Collider collision) {
	    
		if (collision.gameObject.layer == LayerMask.NameToLayer ("Fire")&& collision.gameObject.GetComponent<ParticleSystem>().isPlaying) 
			m_NearFire=false;
	}
}
