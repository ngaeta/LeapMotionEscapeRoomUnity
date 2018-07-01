using UnityEngine;
using System.Collections;

public class ColliderPlayer : MonoBehaviour {

	public Collider colliderStanding, colliderCrouch;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision collision) {
		GestureManager.PlayerInTrigger = true;
	}

	void OnCollisionExit(Collision collision) {
		GestureManager.PlayerInTrigger = false;
	}
}
