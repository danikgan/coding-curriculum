using UnityEngine;
using System.Collections;

public class Popups : MonoBehaviour {


	public Texture background_popup; //popup background
	public Texture content_popup; //popup background
	private bool showpopup = true;
	private Vector2 FirstPopup = Vector2.zero;

	void Update() {

		if (Input.GetKeyDown(KeyCode.Space)) 
			showpopup = false;

	}

	void OnGUI() {

		if (showpopup) {
			GUI.DrawTexture (new Rect (Screen.width * 0.2f, Screen.height * 0.2f, Screen.width * 0.6f, Screen.height * 0.6f), background_popup);

			FirstPopup = GUI.BeginScrollView(new Rect (Screen.width * 0.25f, Screen.height * 0.25f, Screen.width * 0.5f, Screen.height * 0.4f), FirstPopup, new Rect (0, 0, Screen.width * 0.5f - 20, Screen.height));

			GUI.DrawTexture (new Rect (0,0, Screen.width * 0.5f, Screen.height), content_popup);


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
