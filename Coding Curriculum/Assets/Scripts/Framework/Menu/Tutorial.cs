using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour {

	public Texture background; //general background

	public Texture tut1; //textures for tutorial context
	public Texture tut2;
	public Texture tut3;
	public Texture tut4;
	public Texture tut5;
	public Texture tut6;
	public Texture tut7;
	public Texture tut8;

	public GUISkin tutorial_skin = null; //set up skin
	public GUIStyle menu_button_style;
	private Vector2 scrollPositionButtons = Vector2.zero; //vector for scroll buttons scroll area
	private Vector2 scrollPositionText = Vector2.zero; //vector for scroll text  scroll area
	public static int tutorial_to_display = 1; //which tutorial will be displayed by default
	private int num_buttons = 8; //number of buttons in scrollable area
	private int tutorial_text_size;

	void OnGUI () {

		GUI.skin = tutorial_skin; //attach skin
		
		GUI.DrawTexture ( new Rect (0, 0, Screen.width, Screen.height), background); //draw background texture

		tutorial_text_size = (int) (Screen.height * 0.03f);
		tutorial_skin.label.fontSize = tutorial_text_size;

		//setups for button scrollview
		float button_scroll_hor_pos = Screen.width * 0.005f;
		float button_scroll_vert_pos = Screen.height * 0.02f;
		float button_scroll_hor_size = Screen.width * 0.4f;
		float button_scroll_vert_size = Screen.height * 0.8f;
		float gap = Screen.width * 0.06f;


		//setups for text scrollview
		float text_scroll_hor_pos = button_scroll_hor_pos + gap + button_scroll_hor_size;
		float text_scroll_vert_pos = button_scroll_vert_pos;
		float text_scroll_hor_size = Screen.width*0.53f;
		float text_scroll_vert_size = Screen.height*0.9f;


		//set up buttons position and size
		float button_width_pos = 0;
		float button_height_pos = 0;
		float button_width = Screen.width * (1f / 3f);
		float button_height = Screen.height * (1f / 5f);
		float button_gap = Screen.height * (1f / 320f);
		float button_text_size = Screen.height * (1f / 19);

		menu_button_style.fontSize = (int) button_text_size; //set up style for main menu button
		tutorial_skin.button.fontSize = (int) button_text_size; 

		Rect buttonContentRect = new Rect(0, 0, button_scroll_hor_size-20, num_buttons*(button_height+button_gap)); //set button scroll rect size



		//start button scroll
		scrollPositionButtons = GUI.BeginScrollView(new Rect(button_scroll_hor_pos, button_scroll_vert_pos, button_scroll_hor_size, button_scroll_vert_size), scrollPositionButtons, buttonContentRect);


		if (GUI.Button (new Rect (button_width_pos, button_height_pos, button_width, button_height), "Tutorial 1")) {
			tutorial_to_display = 1; 
		}
			
		if (GUI.Button (new Rect (button_width_pos, button_height_pos + button_height + button_gap, button_width, button_height), "Tutorial 2")) {
			tutorial_to_display = 2; 
		}

		if (GUI.Button (new Rect (button_width_pos, button_height_pos + (button_height + button_gap)*2, button_width, button_height), "Tutorial 3")) {
			tutorial_to_display = 3; 
		}

		if (GUI.Button (new Rect (button_width_pos, button_height_pos + (button_height + button_gap)*3, button_width, button_height), "Tutorial 4")) {
			tutorial_to_display = 4; 
		}

		if (GUI.Button (new Rect (button_width_pos, button_height_pos + (button_height + button_gap)*4, button_width, button_height), "Tutorial 5")) {
			tutorial_to_display = 5; 
		}

		if (GUI.Button (new Rect (button_width_pos, button_height_pos + (button_height + button_gap)*5, button_width, button_height), "Tutorial 6")) {
			tutorial_to_display = 6; 
		}

		if (GUI.Button (new Rect (button_width_pos, button_height_pos + (button_height + button_gap)*6, button_width, button_height), "Tutorial 7")) {
			tutorial_to_display = 7; 
		}

		if (GUI.Button (new Rect (button_width_pos, button_height_pos + (button_height + button_gap)*7, button_width, button_height), "Tutorial 8")) {
			tutorial_to_display = 8; 
		}

		GUI.EndScrollView();

		float text_scroll_box_height = Screen.width * 0.7f;

		switch (tutorial_to_display) {

		case 1:
			text_scroll_box_height = Screen.width * 0.67f;
			break;
		case 2:
			text_scroll_box_height = Screen.width * 0.5f;
			break;
		case 3:
			text_scroll_box_height = Screen.width * 0.55f;
			break;
		case 4:
			text_scroll_box_height = Screen.width * 0.6f;
			break;
		case 5:
			text_scroll_box_height = Screen.width * 0.6f;
			break;
		case 6:
			text_scroll_box_height = Screen.width * 0.5f;
			break;
		case 7:
			text_scroll_box_height = Screen.width * 0.6f;
			break;
		case 8:
			text_scroll_box_height = Screen.width * 0.45f;
			break;

		}




		Rect textContentRect = new Rect (0, 0, text_scroll_hor_size - 20, text_scroll_box_height); //set text scroll rect size

		//start text scroll
		scrollPositionText = GUI.BeginScrollView(new Rect(text_scroll_hor_pos, text_scroll_vert_pos, text_scroll_hor_size, text_scroll_vert_size), scrollPositionText, textContentRect);

		if (tutorial_to_display == 1) {
			GUI.DrawTexture (new Rect (0, 0, text_scroll_hor_size - 20, Screen.width*0.67f), tut1);	
		}

		if (tutorial_to_display == 2) {
			GUI.DrawTexture (new Rect (0, 0, text_scroll_hor_size - 20, Screen.width*0.5f), tut2);	
		}

		if (tutorial_to_display == 3) {
			GUI.DrawTexture (new Rect (0, 0, text_scroll_hor_size - 20, Screen.width*0.55f), tut3);	
		}

		if (tutorial_to_display == 4) {
			GUI.DrawTexture (new Rect (0, 0, text_scroll_hor_size - 20, Screen.width*0.6f), tut4);	
		}

		if (tutorial_to_display == 5) {
			GUI.DrawTexture (new Rect (0, 0, text_scroll_hor_size - 20, Screen.width*0.6f), tut5);	
		}

		if (tutorial_to_display == 6) {
			GUI.DrawTexture (new Rect (0, 0, text_scroll_hor_size - 20, Screen.width*0.5f), tut6);	
		}

		if (tutorial_to_display == 7) {
			GUI.DrawTexture (new Rect (0, 0, text_scroll_hor_size - 20, Screen.width*0.6f), tut7);	
		}

		if (tutorial_to_display == 8) {
			GUI.DrawTexture (new Rect (0, 0, text_scroll_hor_size - 20, Screen.width*0.45f), tut8);	
		}

		GUI.EndScrollView();

		if (GUI.Button (new Rect (Screen.width*0.003f, Screen.height*0.8f, button_width, button_height), "Main menu",menu_button_style)) {
			SceneManager.LoadScene ("main_menu");
		}


	}

}
