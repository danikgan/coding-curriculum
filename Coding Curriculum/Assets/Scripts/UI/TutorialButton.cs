using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TutorialButton : MonoBehaviour {

	public GUIStyle style;

	void OnGUI () {

		GUI.skin.button = style;

		int index = SceneManager.GetActiveScene().buildIndex; //get current level index
		index = index - 2; //translate into understandable form

		if (GUI.Button (new Rect (Screen.width*0.076f, Screen.height*0.683f, Screen.width*0.055f, Screen.width*0.055f), "")) {

			if (index > 1) //1b level
				index--;

			Tutorial.tutorial_to_display = index; 
			SceneManager.LoadScene ("tutorial");
		}

	}

}