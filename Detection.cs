//////////////////////////////////////
//Script created by Alexander Ameye //
//Last edit: 10/19/2016  					  //
//Version: 1.0.3 (FREE)  						//
//////////////////////////////////////

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Detection : MonoBehaviour
{

	void Update ()
	{


			//Give the object that was hit the name 'Door'.
			GameObject Door = transform.gameObject;

			//Get access to the 'Door' script attached to the object that was hit.
			Door dooropening = Door.GetComponent<Door>();

				//Open/close the door by running the 'Open' function found in the 'Door' script.
			if (dooropening.Running == false) {
				StartCoroutine (dooropening.Open ());

			}

	}
}

