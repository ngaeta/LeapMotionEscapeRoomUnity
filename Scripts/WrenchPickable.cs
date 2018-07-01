using UnityEngine;
using System.Collections;

public class WrenchPickable : Pickable {

	private Rigidbody m_RbChildHit;
	private Collider m_ColliderChildHit;


	void Start() {
		OnStart ();
		m_RbChildHit = transform.Find ("ColliderHit").GetComponent<Rigidbody> ();
		m_ColliderChildHit = transform.Find ("ColliderHit").GetComponent<Collider> ();
	}

	public override void OnLeave (float forceToLeave)
	{
		m_RbChildHit.isKinematic = true;
		m_ColliderChildHit.enabled = false;
		base.OnLeave (forceToLeave);
	}

	public override Transform PickUp (Transform newParent)
	{
		m_ColliderChildHit.enabled = true;
		m_RbChildHit.isKinematic = false;
		return base.PickUp (newParent);
	}
}
