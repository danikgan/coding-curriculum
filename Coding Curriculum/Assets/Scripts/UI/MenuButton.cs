using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour {

	public GUIStyle style;
		
	void OnGUI () {

		GUI.skin.button = style;

		if (GUI.Button (new Rect (Screen.width*0.329f, Screen.height*0.06f, Screen.width*0.055f, Screen.width*0.055f), "")) {
			SceneManager.LoadScene ("main_menu");
		}
	
	}

}
