using UnityEngine;
using System.Collections;
using Leap;

public class FlowerPetalsPickable : Pickable {

	void Start() {
		m_Collider = GetComponent<Collider> ();
		m_RigidBody = GetComponentInParent<Rigidbody> ();
		outline.gameObject.SetActive (false);
	}

	public override Transform PickUp(Transform newParent) {
		transform.parent.parent = newParent;
		base.PickUp (transform.parent);
		return transform.parent;
	}

	public override void OnLeave(float forceToLeave) {
		Debug.Log ("OnLeave");
		m_Collider.enabled = true;
		m_RigidBody.isKinematic = false;
		m_RigidBody.constraints = RigidbodyConstraints.None;
		//Physics.gravity = new Vector3 (0f, -0.5f, 0f);
		//m_RigidBody.AddForce (Vector3.forward * forceToLeave * Time.deltaTime);
		InHand=false;
		StartCoroutine ("CheckMovement");
	}

	protected override IEnumerator CheckMovement() {
		yield return new WaitForSeconds (0.1f);
		while (m_RigidBody.velocity.magnitude > 0.001f)
			yield return null;
		Debug.Log ("Fermo");
		m_RigidBody.isKinematic = true;
		m_Collider.enabled = false;
		m_Trigger.enabled = true;
		Physics.gravity=new Vector3(0f, -9.81f, 0f);
	}
	void OnTriggerEnter(Collider collision) {
		if (!InHand && (collision.gameObject.tag.Equals ("PalmLeft") || collision.gameObject.tag.Equals ("PalmRight"))) {
			outline.gameObject.SetActive (true);
			outline.material.SetFloat ("_Outline", widthOutline);
		}
	}

	void OnTriggerExit(Collider collision) {

		if(!InHand && (collision.gameObject.tag.Equals("PalmLeft") || collision.gameObject.tag.Equals ("PalmRight")))
			outline.gameObject.SetActive (false);
	}

}
