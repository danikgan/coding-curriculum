using UnityEngine;
using System.Collections;
using System.Text;
using System.IO; 
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour {

	public Texture background;
	private string maxpath;
	private int maxlevel;

	void Awake(){

		maxpath = Application.dataPath + "/SaveFiles/savemax.txt";

	}

	void OnGUI() {


		GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), background);


	}


}
