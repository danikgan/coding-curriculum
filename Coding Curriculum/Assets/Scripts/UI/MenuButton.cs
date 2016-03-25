using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
    void OnMouseDown()
    {
        SceneManager.LoadScene("main_menu");
    }
}
