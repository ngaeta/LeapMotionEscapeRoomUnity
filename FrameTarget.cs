#define UNITY_EDITOR
using UnityEngine;
using System.Collections;


public class FrameTarget : MonoBehaviour {

	public int targetFrame=40;


	void Awake () {
		#if UNITY_EDITOR
		QualitySettings.vSyncCount = 0;  // VSync must be disabled
		Application.targetFrameRate = targetFrame;
		#endif
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
