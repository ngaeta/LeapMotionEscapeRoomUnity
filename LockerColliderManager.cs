using UnityEngine;
using System.Collections;

public class LockerColliderManager : MonoBehaviour {

	public GameObject door;
	public GestureManager gm;
	public float forceToBrokeLocker=0f;

	void OnCollisionStay(Collision collision) {
		//Debug.Log (collision.impulse.magnitude);
		if (collision.gameObject.tag.Equals ("Hit") && collision.impulse.magnitude > forceToBrokeLocker && gm.GestureLeftHand!= Utility.GESTURE.MOVE_PLAYER && !CameraRotation.Rotating ) {  // e mano destra non sta ruotando la camera
			GetComponent<Rigidbody> ().useGravity = true;
			GetComponent<Rigidbody> ().isKinematic = false;
			door.GetComponent<Animator> ().enabled = true;
		}
	}
}
