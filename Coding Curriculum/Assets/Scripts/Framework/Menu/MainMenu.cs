using UnityEngine;
using System.Collections;
using System.Text;
using System.IO; 
using UnityEngine.SceneManagement;
using UnityEditor;

public class MainMenu : MonoBehaviour {

	public GUISkin main_menu_skin = null; //set up skin
	public Texture background; //main background
	public Texture header; //main background

	private string path;
	private int num_scenes;

	void Awake(){

	path = Application.dataPath + "/SaveFiles/save.txt"; //path to save.txt file

	}


	void OnGUI(){

		//set up buttons position and size
		float button_width_pos = Screen.width * 0.008f;
		float button_height_pos = Screen.width * 0.07f;
		float button_width = Screen.width * (1f / 2.15f);
		float button_height = Screen.height * (1f / 5f);
		float button_gap = Screen.height * (1f / 300f);
		float button_text_size = Screen.height * (1f / 19);

		//set up header position and size
		float header_width_pos = Screen.width * 0.47f;
		float header_height_pos = Screen.height*0.1f;
		float header_width = Screen.width/2f;
		float header_height = Screen.height/6.7f;


		
		Rect windowRect = new Rect (0, 0, Screen.width, Screen.height); //set up rectangle of full screen size
		Rect headerRect = new Rect (header_width_pos, header_height_pos, header_width, header_height); //rectangle for header


		GUI.skin = main_menu_skin; //assign skin
		GUI.skin.button.fontSize = (int) button_text_size; //set text size for buttons

		num_scenes = SceneManager.sceneCountInBuildSettings; //number of scenes in the game

		GUI.DrawTexture (windowRect, background); //draw background texture
		GUI.DrawTexture (headerRect, header); //draw header

		if (GUI.Button (new Rect (button_width_pos, button_height_pos, button_width, button_height), "Play")) {

			StreamReader reader = new StreamReader(path);
			string levelstring = reader.ReadLine();
			int level = int.Parse (levelstring);
			reader.Close ();

			Debug.Log (level);
			Debug.Log (num_scenes);

			if (level + 1 > num_scenes || level < 3) {
				SceneManager.LoadScene ("Level1");
			} else {

				SceneManager.LoadScene (level);
			}


		}

		if (GUI.Button (new Rect (button_width_pos, button_height_pos + button_height + button_gap, button_width, button_height), "Level selector")) {
			SceneManager.LoadScene ("level_selector");
		}

		if (GUI.Button (new Rect (button_width_pos, button_height_pos + (button_height + button_gap)*2, button_width, button_height), "Tutorial")) {
			SceneManager.LoadScene ("tutorial");
		}

		if (GUI.Button (new Rect (button_width_pos, button_height_pos + (button_height + button_gap)*3, button_width, button_height), "Quit")) {
			//Application.Quit (); //uncomment when the app is compiled
			EditorApplication.isPlaying = false; //exit play mode

		}




	}
		
}
