using UnityEngine;
using System.Collections;

public class ColliderCandleManager : MonoBehaviour {

	protected ParticleSystem m_Particle;
	public GestureManager m_GestureManager;

	void Start() {
		m_Particle = GetComponentInChildren<ParticleSystem> ();
	}

	void OnTriggerStay(Collider collision) {
		if (collision.gameObject.tag.Equals ("Fire") && !m_Particle.isPlaying) {
			ParticleSystem ps = collision.gameObject.GetComponent<ParticleSystem> ();
			if (ps.isPlaying) {
				m_Particle.Play ();
				m_Particle.GetComponentInChildren<Light> ().enabled = true;
			}
		}
	}

	public virtual void FireOff() {
		if (m_Particle.isPlaying) {
			m_Particle.Stop ();
			m_Particle.GetComponentInChildren<Light> ().enabled = false;
		}
	}
}
