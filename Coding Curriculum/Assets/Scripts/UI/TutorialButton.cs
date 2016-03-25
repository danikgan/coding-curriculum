using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialButton : MonoBehaviour
{
    void OnMouseDown()
    {
        var index = SceneManager.GetActiveScene().buildIndex; //get current level index
        index = index - 2; //translate into understandable form
        
        if (index > 1) //1b level
            index--;

        Tutorial.tutorial_to_display = index;
        SceneManager.LoadScene("tutorial");
    }
}