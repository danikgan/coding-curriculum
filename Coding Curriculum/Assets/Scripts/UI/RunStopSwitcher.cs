using UnityEngine;

public class RunStopSwitcher : MonoBehaviour
{
    public GameObject RunButton;
    public GameObject StopButton;

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
