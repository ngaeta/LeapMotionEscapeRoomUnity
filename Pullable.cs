using UnityEngine;
using System.Collections;

public abstract class Pullable : MonoBehaviour {


	public Utility.GESTURE howPull= Utility.GESTURE.GRAB_HAND;

	public abstract bool PullOut (float forceToPull);

	

}
