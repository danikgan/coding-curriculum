using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Popups : MonoBehaviour {


	public Texture background_popup; //popup background
	public Texture content_popup; //popup background
	private bool showpopup;
	private Vector2 FirstPopup = Vector2.zero;


	void Start () {

		showpopup = true;

	}

	void Update() {

		if (Input.GetKeyDown (KeyCode.Space)) {
			showpopup = false;
			}

	}

	void OnGUI() {

		var index = SceneManager.GetActiveScene().buildIndex; //get current level index
		index = index - 2; //translate into understandable form

		float content_height = Screen.height;

		switch (index) {

		case 1:
			content_height = Screen.height*1.8f;
			break;
			case 2:
			content_height = Screen.width * 0.8f;
				break;
			case 3:
			content_height = Screen.width * 0.7f;
				break;
			case 4:
			content_height = Screen.width * 0.05f;
				break;
			case 5:
			content_height = Screen.width * 0.03f;
				break;
			case 6:
			content_height = Screen.width * 0.06f;
				break;
			case 7:
			content_height = Screen.width * 0.06f;
				break;
			case 8:
			content_height = Screen.width * 0.33f;
				break;
			case 9:
			content_height = Screen.width * 0.04f;
				break;
			case 10:
			content_height = Screen.width * 0.23f;
				break;
			case 11:
			content_height = Screen.width * 0.07f;
				break;

		}


		if (showpopup) {
			GUI.DrawTexture (new Rect (Screen.width * 0.2f, Screen.height * 0.2f, Screen.width * 0.6f, Screen.height * 0.6f), background_popup);

			FirstPopup = GUI.BeginScrollView(new Rect (Screen.width * 0.25f, Screen.height * 0.25f, Screen.width * 0.5f, Screen.height * 0.4f), FirstPopup, new Rect (0, 0, Screen.width * 0.5f - 20, content_height));

			GUI.DrawTexture (new Rect (0,0, Screen.width * 0.5f, content_height), content_popup);


			GUI.EndScrollView();


		}


	}

	IEnumerator WaitForKeyDown(KeyCode keyCode)
	{
		while (!Input.GetKeyDown(keyCode))
			yield return null;
		showpopup = false;

	}
}
