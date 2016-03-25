using UnityEngine;

public class RunStopSwitcher : MonoBehaviour
{
    public GameObject RunButton;
    public GameObject StopButton;
    private SceneReferences _referencesScript;
    private Compiler _compiler;

    void Awake()
    {
        var mainCamera = GameObject.Find("Main Camera");
        if (mainCamera)
            _referencesScript = mainCamera.GetComponent<SceneReferences>();
        else
            Debug.LogError("Error: Main Camera not found");

        _compiler = _referencesScript.RunStopButton.GetComponent<Compiler>();
    }

    void OnMouseDown()
    {
        _compiler.ReverseCompilerStatus();
    }

    public void ShowRun()
    {
        StopButton.GetComponent<SpriteRenderer>().enabled = false;
        RunButton.GetComponent<SpriteRenderer>().enabled = true;
    }

    public void ShowStop()
    {
        StopButton.GetComponent<SpriteRenderer>().enabled = true;
        RunButton.GetComponent<SpriteRenderer>().enabled = false;
    }
}
