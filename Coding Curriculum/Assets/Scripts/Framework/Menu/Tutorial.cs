using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour {

	public Texture background; //general background
	public GUISkin tutorial_skin = null; //set up skin
	public GUIStyle menu_button_style;

	private Vector2 scrollPositionButtons = Vector2.zero; //vector for scroll buttons scroll area
	private Vector2 scrollPositionText = Vector2.zero; //vector for scroll text  scroll area
	private int tutorial_to_display = 1; //which tutorial will be displayed by default
	private int num_buttons = 6; //number of buttons in scrollable area
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
		float text_scroll_box_height = Screen.width;


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
		Rect textContentRect = new Rect (0, 0, text_scroll_hor_size - 20, text_scroll_box_height); //set text scroll rect size



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

		GUI.EndScrollView();



		//start text scroll
		scrollPositionText = GUI.BeginScrollView(new Rect(text_scroll_hor_pos, text_scroll_vert_pos, text_scroll_hor_size, text_scroll_vert_size), scrollPositionText, textContentRect);

		if (tutorial_to_display == 1) {
			GUI.Label (textContentRect, "Lorem ipsum dolor sit amet, consectetur adipiscing elit. In eu enim quis erat vestibulum gravida eu sit amet magna. Phasellus aliquam bibendum ipsum et aliquet. Aliquam at dolor ut ante tincidunt pulvinar quis eget diam. Maecenas a quam quis justo consequat sollicitudin tempor non libero. Curabitur molestie hendrerit leo quis facilisis. Proin in fringilla lorem, eget gravida mi. Vivamus ut posuere urna, id rhoncus diam. Ut semper porta leo, ut eleifend est consectetur gravida. Vestibulum dolor quam, pretium a rhoncus ut, venenatis quis urna. Integer vitae mi ac urna sagittis volutpat. Morbi scelerisque turpis vel interdum fermentum. Curabitur porta ipsum ante, eget scelerisque nunc auctor sit amet. Sed nisi lectus, facilisis porta ligula eget, varius commodo sapien. In sollicitudin velit eget neque eleifend bibendum.\n\nMaecenas rutrum euismod porttitor. Sed congue pharetra orci, sed accumsan leo volutpat ut. Integer maximus maximus pretium. Donec ut ultricies est, quis porttitor sem. In hac habitasse platea dictumst. Mauris consequat aliquam justo molestie placerat. Praesent ornare vitae nulla elementum ultrices. Maecenas sit amet massa non nisi vulputate venenatis maximus vitae quam. Pellentesque mollis metus ac condimentum consequat. Duis quis odio nisi.\n\nNullam et sem vitae mauris vehicula mollis. Sed mattis dui lorem, at iaculis risus rhoncus non. Suspendisse nec accumsan lorem. Fusce eget varius urna, in facilisis ante. Vestibulum viverra dolor orci, et aliquet sem maximus ac. Nam vulputate elit sapien, rutrum semper ligula vulputate quis. Integer tincidunt a tellus ut pellentesque. Mauris vestibulum arcu vel ligula mattis iaculis. Vivamus at elit pellentesque, imperdiet orci eget, rutrum est. Pellentesque vestibulum quam sit amet viverra vestibulum. Nam accumsan libero commodo orci blandit, in blandit nisi malesuada. Sed pretium finibus ligula eget rhoncus. Phasellus ut felis neque. Integer sodales sollicitudin leo, at pretium elit vehicula id.\n\n");
		}

		if (tutorial_to_display == 2) {
			GUI.Label (textContentRect, "Nullam et sem vitae mauris vehicula mollis. Sed mattis dui lorem, at iaculis risus rhoncus non. Suspendisse nec accumsan lorem. Fusce eget varius urna, in facilisis ante. Vestibulum viverra dolor orci, et aliquet sem maximus ac. Nam vulputate elit sapien, rutrum semper ligula vulputate quis. Integer tincidunt a tellus ut pellentesque. Mauris vestibulum arcu vel ligula mattis iaculis. Vivamus at elit pellentesque, imperdiet orci eget, rutrum est. Pellentesque vestibulum quam sit amet viverra vestibulum. Nam accumsan libero commodo orci blandit, in blandit nisi malesuada. Sed pretium finibus ligula eget rhoncus. Phasellus ut felis neque. Integer sodales sollicitudin leo, at pretium elit vehicula id.\n\nDonec bibendum eu mi non consequat. Suspendisse lobortis laoreet felis sit amet laoreet. Quisque ante magna, sollicitudin sed semper quis, tempus ac ante. Nam neque mauris, rutrum ut risus sed, hendrerit vestibulum purus. Nam a massa at odio laoreet porttitor eget in ligula. Ut fermentum auctor ligula vel aliquet. Mauris est libero, dapibus sed metus ut, fermentum dapibus tortor. Nullam mattis justo eget tellus tempus ullamcorper. Vivamus in semper erat.\n\nIn viverra eros laoreet, mollis augue id, interdum est. Sed arcu massa, condimentum a luctus sed, tempus dignissim urna. Nullam sagittis mauris leo, venenatis lobortis lectus cursus sollicitudin. Proin porta ligula et tristique iaculis. Sed sodales euismod metus vitae malesuada. Aliquam consequat aliquam neque, sit amet sollicitudin arcu luctus at. Nam ac purus vitae ipsum pellentesque placerat.\n\n");
		}

		GUI.EndScrollView();

		if (GUI.Button (new Rect (Screen.width*0.003f, Screen.height*0.8f, button_width, button_height), "Main menu",menu_button_style)) {
			SceneManager.LoadScene ("main_menu");
		}


	}

}
