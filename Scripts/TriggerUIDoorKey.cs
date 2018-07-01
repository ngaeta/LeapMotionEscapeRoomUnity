using UnityEngine;
using System.Collections;

public class TriggerUIDoorKey : UIMessage {

	public DoorLocker door;

	void OnTriggerEnter(Collider collision) {
	
		if (door.IsKeyInPosition)
			base.TriggerEnter (collision);
	}
}
