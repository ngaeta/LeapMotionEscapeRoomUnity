using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour {

	public GameObject riga3Right, riga3Left;
	public Text Hint;
	private Image imageRight, imageLeft;
	private Text textImageLeft, textImageRight;


	void Start() {
		Hint.gameObject.SetActive (false);
		imageLeft = riga3Left.transform.Find ("Image").GetComponent<Image> ();
		textImageLeft = riga3Left.transform.Find ("Text").GetComponent<Text>();
		riga3Left.SetActive(false);
		imageRight = riga3Right.transform.Find ("Image").GetComponent<Image> ();
		textImageRight = riga3Right.transform.Find ("Text").GetComponent<Text>();
		riga3Right.SetActive(false);
	}

	public void ActiveHint(string messageHint) {
	
		Hint.text = messageHint;
		Hint.gameObject.SetActive (true);
	}

	public void DeactiveHint() {
		Hint.gameObject.SetActive (false);
	}

	public void ActiveLineRight(Sprite image, string message) {
		imageRight.sprite = image;
		textImageRight.text = message;
		riga3Right.SetActive (true);
	}

	public void ActiveLineLeft(Sprite image, string message) {

		imageLeft.sprite = image;
		textImageLeft.text = message;
		riga3Left.SetActive (true);
	}

	public void DeactiveLineRight() {
		riga3Right.SetActive (false);
	}

	public void DeactiveLineLeft() {
		riga3Left.SetActive (false);
	}
}
