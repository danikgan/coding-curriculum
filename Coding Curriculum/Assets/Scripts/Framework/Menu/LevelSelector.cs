using UnityEngine;
using System.Collections;
using System.Text;
using System.IO; 
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour {

	public GUIStyle locked_button_style;
	public GUIStyle unlocked_button_style;
	public GUIStyle menu_button_style;
	public Texture background;
	private string maxpath;
	private int maxlevel;
	private int menus_num = 3; //number of scenes included into menu - do not count as levels


	void Awake(){

		maxpath = Application.persistentDataPath + "/savemax.txt"; //path to savemax.txt file

	}

	void OnGUI() {

		//set up for level buttons
		float levelbutton_width_pos = Screen.width * 0.02f;
		float levelbutton_height_pos = Screen.height * 0.3f;
		float levelbutton_size = Screen.width*0.08f;
		float levelbutton_vert_gap = Screen.width*0.03f;
		float levelbutton_horiz_gap = Screen.height*0.05f;

		//set up for main menu button
		float button_width_pos = Screen.width * 0.7f;
		float button_height_pos = Screen.height * 0.8f;
		float button_width = Screen.width * (1f / 3.3f);
		float button_height = Screen.height * (1f / 5f);
		float button_text_size =  Screen.height * (1f / 19);

		locked_button_style.fontSize = 1;
		unlocked_button_style.fontSize = (int) button_text_size;
		menu_button_style.fontSize = (int) button_text_size;

		//styles to be conditionally defined
		GUIStyle button_style2 = locked_button_style;
		GUIStyle button_style3 = locked_button_style;
		GUIStyle button_style4 = locked_button_style;
		GUIStyle button_style5 = locked_button_style;
		GUIStyle button_style6 = locked_button_style;
		GUIStyle button_style7 = locked_button_style;
		GUIStyle button_style8 = locked_button_style;
		GUIStyle button_style9 = locked_button_style;
		GUIStyle button_style10 = locked_button_style;
		GUIStyle button_style11 = locked_button_style;
		GUIStyle button_style12 = locked_button_style;
		GUIStyle button_style13 = locked_button_style;
		GUIStyle button_style14 = locked_button_style;
		GUIStyle button_style15 = locked_button_style;
		GUIStyle button_style16 = locked_button_style;
		GUIStyle button_style17 = locked_button_style;
		GUIStyle button_style18 = locked_button_style;

	
		StreamReader reader = new StreamReader(maxpath);
		string maxlevelstring = reader.ReadLine();
		int maxlevel = int.Parse (maxlevelstring);
		reader.Close ();
		maxlevel = maxlevel - menus_num + 1;

		GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), background);


	
			
		switch(maxlevel)
		{
			case 2:
				button_style2 = unlocked_button_style;
				break;
			case 3:
				button_style2 = unlocked_button_style;
				button_style3 = unlocked_button_style;
				break;
			case 4:
				button_style2 = unlocked_button_style;
				button_style3 = unlocked_button_style;
				button_style4 = unlocked_button_style;
				break;
			case 5:
				button_style2 = unlocked_button_style;
				button_style3 = unlocked_button_style;
				button_style4 = unlocked_button_style;
				button_style5 = unlocked_button_style;
				break;
			case 6:
				button_style2 = unlocked_button_style;
				button_style3 = unlocked_button_style;
				button_style4 = unlocked_button_style;
				button_style5 = unlocked_button_style;
				button_style6 = unlocked_button_style;
				break;
			case 7:
				button_style2 = unlocked_button_style;
				button_style3 = unlocked_button_style;
				button_style4 = unlocked_button_style;
				button_style5 = unlocked_button_style;
				button_style6 = unlocked_button_style;
				button_style7 = unlocked_button_style;
				break;
			case 8:
				button_style2 = unlocked_button_style;
				button_style3 = unlocked_button_style;
				button_style4 = unlocked_button_style;
				button_style5 = unlocked_button_style;
				button_style6 = unlocked_button_style;
				button_style7 = unlocked_button_style;
				button_style8 = unlocked_button_style;
				break;
			case 9:
				button_style2 = unlocked_button_style;
				button_style3 = unlocked_button_style;
				button_style4 = unlocked_button_style;
				button_style5 = unlocked_button_style;
				button_style6 = unlocked_button_style;
				button_style7 = unlocked_button_style;
				button_style8 = unlocked_button_style;
				button_style9 = unlocked_button_style;
		
				break;
			case 10:
				button_style2 = unlocked_button_style;
				button_style3 = unlocked_button_style;
				button_style4 = unlocked_button_style;
				button_style5 = unlocked_button_style;
				button_style6 = unlocked_button_style;
				button_style7 = unlocked_button_style;
				button_style8 = unlocked_button_style;
				button_style9 = unlocked_button_style;
				button_style10 = unlocked_button_style;
				break;
			case 11:
				button_style2 = unlocked_button_style;
				button_style3 = unlocked_button_style;
				button_style4 = unlocked_button_style;
				button_style5 = unlocked_button_style;
				button_style6 = unlocked_button_style;
				button_style7 = unlocked_button_style;
				button_style8 = unlocked_button_style;
				button_style9 = unlocked_button_style;
				button_style10 = unlocked_button_style;
				button_style11 = unlocked_button_style;
				break;
			case 12:
				button_style2 = unlocked_button_style;
				button_style3 = unlocked_button_style;
				button_style4 = unlocked_button_style;
				button_style5 = unlocked_button_style;
				button_style6 = unlocked_button_style;
				button_style7 = unlocked_button_style;
				button_style8 = unlocked_button_style;
				button_style9 = unlocked_button_style;
				button_style10 = unlocked_button_style;
				button_style11 = unlocked_button_style;
				button_style12 = unlocked_button_style;
				break;
			case 13:
				button_style2 = unlocked_button_style;
				button_style3 = unlocked_button_style;
				button_style4 = unlocked_button_style;
				button_style5 = unlocked_button_style;
				button_style6 = unlocked_button_style;
				button_style7 = unlocked_button_style;
				button_style8 = unlocked_button_style;
				button_style9 = unlocked_button_style;
				button_style10 = unlocked_button_style;
				button_style11 = unlocked_button_style;
				button_style12 = unlocked_button_style;
				button_style13 = unlocked_button_style;
				break;
			case 14:
				button_style2 = unlocked_button_style;
				button_style3 = unlocked_button_style;
				button_style4 = unlocked_button_style;
				button_style5 = unlocked_button_style;
				button_style6 = unlocked_button_style;
				button_style7 = unlocked_button_style;
				button_style8 = unlocked_button_style;
				button_style9 = unlocked_button_style;
				button_style10 = unlocked_button_style;
				button_style11 = unlocked_button_style;
				button_style12 = unlocked_button_style;
				button_style13 = unlocked_button_style;
				button_style14 = unlocked_button_style;
				break;
			case 15:
				button_style2 = unlocked_button_style;
				button_style3 = unlocked_button_style;
				button_style4 = unlocked_button_style;
				button_style5 = unlocked_button_style;
				button_style6 = unlocked_button_style;
				button_style7 = unlocked_button_style;
				button_style8 = unlocked_button_style;
				button_style9 = unlocked_button_style;
				button_style10 = unlocked_button_style;
				button_style11 = unlocked_button_style;
				button_style12 = unlocked_button_style;
				button_style13 = unlocked_button_style;
				button_style14 = unlocked_button_style;
				button_style15 = unlocked_button_style;
				break;
			case 16:
				button_style2 = unlocked_button_style;
				button_style3 = unlocked_button_style;
				button_style4 = unlocked_button_style;
				button_style5 = unlocked_button_style;
				button_style6 = unlocked_button_style;
				button_style7 = unlocked_button_style;
				button_style8 = unlocked_button_style;
				button_style9 = unlocked_button_style;
				button_style10 = unlocked_button_style;
				button_style11 = unlocked_button_style;
				button_style12 = unlocked_button_style;
				button_style13 = unlocked_button_style;
				button_style14 = unlocked_button_style;
				button_style15 = unlocked_button_style;
				button_style16 = unlocked_button_style;
				break;
			case 17:
				button_style2 = unlocked_button_style;
				button_style3 = unlocked_button_style;
				button_style4 = unlocked_button_style;
				button_style5 = unlocked_button_style;
				button_style6 = unlocked_button_style;
				button_style7 = unlocked_button_style;
				button_style8 = unlocked_button_style;
				button_style9 = unlocked_button_style;
				button_style10 = unlocked_button_style;
				button_style11 = unlocked_button_style;
				button_style12 = unlocked_button_style;
				button_style13 = unlocked_button_style;
				button_style14 = unlocked_button_style;
				button_style15 = unlocked_button_style;
				button_style16 = unlocked_button_style;
				button_style17 = unlocked_button_style;
				break;
			case 18:
				button_style2 = unlocked_button_style;
				button_style3 = unlocked_button_style;
				button_style4 = unlocked_button_style;
				button_style5 = unlocked_button_style;
				button_style6 = unlocked_button_style;
				button_style7 = unlocked_button_style;
				button_style8 = unlocked_button_style;
				button_style9 = unlocked_button_style;
				button_style10 = unlocked_button_style;
				button_style11 = unlocked_button_style;
				button_style12 = unlocked_button_style;
				button_style13 = unlocked_button_style;
				button_style14 = unlocked_button_style;
				button_style15 = unlocked_button_style;
				button_style16 = unlocked_button_style;
				button_style17 = unlocked_button_style;
				button_style18 = unlocked_button_style;
				break;
		}


		if (GUI.Button (new Rect (levelbutton_width_pos, levelbutton_height_pos, levelbutton_size, levelbutton_size), "1", unlocked_button_style)) {
			SceneManager.LoadScene ("Level1");

		}
			
		if (GUI.Button (new Rect (levelbutton_width_pos + levelbutton_size + levelbutton_vert_gap, levelbutton_height_pos, levelbutton_size, levelbutton_size), "2", button_style2)) {
				
			if(maxlevel >= 2)
				SceneManager.LoadScene ("Level2");
		}
				

		if (GUI.Button (new Rect (levelbutton_width_pos + (levelbutton_size + levelbutton_vert_gap)*2, levelbutton_height_pos, levelbutton_size, levelbutton_size), "3", button_style3)) {

			if(maxlevel >= 3)
				SceneManager.LoadScene ("Level3");
		}

		if (GUI.Button (new Rect (levelbutton_width_pos + (levelbutton_size + levelbutton_vert_gap)*3, levelbutton_height_pos, levelbutton_size, levelbutton_size), "4", button_style4)) {

		if(maxlevel >= 4)
			SceneManager.LoadScene ("Level4");
		}

		if (GUI.Button (new Rect (levelbutton_width_pos + (levelbutton_size + levelbutton_vert_gap)*4, levelbutton_height_pos, levelbutton_size, levelbutton_size), "5",button_style5)) {

		if(maxlevel >= 5)
			SceneManager.LoadScene ("Level5");
		}

		if (GUI.Button (new Rect (levelbutton_width_pos + (levelbutton_size + levelbutton_vert_gap)*5, levelbutton_height_pos, levelbutton_size, levelbutton_size), "6",button_style6)) {
		
		if(maxlevel >= 6)
			SceneManager.LoadScene ("Level6");
		}

		if (GUI.Button (new Rect (levelbutton_width_pos + (levelbutton_size + levelbutton_vert_gap)*6, levelbutton_height_pos, levelbutton_size, levelbutton_size), "7", button_style7)) {

		if(maxlevel >= 7)
			SceneManager.LoadScene ("Level7");
		}

		if (GUI.Button (new Rect (levelbutton_width_pos + (levelbutton_size + levelbutton_vert_gap)*7, levelbutton_height_pos, levelbutton_size, levelbutton_size), "8", button_style8)) {

		if(maxlevel >= 8)
			SceneManager.LoadScene ("Level8");
		}

		if (GUI.Button (new Rect (levelbutton_width_pos + (levelbutton_size + levelbutton_vert_gap)*8, levelbutton_height_pos, levelbutton_size, levelbutton_size), "9", button_style9)) {

		if(maxlevel >= 9)
			SceneManager.LoadScene ("Level9");
		}
			
		if (GUI.Button (new Rect (levelbutton_width_pos, levelbutton_height_pos + levelbutton_size + levelbutton_horiz_gap, levelbutton_size, levelbutton_size), "10", button_style10)) {

			if(maxlevel >= 10)
				SceneManager.LoadScene ("Level10");
		}	

		if (GUI.Button (new Rect (levelbutton_width_pos + levelbutton_size + levelbutton_vert_gap, levelbutton_height_pos + levelbutton_size + levelbutton_horiz_gap, levelbutton_size, levelbutton_size), "11", button_style11)) {

			if(maxlevel >= 11)
				SceneManager.LoadScene ("Level11");
		}


		if (GUI.Button (new Rect (levelbutton_width_pos + (levelbutton_size + levelbutton_vert_gap)*2, levelbutton_height_pos + levelbutton_size + levelbutton_horiz_gap, levelbutton_size, levelbutton_size), "12", button_style12)) {

			if(maxlevel >= 12)
				SceneManager.LoadScene ("Level12");
		}

		if (GUI.Button (new Rect (levelbutton_width_pos + (levelbutton_size + levelbutton_vert_gap)*3, levelbutton_height_pos + levelbutton_size + levelbutton_horiz_gap, levelbutton_size, levelbutton_size), "13", button_style13)) {

			if(maxlevel >= 13)
				SceneManager.LoadScene ("Level13");
		}

		if (GUI.Button (new Rect (levelbutton_width_pos + (levelbutton_size + levelbutton_vert_gap)*4, levelbutton_height_pos + levelbutton_size + levelbutton_horiz_gap, levelbutton_size, levelbutton_size), "14",button_style14)) {

			if(maxlevel >= 14)
				SceneManager.LoadScene ("Level14");
		}

		if (GUI.Button (new Rect (levelbutton_width_pos + (levelbutton_size + levelbutton_vert_gap)*5, levelbutton_height_pos + levelbutton_size + levelbutton_horiz_gap, levelbutton_size, levelbutton_size), "15",button_style15)) {

			if(maxlevel >= 15)
				SceneManager.LoadScene ("Level15");
		}

		if (GUI.Button (new Rect (levelbutton_width_pos + (levelbutton_size + levelbutton_vert_gap)*6, levelbutton_height_pos + levelbutton_size + levelbutton_horiz_gap, levelbutton_size, levelbutton_size), "16", button_style16)) {

			if(maxlevel >= 16)
				SceneManager.LoadScene ("Level16");
		}

		if (GUI.Button (new Rect (levelbutton_width_pos + (levelbutton_size + levelbutton_vert_gap)*7, levelbutton_height_pos + levelbutton_size + levelbutton_horiz_gap, levelbutton_size, levelbutton_size), "17", button_style17)) {

			if(maxlevel >= 17)
				SceneManager.LoadScene ("Level17");
		}

		if (GUI.Button (new Rect (levelbutton_width_pos + (levelbutton_size + levelbutton_vert_gap)*8, levelbutton_height_pos + levelbutton_size + levelbutton_horiz_gap, levelbutton_size, levelbutton_size), "18", button_style18)) {

			if(maxlevel >= 18)
				SceneManager.LoadScene ("Level18");
		}





		if (GUI.Button (new Rect (button_width_pos, button_height_pos, button_width, button_height), "Main menu",menu_button_style)) {
			SceneManager.LoadScene ("main_menu");
		}

	}


}
