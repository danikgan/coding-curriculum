using System.Collections.Generic;
using UnityEngine;

public class CodeBlocksManager : MonoBehaviour
{
    public GameObject GoForwardBlockPrefab;
    public int GoForwardBlockLimit;
    public GameObject CanGoForwardBlockPrefab;
    public int CanGoForwardBlockLimit;
    public GameObject IfBlockPrefab;
    public int IfBlockLimit;
    public GameObject WhileBlockPrefab;
    public int WhileBlockLimit;
    public GameObject TurnLeftBlockPrefab;
    public int TurnLeftBlockLimit;
    public GameObject TurnRightBlockPrefab;
    public int TurnRightBlockLimit;

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

    private static Dictionary<GameObject, TwoInts> _codeBlocksLimitManager = new Dictionary<GameObject, TwoInts>();

    private SceneReferences _referencesScript;
    private static RectTransform _dragAreaRectTransform;

    void Awake()
    {
        var mainCamera = GameObject.Find("Main Camera");
        _referencesScript = mainCamera.GetComponent<SceneReferences>();
    }

    void Start ()
    {
        _codeBlocksLimitManager.Add(GoForwardBlockPrefab, new TwoInts(GoForwardBlockLimit, 0));
        _codeBlocksLimitManager.Add(CanGoForwardBlockPrefab, new TwoInts(CanGoForwardBlockLimit, 0));
        _codeBlocksLimitManager.Add(IfBlockPrefab, new TwoInts(IfBlockLimit, 0));
        _codeBlocksLimitManager.Add(WhileBlockPrefab, new TwoInts(WhileBlockLimit, 0));
        _codeBlocksLimitManager.Add(TurnLeftBlockPrefab, new TwoInts(TurnLeftBlockLimit, 0));
        _codeBlocksLimitManager.Add(TurnRightBlockPrefab, new TwoInts(TurnRightBlockLimit, 0));

        _dragAreaRectTransform = _referencesScript.DragArea.GetComponent<RectTransform>();

        foreach (var codeBlock in _codeBlocksLimitManager)
        {
            if (codeBlock.Key == null || codeBlock.Value.BlocksLimit <= 0) continue;
            var codeBlockTransform = Instantiate(codeBlock.Key.transform, new Vector3(), Quaternion.identity) as Transform;

            if (codeBlockTransform == null) continue;
            codeBlockTransform.SetParent(_dragAreaRectTransform, false);
            codeBlockTransform.localScale = new Vector3(1,1,1);
            codeBlockTransform.gameObject.GetComponent<CodeBlockData>().CurrentCodeBlockPrefab = codeBlock.Key;
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
        }

        Destroy(codeBlockToBeRemoved);
    }
}
