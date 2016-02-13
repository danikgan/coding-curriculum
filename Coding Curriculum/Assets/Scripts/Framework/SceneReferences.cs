using JetBrains.Annotations;
using UnityEngine;

public class SceneReferences : MonoBehaviour
{
    [NotNull] public GameObject DragArea;
    [NotNull] public GameObject DropArea;
    [NotNull] public GameObject OuterDragArea;
    [NotNull] public GameObject OuterDropArea;
    [NotNull] public GameObject StartProgramCodeBlock;
    [NotNull] public GameObject Player;
    [NotNull] public GameObject MainCanvas;
    [NotNull] public GameObject RunButton;
    [NotNull] public Vector3 MainCanvasScale;

    void Start()
    {
        Player = GameObject.Find("Player");
        MainCanvas = GameObject.Find("Main Canvas");
        MainCanvasScale = MainCanvas.GetComponent<RectTransform>().localScale;
        OuterDragArea = GameObject.Find("Outer Drag Area");
        OuterDropArea = GameObject.Find("Outer Drop Area");
        DragArea = GameObject.Find("Drag Area");
        DropArea = GameObject.Find("Drop Area");
        RunButton = GameObject.Find("Run Button");
        StartProgramCodeBlock = DropArea.transform.FindChild("StartProgram").gameObject;
    }

}
