using UnityEngine;

public class SceneReferences : MonoBehaviour
{
    public GameObject DragArea;
    public GameObject DropArea;
    public GameObject OuterDragArea;
    public GameObject OuterDropArea;
    public GameObject StartProgramCodeBlock;
    public GameObject Player;
    public GameObject MainCanvas;
    public GameObject RunButton;

    private RectTransform _mainCanvasRectTransform;

    public Vector3 MainCanvasScale
    {
        get { return _mainCanvasRectTransform != null ? _mainCanvasRectTransform.lossyScale : new Vector3(1, 1, 1); }
    }

    void Start()
    {
        Player = GameObject.Find("Player");
        MainCanvas = GameObject.Find("Main Canvas");
        OuterDragArea = GameObject.Find("Outer Drag Area");
        OuterDropArea = GameObject.Find("Outer Drop Area");
        DragArea = GameObject.Find("Drag Area");
        DropArea = GameObject.Find("Drop Area");
        RunButton = GameObject.Find("Run Button");
        StartProgramCodeBlock = DropArea.transform.FindChild("StartProgram").gameObject;
        _mainCanvasRectTransform = MainCanvas.GetComponent<RectTransform>();
    }
}
