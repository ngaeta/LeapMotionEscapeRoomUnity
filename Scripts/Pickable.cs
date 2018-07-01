using UnityEngine;
using System.Collections.Generic;
using Leap;
using Leap.Unity;
using System.Collections;



public class Pickable : MonoBehaviour{

	public Utility.GESTURE howPick;
	public Collider m_Trigger;
	public Collider m_Collider;
	public Vector3 m_Offset;
	[Range(0.0f, 0.03f)] public float widthOutline;
	protected bool m_IsPickUp;
	protected Rigidbody m_RigidBody;
	public MeshRenderer outline;
	protected bool InHand;
	protected Quaternion now;


	void Start() {
		OnStart ();
	}
		
	void Update() {
		OnUpdate ();
	}

	public virtual void OnLeave(float forceToLeave) {
		Debug.Log ("OnLeave");
		InHand = false;
		m_Collider.enabled = true;
		m_RigidBody.isKinematic = false;
		m_RigidBody.constraints = RigidbodyConstraints.None;
		m_RigidBody.AddForce (Vector3.forward * forceToLeave * Time.deltaTime);
		StartCoroutine ("CheckMovement");
	}

	protected void OnStart() {
		InHand = false;
		m_RigidBody = GetComponent<Rigidbody> ();
		outline.gameObject.SetActive (false);
	}

	protected void OnUpdate() {
	}


	public virtual Transform PickUp(Transform newParent) {
		transform.parent = newParent;
		transform.localPosition = m_Offset;
		m_Collider.enabled = false;
		m_Trigger.enabled = false;
		m_RigidBody.constraints = RigidbodyConstraints.FreezePosition;
		m_RigidBody.isKinematic = true;
		InHand = true;
		outline.gameObject.SetActive (false);
		return transform;
	}

	protected virtual IEnumerator CheckMovement() {
		yield return new WaitForSeconds (0.1f);
		while (m_RigidBody.velocity.magnitude > 0.001f)
			yield return null;
		Debug.Log ("Fermo");
		m_RigidBody.isKinematic = true;
		m_Collider.enabled = false;
		m_Trigger.enabled = true;
	}

	void OnTriggerEnter(Collider collision) {
		if (!InHand && collision.gameObject.tag.Equals ("PalmLeft")) {
			outline.gameObject.SetActive (true);
			outline.material.SetFloat ("_Outline", widthOutline);
		}
	}

	void OnTriggerExit(Collider collision) {
	
		if(!InHand && collision.gameObject.tag.Equals("PalmLeft"))
		   outline.gameObject.SetActive (false);
	}
}
