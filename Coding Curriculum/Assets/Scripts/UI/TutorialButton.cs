using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialButton : MonoBehaviour{

	public Texture background_popup; //popup background
	public Texture content_popup; //popup background
	private bool showpopup = false;
	private Vector2 PopupVector = Vector2.zero;
	public GUIStyle button_style; //style for buttons


	void Update() {

		if (showpopup) {
			if (Input.GetKeyDown (KeyCode.Space))
				showpopup = false;
		}

	}

    void OnMouseDown()
    {
      

		showpopup = true;

    }

	void OnGUI() {

		if (showpopup) {

			//set up buttons position and size
			float button_width_pos = Screen.width * 0.12f;
			float button_width = Screen.width * 0.22f;
			float button_height = Screen.height * 0.14f;
			float button_text_size = Screen.height * 0.04f;

			float content_height = Screen.height;
			float popup_width = Screen.width * 0.5f;


			button_style.fontSize = (int) button_text_size; //set up text size for button

			var index = SceneManager.GetActiveScene().buildIndex; //get current level index
        	index = index - 2; //translate into understandable form
        
			if (index > 1) { //1b level
				index--;
			}

			Tutorial.tutorial_to_display = index;

			Debug.Log (index);

			switch (index) {

			case 1:
				content_height = Screen.height*0.3f;
				break;
				case 2:
				content_height = Screen.width * 0.1f;
				break;
			case 3:
				content_height = Screen.width * 0.1f;
				break;
			case 4:
				content_height = Screen.width * 0.1f;
				break;
			case 5:
				content_height = Screen.width * 0.07f;
				break;
			case 6:
				content_height = Screen.width * 0.1f;
				break;
			case 7:
				content_height = Screen.width * 0.05f;
				break;
			case 8:
				content_height = Screen.width * 0.07f;
				break;
			case 9:
				content_height = Screen.width * 0.05f;
				break;
			case 10:
				content_height = Screen.width * 0.05f;
				break;

			}



			GUI.DrawTexture (new Rect (Screen.width * 0.2f, Screen.height * 0.2f, Screen.width * 0.6f, Screen.height * 0.6f), background_popup); //draw background of popup message

			//start scrolling area
			PopupVector = GUI.BeginScrollView(new Rect (Screen.width * 0.25f, Screen.height * 0.25f, popup_width, Screen.height * 0.4f), PopupVector, new Rect (0, 0, popup_width - 20, content_height + button_height));

			//draw content
			GUI.DrawTexture (new Rect (0,0, popup_width, content_height), content_popup);

			//draw button
			if (GUI.Button (new Rect (button_width_pos, content_height, button_width, button_height), "Tutorial", button_style)) {
				SceneManager.LoadScene("tutorial");  
			}

			GUI.EndScrollView();




		}


	}
}