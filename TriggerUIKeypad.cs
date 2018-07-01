using UnityEngine;
using System.Collections;

public class TriggerUIKeypad : UIMessage {

	public BookEnigma locker;

	void OnTriggerEnter(Collider collision) {

		if (locker.IsKeypadInPosition)
			base.TriggerEnter (collision);
	}
}
