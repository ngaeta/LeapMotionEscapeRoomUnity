using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEditor;

public class TriggerGameOver : MonoBehaviour {

	private GUIStyle guiStyle;
	public GameObject canvasToDeactivate;
	public bool ShowGUI=false;
	public GestureManager gesture;
	public CameraRotation camera;



	void OnGUI() {

		if (ShowGUI) {
			
			GUI.color = Color.black;
			GUI.Label (new Rect(Screen.width / 2 - 30, Screen.height / 2 - 30, 100, 100), "GAME OVER ", guiStyle);
			GUI.color = Color.white;
			if (GUI.Button (new Rect (Screen.width / 2 - 100, Screen.height / 2 + 100, 100, 100), "RESTART"))
				SceneManager.LoadScene ("Room");	
			if (GUI.Button (new Rect (Screen.width / 2 + 100, Screen.height / 2 + 100, 100, 100), "QUIT"))
				EditorApplication.Exit (0);
		}
	}

	// Use this for initialization
	void Start () {
	
		guiStyle = new GUIStyle ();
		guiStyle.fontSize = 25;
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	void OnTriggerEnter(Collider collision) {
		if (collision.gameObject.layer == LayerMask.NameToLayer ("ColliderPlayer")) {
			ShowGUI = true;
			canvasToDeactivate.SetActive (false);
			gesture.enabled = false;
			camera.enabled = false;
		}
	}
}
