using UnityEngine;
using System.Collections;

public class CandleEnigma : ColliderCandleManager {

	public AltarEnigmaScript altarEnigma;

	void OnTriggerStay(Collider collision) {
		if (collision.gameObject.tag.Equals ("Fire") && !m_Particle.isPlaying) {
			ParticleSystem ps = collision.gameObject.GetComponent<ParticleSystem> ();
			if (ps.isPlaying) {
				m_Particle.Play ();
				m_Particle.GetComponentInChildren<Light> ().enabled = true;
				altarEnigma.AddCandle (transform.parent);
				if (altarEnigma.GetCandleCountOn() == 4)
					altarEnigma.ControlCandles ();
			}
		}
	}

	public override void FireOff() {
		if (m_Particle.isPlaying) {
			m_Particle.Stop ();
			m_Particle.GetComponentInChildren<Light> ().enabled = false;
			altarEnigma.RemoveCandle (transform.parent);
		}
	}


}
