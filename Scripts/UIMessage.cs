using UnityEngine;
using System.Collections;

public class UIMessage : MonoBehaviour {

	public HUDManager manager;
	public string hintToShow, textRight, textLeft;
	public Sprite imageLeft, imageRight;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider collision) {

		TriggerEnter (collision);
	}

	void OnTriggerExit(Collider collision) {
		TriggerExit (collision);
	}

	protected void TriggerEnter(Collider collision) {
		if (collision.gameObject.layer == LayerMask.NameToLayer ("ColliderPlayer")) {
			if (hintToShow.Length > 0)
				manager.ActiveHint (hintToShow);
			if (imageLeft != null)
				manager.ActiveLineLeft (imageLeft, textLeft);
			if (imageRight != null)
				manager.ActiveLineRight (imageRight, textRight);
		}
	}

	protected void TriggerExit(Collider collision) {
		if (collision.gameObject.layer == LayerMask.NameToLayer ("ColliderPlayer")) {
			manager.DeactiveHint ();
			if (imageLeft != null)
				manager.DeactiveLineLeft ();
			if (imageRight != null)
				manager.DeactiveLineRight ();
		}
	}
}
