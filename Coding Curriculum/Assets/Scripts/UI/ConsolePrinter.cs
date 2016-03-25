using System;
using UnityEngine;
using UnityEngine.UI;

public class ConsolePrinter : MonoBehaviour
{
    private Text _textField;

    void Awake()
    {
        _textField = transform.FindChild("Console Text Field").gameObject.GetComponent<Text>();
        _textField.supportRichText = true;
    }

    void Start()
    {
        ResetConsole();
    }

    public void ResetConsole()
    {
        _textField.text = "";
        PushText("Press run when you are ready to start!", "green");
    }

    public void PushText(string text, string colour)
    {
        _textField.text += "<color=" + colour + ">" + text + "</color>" + "\n";
    }

    public void PushText(string text)
    {
        PushText(text, "black");
    }

    public void PushWarning(string text)
    {
        PushText(text, "black");
    }

    public void PushError(string text)
    {
        PushText(text, "red");
    }
}
