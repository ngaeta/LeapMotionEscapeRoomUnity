using UnityEngine;
using System.Collections;

public class provaoutline : MonoBehaviour {
	
	// Use this for initialization
	void Start () {

		GetComponent<MeshRenderer> ().material.SetFloat ("_Outline", 1f);
	}
	
	// Update is called once per frame
	void Update () {
		GetComponent<MeshRenderer> ().material.SetFloat ("Outline width", 1f);
	}
}
