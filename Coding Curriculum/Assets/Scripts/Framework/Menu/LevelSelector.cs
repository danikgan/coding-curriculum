using UnityEngine;
using System.IO; 
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour {

	public Texture background;
	private string maxpath;
	private int maxlevel;
	private int menus_num = 3;


	void Awake()
    {
		maxpath = Application.dataPath + "/SaveFiles/savemax.txt";
	}

	void OnGUI() {

		GUIStyle button_style = new GUIStyle("Button");
		button_style.fontSize = 40;

		StreamReader reader = new StreamReader(maxpath);
		string maxlevelstring = reader.ReadLine();
		int maxlevel = int.Parse (maxlevelstring);
		reader.Close ();

		GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), background);

		if (GUI.Button (new Rect (Screen.width * 0.0625f, Screen.height * 0.0625f, Screen.width * (1f / 3), Screen.height * (1f / 8)), "Level 1", button_style)) {
			SceneManager.LoadScene ("Level1");
			
		}

		if (GUI.Button (new Rect (Screen.width * 0.0625f, Screen.height * 0.0625f + Screen.height * (1f / 8) + 20, Screen.width * (1f / 3), Screen.height * (1f / 8)), "Level 2",button_style)) {

			if (maxlevel > menus_num) {
				
				SceneManager.LoadScene ("Level2");
			}
		}


		if (GUI.Button (new Rect (Screen.width * 0.7f, Screen.height * 0.7f + Screen.height * (1f / 8) + 20, Screen.width * (1f / 4), Screen.height * (1f / 8)), "Main menu",button_style)) {
			SceneManager.LoadScene ("main_menu");
		}

	}


}
