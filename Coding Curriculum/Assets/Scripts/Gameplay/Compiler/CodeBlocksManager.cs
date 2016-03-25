using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CodeBlocksManager : MonoBehaviour
{
    public GameObject GoForwardBlockPrefab;
    public int GoForwardBlockLimit;
    public GameObject CanGoForwardBlockPrefab;
    public int CanGoForwardBlockLimit;
    public GameObject IfBlockPrefab;
    public int IfBlockLimit;
    public GameObject ElseBlockPrefab;
    public int ElseBlockLimit;
    public GameObject WhileBlockPrefab;
    public int WhileBlockLimit;
    public GameObject TurnLeftBlockPrefab;
    public int TurnLeftBlockLimit;
    public GameObject TurnRightBlockPrefab;
    public int TurnRightBlockLimit;
    public GameObject NotCanGoForwardBlockPrefab;
    public int NotCanGoForwardBlockLimit;
    public GameObject NoObstaclesAroundBlockPrefab;
    public int NoObstaclesAroundBlockLimit;
    public GameObject NotReachedTargetBlockPrefab;
    public int NotReachedTargetBlockLimit;

    internal struct TwoInts
    {
        internal int BlocksLimit;
        internal int BlocksPlaced;

        public TwoInts(int blocksLimit, int blocksPlaced)
        {
            BlocksLimit = blocksLimit;
            BlocksPlaced = blocksPlaced;
        }

        public TwoInts(TwoInts twoIntObject)
        {
            BlocksLimit = twoIntObject.BlocksLimit;
            BlocksPlaced = twoIntObject.BlocksPlaced;
        }
    }

	private static Dictionary<GameObject, TwoInts> _codeBlocksLimitManager;

    private SceneReferences _referencesScript;
    private static RectTransform _dragAreaRectTransform;

    void Awake()
    {
        var mainCamera = GameObject.Find("Main Camera");
        _referencesScript = mainCamera.GetComponent<SceneReferences>();
    }

    void Start ()
    {
		_codeBlocksLimitManager = new Dictionary<GameObject, TwoInts>();
        _codeBlocksLimitManager.Add(GoForwardBlockPrefab, new TwoInts(GoForwardBlockLimit, 0));
        _codeBlocksLimitManager.Add(CanGoForwardBlockPrefab, new TwoInts(CanGoForwardBlockLimit, 0));
        _codeBlocksLimitManager.Add(IfBlockPrefab, new TwoInts(IfBlockLimit, 0));
        _codeBlocksLimitManager.Add(ElseBlockPrefab, new TwoInts(ElseBlockLimit, 0));
        _codeBlocksLimitManager.Add(WhileBlockPrefab, new TwoInts(WhileBlockLimit, 0));
        _codeBlocksLimitManager.Add(TurnLeftBlockPrefab, new TwoInts(TurnLeftBlockLimit, 0));
        _codeBlocksLimitManager.Add(TurnRightBlockPrefab, new TwoInts(TurnRightBlockLimit, 0));
        _codeBlocksLimitManager.Add(NotCanGoForwardBlockPrefab, new TwoInts(NotCanGoForwardBlockLimit, 0));
        _codeBlocksLimitManager.Add(NoObstaclesAroundBlockPrefab, new TwoInts(NoObstaclesAroundBlockLimit, 0));
        _codeBlocksLimitManager.Add(NotReachedTargetBlockPrefab, new TwoInts(NotReachedTargetBlockLimit, 0));

        _dragAreaRectTransform = _referencesScript.DragArea.GetComponent<RectTransform>();

        foreach (var codeBlock in _codeBlocksLimitManager)
        {
            if (codeBlock.Key == null || codeBlock.Value.BlocksLimit <= 0) continue;
            var codeBlockTransform = Instantiate(codeBlock.Key.transform, new Vector3(), Quaternion.identity) as Transform;

            if (codeBlockTransform == null) continue;
            codeBlockTransform.SetParent(_dragAreaRectTransform, false);
            codeBlockTransform.localScale = new Vector3(1,1,1);
            codeBlockTransform.gameObject.GetComponent<CodeBlockData>().CurrentCodeBlockPrefab = codeBlock.Key;
            UpdateBlocksLeftField(codeBlockTransform.gameObject, codeBlock.Value.BlocksLimit, true);
        }
    }


    public static void CreateNewBlock(GameObject codeBlockToBeCopied)
    {
        var codeBlockPrefab = codeBlockToBeCopied.GetComponent<CodeBlockData>().CurrentCodeBlockPrefab;
        if(!_codeBlocksLimitManager.ContainsKey(codeBlockPrefab))
        {
            Debug.LogError("New CodeBlock could not be created");
            return;
        }

        var codeBlockLimiterData = _codeBlocksLimitManager[codeBlockPrefab];
        codeBlockLimiterData.BlocksPlaced++;
        _codeBlocksLimitManager[codeBlockPrefab] = new TwoInts(codeBlockLimiterData);

        UpdateBlocksLeftField(codeBlockToBeCopied, 0, false);

        if (codeBlockLimiterData.BlocksPlaced >= codeBlockLimiterData.BlocksLimit) return;

        var newTransform =
            Instantiate(codeBlockToBeCopied.transform, new Vector3(), Quaternion.identity) as Transform;

        if (newTransform == null)
        {
            Debug.LogError("New CodeBlock could not be created");
            return;
        }

        newTransform.SetParent(_dragAreaRectTransform, false);
        newTransform.localPosition = new Vector3(newTransform.localPosition.x, newTransform.localPosition.y, 0);
        newTransform.gameObject.transform.SetSiblingIndex(codeBlockToBeCopied.transform.GetSiblingIndex());

        UpdateBlocksLeftField(newTransform.gameObject,
            codeBlockLimiterData.BlocksLimit - codeBlockLimiterData.BlocksPlaced, true);
    }

    public static void RemoveBlock(GameObject codeBlockToBeRemoved)
    {
        var codeBlockPrefab = codeBlockToBeRemoved.GetComponent<CodeBlockData>().CurrentCodeBlockPrefab;
        if (!_codeBlocksLimitManager.ContainsKey(codeBlockPrefab))
        {
            Debug.LogError("CodeBlock removing error");
            return;
        }

        var codeBlockLimiterData = _codeBlocksLimitManager[codeBlockPrefab];

        if (codeBlockLimiterData.BlocksPlaced == codeBlockLimiterData.BlocksLimit)
        {
            codeBlockLimiterData.BlocksPlaced -= 2;
            _codeBlocksLimitManager[codeBlockPrefab] = new TwoInts(codeBlockLimiterData);
            CreateNewBlock(codeBlockToBeRemoved);
        }
        else
        {
            codeBlockLimiterData.BlocksPlaced--;
            _codeBlocksLimitManager[codeBlockPrefab] = new TwoInts(codeBlockLimiterData);

            var dragArea = GameObject.Find("Main Camera").GetComponent<SceneReferences>().DragArea;

            for (var i = 0; i < dragArea.transform.childCount; ++i)
            {
                var child = dragArea.transform.GetChild(i);
                var codeBlockData = child.GetComponent<CodeBlockData>();
                if (codeBlockData == null) continue;
                var prefab = codeBlockData.CurrentCodeBlockPrefab;
                if (prefab != codeBlockPrefab) continue;
                UpdateBlocksLeftField(child.gameObject,
                    codeBlockLimiterData.BlocksLimit - codeBlockLimiterData.BlocksPlaced, true);
                break;
            }
        }

        Destroy(codeBlockToBeRemoved);
    }

    static void UpdateBlocksLeftField(GameObject codeBlock, int blocksLeft, bool activate)
    {
        var blocksRemained = codeBlock.transform.FindChild("Blocks Left").gameObject;
        blocksRemained.SetActive(activate);

        if (!activate) return;

        var blocksRemainedTextField = blocksRemained.transform.FindChild("Blocks Left Text").gameObject.GetComponent<Text>();
        blocksRemainedTextField.text = blocksLeft.ToString();
    }
}
