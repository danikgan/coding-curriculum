using UnityEngine;

public class SceneReferences : MonoBehaviour
{
    public GameObject DragArea;
    public GameObject DropArea;
    public GameObject OuterDragArea;
    public GameObject OuterDropArea;
    public GameObject StartProgramCodeBlock;
    public GameObject Player;
    public GameObject RunStopButton;
    public GameObject Map;
    public GameObject Console;

    public Vector3 MapScale
    {
        get { return Map.transform != null ? Map.transform.lossyScale : new Vector3(1, 1, 1); }
    }
    
    void Awake()
    {
        Player = GameObject.Find("Player");
        OuterDragArea = GameObject.Find("Outer Drag Area");
        OuterDropArea = GameObject.Find("Outer Drop Area");
        DragArea = GameObject.Find("Drag Area");
        DropArea = GameObject.Find("Drop Area");
        RunStopButton = GameObject.Find("RunStopButton");
        Map = GameObject.Find("Map");
        Console = GameObject.Find("Console");
        StartProgramCodeBlock = DropArea.transform.FindChild("StartProgram").gameObject;
    }
}
