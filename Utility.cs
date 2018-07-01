using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Leap;

public static class Utility  {

	public enum GESTURE{ OPEN_HAND, GRAB_HAND, PINCH, MOVE_PLAYER, ROTATION_CAMERA, ONLY_THUMB_HORIZONTAL, CROUCH
	};

	public static bool IsHandExtended(Hand hand) {
		List<Finger> fingers = hand.Fingers;
		if (fingers [0].IsExtended && fingers [1].IsExtended && fingers [2].IsExtended && fingers [3].IsExtended && fingers [4].IsExtended)
			return true;
		return false;
	}

	public static bool IsOnlyIndexExtended(List<Finger> fingers) {

		if (fingers [1].IsExtended && !fingers [0].IsExtended && !fingers [2].IsExtended && !fingers [3].IsExtended && !fingers [4].IsExtended)
			return true;
		else
			return false;
	}

	public static bool IsOnlyIndexAndMiddleExtended(Hand hand) {
	    
		List<Finger> fingers = hand.Fingers;
		if (fingers [1].IsExtended && fingers [2].IsExtended && !fingers [0].IsExtended && !fingers [3].IsExtended && !fingers [4].IsExtended)
			return true;
		else
			return false;
	}

	public static bool IsOnlyThumbExtended(Hand hand) {
		List<Finger> fingers = hand.Fingers;
		if (fingers [0].IsExtended && !fingers [1].IsExtended && !fingers [2].IsExtended && !fingers [3].IsExtended && !fingers [4].IsExtended)
			return true;
		else
			return false;
	}
		
}

