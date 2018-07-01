using UnityEngine;
using System.Collections;

public class IndexColliderManager : MonoBehaviour {

	public GestureManager gestureManager;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerExit(Collider collision) {
	
		if (collision.gameObject.layer == LayerMask.NameToLayer ("ButtonKeypad") && gestureManager.GestureRightHand == Utility.GESTURE.MOVE_PLAYER) {
			PasswordPanelManager panelManager = collision.gameObject.transform.parent.parent.GetComponent<PasswordPanelManager> ();
			if (panelManager != null)
				panelManager.OnButtonReleased ();
		}
	}

	void OnTriggerEnter(Collider collision) {
		if (collision.gameObject.layer == LayerMask.NameToLayer("ButtonKeypad") && gestureManager.GestureRightHand == Utility.GESTURE.MOVE_PLAYER) {
			PasswordPanelManager panelManager = collision.gameObject.transform.parent.parent.GetComponent<PasswordPanelManager> ();
			if (panelManager != null) {
				panelManager.OnButtonPressed (collision.gameObject);
			}
		}
	}
}
