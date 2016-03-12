using UnityEngine;
using System.Collections;
using System.Text;
using System.IO; 
using UnityEngine.SceneManagement;
using UnityEditor;

public class MainMenu : MonoBehaviour {

	public Texture background;
	private string path;
	private int num_scenes;

	void Awake(){

	path = Application.dataPath + "/SaveFiles/save.txt";

	}


	void OnGUI(){

		num_scenes = SceneManager.sceneCountInBuildSettings;

		GUIStyle button_style = new GUIStyle("Button");
		button_style.fontSize = 40;

		GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), background);

		if (GUI.Button (new Rect (Screen.width * 0.0625f, Screen.height * 0.0625f, Screen.width * (1f / 3), Screen.height * (1f / 8)), "Play",button_style)) {

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

		if (GUI.Button (new Rect (Screen.width * 0.0625f, Screen.height * 0.0625f + Screen.height * (1f / 8) + 20, Screen.width * (1f / 3), Screen.height * (1f / 8)), "Level selector",button_style)) {
			SceneManager.LoadScene ("level_selector");
		}

		if (GUI.Button (new Rect (Screen.width * 0.0625f, Screen.height * 0.0625f + (Screen.height * (1f / 8) + 20)*2, Screen.width * (1f / 3), Screen.height * (1f / 8)), "Tutorial",button_style)) {
			SceneManager.LoadScene ("tutorial");
		}

		if (GUI.Button (new Rect (Screen.width * 0.0625f, Screen.height * 0.0625f + (Screen.height * (1f / 8) + 20)*3, Screen.width * (1f / 3), Screen.height * (1f / 8)), "Quit",button_style)) {
			//Application.Quit (); //uncomment when the app is compiled
			EditorApplication.isPlaying = false; //exit play mode
		
		}

	}
}
