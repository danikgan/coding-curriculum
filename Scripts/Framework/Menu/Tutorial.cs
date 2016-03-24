using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{

    public Texture background;

    void OnGUI()
    {

        GUIStyle button_style = new GUIStyle("Button");
        button_style.fontSize = 40;
        GUIStyle label_style = new GUIStyle("label");
        label_style.fontSize = 40;

        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), background);

        GUI.Label(new Rect(50, 50, 250, 600), "No tutorials yet, just enjoy the boobs", label_style);

        if (
            GUI.Button(
                new Rect(Screen.width*0.7f, Screen.height*0.7f + Screen.height*(1f/8) + 20, Screen.width*(1f/4),
                    Screen.height*(1f/8)), "Main menu", button_style))
        {
            SceneManager.LoadScene("main_menu");
        }
    }
}
