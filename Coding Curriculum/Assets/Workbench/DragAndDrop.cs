using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof (CodeBlock))]

public class DragAndDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [NotNull] private SceneReferences _referencesScript;

    private GameObject DragArea;
    private GameObject DropArea;

    float _deltaX;
    float _deltaY;

    private Vector3 UpdateCodeBlocks_CurrentHeadPosition;

    void Start()
    {
        var mainCamera = GameObject.Find("Main Camera");        //TODO Do the same thing in other classes
        if (mainCamera)
        {
            _referencesScript = mainCamera.GetComponent<SceneReferences>();
            if (_referencesScript)
            {
                DragArea = _referencesScript.DragArea;
                DropArea = _referencesScript.DropArea;
            }
            else
                Debug.LogError("ReferencesScript Component not found");
        }
        else
            Debug.LogError("Main Camera not found");
    }

    public void OnMouseDown()
    {
        Debug.Log("bullshit");
    }

    public void OnBeginDrag(PointerEventData eventData) //Start moving the object
    {
        //dist = Camera.main.WorldToScreenPoint(transform.position);
        _deltaX = Input.mousePosition.x - eventData.position.x;
        _deltaY = Input.mousePosition.y - eventData.position.y;

        if (transform.parent != DragArea.transform) return;
        var thisTransform = Instantiate(transform, transform.position, Quaternion.identity) as Transform;
        if (thisTransform != null)
            thisTransform.SetParent(DragArea.transform, false);
    }

    public void OnDrag(PointerEventData eventData) //Moving the object
    {
        //Vector3 curPos = new Vector3(Input.mousePosition.x - _deltaX, Input.mousePosition.y - _deltaY, dist.z);
        //Vector3 worldPos = Camera.main.ScreenToWorldPoint(curPos);

        var codeBlockData = GetComponent<CodeBlock>();
        codeBlockData.NextBlock =
            codeBlockData.PreviousBlock = codeBlockData.ParameterBlock = codeBlockData.HeadOfCompoundStatement = null;

        var newPosition = Camera.main.ScreenToWorldPoint(new Vector3(eventData.position.x - _deltaX, eventData.position.y - _deltaY,
            transform.position.z));
        transform.position = new Vector3(eventData.position.x,eventData.position.y,transform.position.z);

        var topCorner = new Vector2(newPosition.x - transform.GetComponent<RectTransform>().rect.width/2,
            newPosition.y - transform.GetComponent<RectTransform>().rect.height/2);
        var bottomCorner = new Vector2(newPosition.x + transform.GetComponent<RectTransform>().rect.width / 2,
            newPosition.y + transform.GetComponent<RectTransform>().rect.height / 2);
        var touchedCollider2Ds = Physics2D.OverlapAreaAll(topCorner, bottomCorner);

        if (touchedCollider2Ds.Length == 0)
            return;

        Collider2D codeBlockColliderTop = null;
        Collider2D codeBlockColliderBottom = null;
        foreach (var collider in touchedCollider2Ds)
        {
            var gameObjectCodeBlockData = collider.gameObject.GetComponent<CodeBlock>();
            if (gameObjectCodeBlockData == null || gameObjectCodeBlockData.HeadOfCompoundStatement == null)
                continue;

            if (codeBlockColliderTop == null ||
                collider.transform.position.y > codeBlockColliderTop.transform.position.y)
            {
                codeBlockColliderBottom = codeBlockColliderTop;
                codeBlockColliderTop = collider;
            }
            else
            {
                if (collider.transform.position.y > codeBlockColliderBottom.transform.position.y)
                {
                    codeBlockColliderBottom = collider;
                }
            }
        }

        if (codeBlockColliderTop == null && codeBlockColliderBottom == null)
            return;

        var codeBlockDataTop = codeBlockColliderTop.gameObject.GetComponent<CodeBlock>();
        var codeBlockDataBottom = codeBlockColliderBottom.gameObject.GetComponent<CodeBlock>();

        if (codeBlockColliderTop && codeBlockColliderBottom)
        {
            //TODO Explain what dafaq you've done here
            if (codeBlockData.Type == "Instruction") // Instruction CodeBlock
            {
                //When we have two blocks with the same head of compound statement. we put the new block under the top one
                //If the two blocks have different heads, we attach it to the closest one
                if (codeBlockDataTop.NextBlock != codeBlockColliderBottom.gameObject &&
                    IsBottomCloser(codeBlockColliderTop, codeBlockColliderBottom))
                    AttachTemporarilyToCodeBox(codeBlockData, codeBlockColliderBottom, codeBlockDataBottom, false);
                else
                    AttachTemporarilyToCodeBox(codeBlockData, codeBlockColliderTop, codeBlockDataTop, true);
            }
            else //Parameter CodeBlock
            {
                //TODO Explain what dafaq you've done here
                if (codeBlockDataBottom.SupportsParameterBlock || codeBlockDataTop.SupportsParameterBlock)
                    //TODO PrintError("Illegal move. Blocks don't supports parameters");
                    Debug.Log("Illegal move. Blocks don't supports parameters");
                else if (codeBlockDataBottom.SupportsParameterBlock && codeBlockDataTop.SupportsParameterBlock)
                    if (IsBottomCloser(codeBlockColliderTop, codeBlockColliderBottom))
                        AttachTemporarilyToCodeBox(codeBlockData, codeBlockColliderBottom, codeBlockDataBottom, false);
                    else
                        AttachTemporarilyToCodeBox(codeBlockData, codeBlockColliderTop, codeBlockDataTop, true);
                else if (codeBlockDataBottom.SupportsParameterBlock && !codeBlockDataTop.SupportsParameterBlock)
                    AttachTemporarilyToCodeBox(codeBlockData, codeBlockColliderBottom, codeBlockDataBottom, false);
                else
                    AttachTemporarilyToCodeBox(codeBlockData, codeBlockColliderTop, codeBlockDataTop, true);
            }
        }
        else
        {
            if (codeBlockColliderTop.gameObject.transform.position.y <= transform.position.y)
                AttachTemporarilyToCodeBox(codeBlockData, codeBlockColliderTop, codeBlockDataTop, true);
            else
                AttachTemporarilyToCodeBox(codeBlockData, codeBlockColliderTop, codeBlockDataTop, false);
        }
    }

    private void AttachTemporarilyToCodeBox(CodeBlock attachableCodeBlockData, Collider2D refCodeBlockCollider,
        CodeBlock refCodeBlockData, bool AttachUnder)
    {
        if (attachableCodeBlockData.Type == "Instruction") //Instruction CodeBlock
        {
            if (attachableCodeBlockData.SupportsCompoundStatement &&
                transform.position.x > refCodeBlockCollider.gameObject.transform.position.x)
            {
                attachableCodeBlockData.NextBlock = refCodeBlockData.FirstBlockInCompoundStatement;
                attachableCodeBlockData.HeadOfCompoundStatement = refCodeBlockCollider.gameObject;
            }
            else
            {
                if (AttachUnder || refCodeBlockData.Type == "Start")
                {
                    attachableCodeBlockData.PreviousBlock = refCodeBlockCollider.gameObject;
                    attachableCodeBlockData.NextBlock = refCodeBlockData.NextBlock;
                    attachableCodeBlockData.HeadOfCompoundStatement = refCodeBlockData.HeadOfCompoundStatement;
                }
                else
                {
                    attachableCodeBlockData.PreviousBlock = refCodeBlockData.PreviousBlock;
                    attachableCodeBlockData.NextBlock = refCodeBlockCollider.gameObject;
                    attachableCodeBlockData.HeadOfCompoundStatement = refCodeBlockData.HeadOfCompoundStatement;
                }
            }
        }
        else // Parameter CodeBlock
        {
            if (refCodeBlockData.SupportsParameterBlock)
            {
                attachableCodeBlockData.HeadOfCompoundStatement = refCodeBlockCollider.gameObject;
            }
            else
            {
                //TODO PrintError("Illegal move. Blocks don't supports parameters");
                Debug.Log("Illegal move. Blocks don't supports parameters");
            }
        }
    }

    private void AttachPermanently()
    {
        var codeBlockData = GetComponent<CodeBlock>();

        if (!(codeBlockData.NextBlock || codeBlockData.PreviousBlock || codeBlockData.HeadOfCompoundStatement))
            return;

        if (codeBlockData.Type == "Instruction")
        {
            if (codeBlockData.PreviousBlock)
            {
                codeBlockData.PreviousBlock.GetComponent<CodeBlock>().NextBlock = gameObject;
            }
            else
            {
                if (codeBlockData.HeadOfCompoundStatement)
                {
                    var headBlockData = codeBlockData.HeadOfCompoundStatement.GetComponent<CodeBlock>();
                    headBlockData.FirstBlockInCompoundStatement = gameObject;
                }
            }

            if (codeBlockData.NextBlock)
            {
                codeBlockData.NextBlock.GetComponent<CodeBlock>().PreviousBlock = gameObject;
            }
        }
        else
        {
            codeBlockData.HeadOfCompoundStatement.GetComponent<CodeBlock>().ParameterBlock = gameObject;
        }
    }

    private bool IsBottomCloser(Collider2D codeBlockColliderTop, Collider2D codeBlockColliderBottom)
    {
        var distTop = Vector3.Distance(transform.position,
            codeBlockColliderTop.gameObject.transform.position);
        var distBottom = Vector3.Distance(transform.position,
            codeBlockColliderBottom.gameObject.transform.position);
        return distBottom < distTop;
    }



    public void OnEndDrag(PointerEventData eventData) //Droping the object
    {
        var adjustedPositionTo2D = new Vector3(transform.position.x, transform.position.y, 0);

        if (!DropArea.transform.GetComponent<RectTransform>().rect.Contains(adjustedPositionTo2D))
        {
            Destroy(gameObject);
            return;
        }

        transform.parent = DropArea.transform;
        AttachPermanently();
        UpdateCodeBlocks_CurrentHeadPosition = _referencesScript.StartProgramCodeBlock.transform.position;
        UpdateCodeBlocksPositions(
            _referencesScript.StartProgramCodeBlock.GetComponent<CodeBlock>().FirstBlockInCompoundStatement);
    }

    const int speed = 5;
    const int deltaX = 70;
    const int deltaY = 200;

    private void UpdateCodeBlocksPositions(GameObject currentCodeBlock)
    {
        UpdateCodeBlocks_CurrentHeadPosition.x += deltaX;
        for (var headBlockData = currentCodeBlock.GetComponent<CodeBlock>();
            headBlockData;
            currentCodeBlock = headBlockData.NextBlock,
                headBlockData = currentCodeBlock ? currentCodeBlock.GetComponent<CodeBlock>() : null)
        {
            UpdateCodeBlocks_CurrentHeadPosition.y -= deltaY;
            var newPosition = currentCodeBlock.transform.position;
            newPosition.y = UpdateCodeBlocks_CurrentHeadPosition.y;
            currentCodeBlock.transform.position = Vector3.MoveTowards(currentCodeBlock.transform.position, newPosition,
                speed*Time.deltaTime);

            if (headBlockData.ParameterBlock)
            {
                newPosition = headBlockData.ParameterBlock.transform.position;
                newPosition.y = UpdateCodeBlocks_CurrentHeadPosition.y;
                headBlockData.ParameterBlock.transform.position =
                    Vector3.MoveTowards(headBlockData.ParameterBlock.transform.position, newPosition,
                        speed*Time.deltaTime);
            }

            if (headBlockData.FirstBlockInCompoundStatement)
                UpdateCodeBlocksPositions(headBlockData.FirstBlockInCompoundStatement);

        }

        UpdateCodeBlocks_CurrentHeadPosition.x -= deltaX;
    }
}
