using UnityEngine;
using System.Collections;

public class PasswordPanelManager : MonoBehaviour {

	public TextMesh textMesh;
	public GameObject SecretRoom;
	public int passwordLenght;
	private bool m_NextButton=true;
	public Color colorPressed, colorDisplayPasswordConfirmed, colorDisplayPasswordDenied;
	private GameObject m_LastButtonPressed;
	public string password = "1234";
	private MeshRenderer m_Display;
	private Color m_ColorDefault;
	private AudioSource m_AudioSource;
	public AudioClip clipDigit, passwordDenied;
	public SecretPassage secretPassage;


	void Awake() {
		m_AudioSource = GetComponent<AudioSource> ();
	}
	// Use this for initialization
	void Start () {
		m_AudioSource.clip = clipDigit;
		m_Display = transform.Find ("Display").GetComponent<MeshRenderer> ();
		m_ColorDefault = m_Display.material.color;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnButtonPressed(GameObject button) {
		Debug.Log ("premuto " + button);
		string buttonName = button.name;

		if (m_NextButton) {
			m_NextButton = false;
			button.GetComponent<MeshRenderer> ().material.color = colorPressed;
			//button.transform.position = new Vector3 (button.transform.position.x, button.transform.position.y, 0.4f);
			m_LastButtonPressed = button;

			if (buttonName.Equals ("Clear") && textMesh.text.Length > 0) {
				textMesh.text = textMesh.text.Substring (0, textMesh.text.Length - 1);
			} else if (buttonName.Equals ("Enter")) {
				
				if (textMesh.text.Equals (password)) {
					m_Display.material.color = colorDisplayPasswordConfirmed;
					m_NextButton = false;
					secretPassage.enabled = true;
					SecretRoom.SetActive (true);
				} else {
					m_AudioSource.clip = passwordDenied;
					m_Display.material.color = colorDisplayPasswordDenied;
					StartCoroutine (DisplayColor ());
				}
			} else if (textMesh.text.Length < passwordLenght) 
				textMesh.text = textMesh.text + buttonName;
			
			m_AudioSource.Play ();
			StartCoroutine (WaitForNextButton());
		}
	}

	public void OnButtonReleased() {
		m_LastButtonPressed.GetComponent<MeshRenderer> ().material.color = Color.white;
		//m_LastButtonPressed.transform.localPosition = new Vector3 (0.63f, m_LastButtonPressed.transform.localPosition.y, m_LastButtonPressed.transform.localPosition.z);
	}

	private IEnumerator WaitForNextButton() {
		yield return new WaitForSeconds (0.8f);
		m_AudioSource.clip = clipDigit;
		m_NextButton = true;
	}

	private IEnumerator DisplayColor() {
		yield return new WaitForSeconds (1.5f);
		m_Display.material.color = m_ColorDefault;
	}
}
