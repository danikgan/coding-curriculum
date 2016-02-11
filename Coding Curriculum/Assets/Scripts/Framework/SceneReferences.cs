using JetBrains.Annotations;
using UnityEngine;

public class SceneReferences : MonoBehaviour
{
    [NotNull] public GameObject DragArea;
    [NotNull] public GameObject DropArea;
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
        DragArea = MainCanvas.transform.FindChild("DragArea").gameObject;
        DropArea = MainCanvas.transform.FindChild("DropArea").gameObject;
        RunButton = MainCanvas.transform.FindChild("Run Button").gameObject;
        StartProgramCodeBlock = DropArea.transform.FindChild("StartProgram").gameObject;
    }

}
