using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof (CodeBlock))]

public class DragAndDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [NotNull] private SceneReferences _referencesScript;
    [NotNull] private CodeBlock _thisCodeBlockData;

    void Start()
    {
        _thisCodeBlockData = GetComponent<CodeBlock>();
        if(!_thisCodeBlockData)
            Debug.LogError("Error: CodeBlock data can't be accessed");

        var mainCamera = GameObject.Find("Main Camera");
        if (mainCamera)
            _referencesScript = mainCamera.GetComponent<SceneReferences>();
        else
            Debug.LogError("Error: Main Camera not found");
    }

#region DragEvents

    public void OnBeginDrag(PointerEventData eventData)     //Start moving the object
    {
        //We deactivate the collider of this CodeBlock as we don't want it to appear in CheckCollisionsWithCodeBlocks()
        GetComponent<BoxCollider2D>().enabled = false;

        var dragArea = _referencesScript.DragArea;
        if (transform.parent != dragArea.transform)
            return;

        var newTransform = Instantiate(transform, transform.localPosition, Quaternion.identity) as Transform;
        if (newTransform != null)
            newTransform.SetParent(dragArea.transform, false);
        else
            Debug.LogError("New CodeBlock could not be created");
    }

    public void OnDrag(PointerEventData eventData)         //Moving the object
    {
        //Move to the new position
        var newPosition = new Vector3(eventData.position.x, eventData.position.y, 0);
        newPosition = Camera.main.ScreenToWorldPoint(newPosition);
        newPosition.z = transform.position.z;
        transform.position = newPosition;

        //Now we have to check if our CodeBlock has collided with any other code block
        //If it has, than we temporarily and unilaterally connect it to the other code blocks
        CheckCollisionsWithCodeBlocks();
        //TODO: Show some visual feedback if there is an error with the colliders. e.g. the CodeBlock becomes
        //TODO: red if the colliders are not valid and green if it's ok. If no colliders are found, nothing happens.
    }

    public void OnEndDrag(PointerEventData eventData)   //Droping the object
    {
        //We assign the object to the OuterDropArea so that we can easier calculate his position
        transform.SetParent(_referencesScript.OuterDropArea.transform);

        //We check if the CodeBlock has been dropped in the OuterDropArea. If not, we destroy it
        var outerDropAreaRectTransform = _referencesScript.OuterDropArea.transform.GetComponent<RectTransform>();
        if (!outerDropAreaRectTransform.rect.Contains(transform.localPosition))
        {
            Destroy(gameObject);
            return;
        }

        //The object is in the right area. We can now assign it to the actual DropArea
        transform.SetParent(_referencesScript.DropArea.transform);

        //The next step is to run one final check for collisions
        //The difference this time is that we also get if there is any error with the collisions and if there is, than we remove the current CodeBlock
        var collidersError = CheckCollisionsWithCodeBlocks();
        if (collidersError != null)
        {
            Destroy(gameObject);
            Debug.Log("Positioning error: " + collidersError);     //TODO: Maybe show a message to the user???
            return;
        }

        //We (re)activate the collider of this CodeBlock if of type Instruction
        if(_thisCodeBlockData.Type == "Instruction")
            GetComponent<BoxCollider2D>().enabled = true;

        //We also connect the CodeBlock with the other code blocks around it and we update the position of all the code blocks
        AttachPermanently();
        _referencesScript.DropArea.GetComponent<UpdateBlocksPositions>().UpdatePositions(_referencesScript.StartProgramCodeBlock);
    }

#endregion

    private string CheckCollisionsWithCodeBlocks()
    {
        //The object has moved. Reset all parameters.
        _thisCodeBlockData.NextBlock = _thisCodeBlockData.PreviousBlock = _thisCodeBlockData.ParameterBlock = _thisCodeBlockData.HeadOfCompoundStatement = null;

        //Next we'll get a list of all the colliders that are in the rectangular area determined by the code blocks we are dragging right now
        var rectTransform = transform.GetComponent<RectTransform>();
        var pointA = new Vector2
        {
            x = transform.position.x - rectTransform.sizeDelta.x/2,
            y = transform.position.y - rectTransform.sizeDelta.y/2
        };

        var pointB = new Vector2
        {
            x = transform.position.x + rectTransform.sizeDelta.x / 2,
            y = transform.position.y + rectTransform.sizeDelta.y / 2
        };

        var touchedColliders = Physics2D.OverlapAreaAll(pointA, pointB);

        if (touchedColliders.Length == 0)
            return "Error: No colliders were touched.";

        //The next step is to validate the colliders we've hit. The way we are doing this is by using the <CodeBlock> component inside each code block
        //Also, we are taking into consideration only the two highest (greater y) code blocks
        Collider2D codeBlockColliderTop = null;
        Collider2D codeBlockColliderBottom = null;
        foreach (var currentCollider in
            touchedColliders.Where(currentCollider => currentCollider.gameObject.GetComponent<CodeBlock>()))
        {
            if (codeBlockColliderTop == null ||
                currentCollider.transform.position.y > codeBlockColliderTop.transform.position.y)
            {
                codeBlockColliderBottom = codeBlockColliderTop;
                codeBlockColliderTop = currentCollider;
            }
            else
            {
                if (codeBlockColliderBottom == null ||
                    currentCollider.transform.position.y > codeBlockColliderBottom.transform.position.y)
                    codeBlockColliderBottom = currentCollider;
            }
        }

        //In the case in which none of the found colliders are valid, we stop this function
        if (!codeBlockColliderTop)
            return "Error: No valid colliders were touched.";

        var codeBlockDataTop = codeBlockColliderTop.gameObject.GetComponent<CodeBlock>();

        /*
          We break the solving of the problem into two cases
          Case 1: two colliders overlapped
          Case 2: one collider overlapped
        */

        if (codeBlockColliderTop && codeBlockColliderBottom) //Two overlapped colliders
        {
            var codeBlockDataBottom = codeBlockColliderBottom.gameObject.GetComponent<CodeBlock>();

            /*
              We will now break this case into two sub-cases
              Case 1: The new block is an Instruction block
              Case 2: The new block is a Parameter block
            */

            if (_thisCodeBlockData.Type == "Instruction") // Instruction CodeBlock
            {
                //When we have two blocks with the same head of compound statement, we put the new block under the top one
                //If the two blocks have different heads, we attach it to the closest one
                if (codeBlockDataTop.NextBlock != codeBlockColliderBottom.gameObject &&
                    IsBottomCloser(codeBlockColliderTop, codeBlockColliderBottom))
                {
                    AttachTemporarilyToCodeBox(_thisCodeBlockData, codeBlockColliderBottom, codeBlockDataBottom,
                        false);
                }
                else
                    AttachTemporarilyToCodeBox(_thisCodeBlockData, codeBlockColliderTop, codeBlockDataTop, true);
            }
            else //Parameter CodeBlock
            {
                //If none of the two overlapped blocks don't support parameters, the user should get a visual feedback
                if (!codeBlockDataBottom.SupportsParameterBlock && !codeBlockDataTop.SupportsParameterBlock)
                    return "Illegal move. Blocks don't supports parameters";

                //If both blocks support parameters, then we just add it to the closer one
                if (codeBlockDataBottom.SupportsParameterBlock && codeBlockDataTop.SupportsParameterBlock)
                {
                    if (IsBottomCloser(codeBlockColliderTop, codeBlockColliderBottom))
                    {
                        AttachTemporarilyToCodeBox(_thisCodeBlockData, codeBlockColliderBottom,
                            codeBlockDataBottom, false);
                    }
                    else
                    {
                        AttachTemporarilyToCodeBox(_thisCodeBlockData, codeBlockColliderTop,
                            codeBlockDataTop, true);
                    }
                }
                //If just one block support parameter, we identity that block and we attach the new block to it
                else
                {
                    if (codeBlockDataBottom.SupportsParameterBlock)
                    {
                        AttachTemporarilyToCodeBox(_thisCodeBlockData, codeBlockColliderBottom,
                            codeBlockDataBottom, false);
                    }
                    else
                    {
                        AttachTemporarilyToCodeBox(_thisCodeBlockData, codeBlockColliderTop,
                            codeBlockDataTop, true);
                    }
                }
            }
        }
        else //One overlapped collider
        {
            if (_thisCodeBlockData.Type == "Parameter" && !codeBlockDataTop.SupportsParameterBlock)
                return "Error: Illegal move. Block doesn't supports parameters";

            //We can attach it above or below the code block
            AttachTemporarilyToCodeBox(_thisCodeBlockData, codeBlockColliderTop, codeBlockDataTop,
                codeBlockColliderTop.gameObject.transform.position.y > transform.position.y);
        }

        return null;
    }

    #region AttachFunction

    private void AttachTemporarilyToCodeBox(CodeBlock attachableCodeBlockData, Collider2D refCodeBlockCollider,
        CodeBlock refCodeBlockData, bool attachUnder)
    {
        /*
          We will break the problem into two cases
          Case 1: The new block is an Instruction block
          Case 2: The new block is a Parameter block
        */

        if (attachableCodeBlockData.Type == "Instruction") //Instruction CodeBlock
        {
            if (attachUnder || refCodeBlockData.Type == "Start")
            {
                /*
                    If the overlapped codeblock supports a compound statement and the new block is under the 
                    right half of the overlapped block, then we attach the new block inside the compound statement
                    as the first block of that compound statement.

                    Also, if the overlapped block is a Start block, we attach the new block as the first block under
                    the start block however it is positioned.
                */

                if ((refCodeBlockData.SupportsCompoundStatement &&
                     transform.position.x > refCodeBlockCollider.gameObject.transform.position.x) ||
                    refCodeBlockData.Type == "Start")
                {
                    attachableCodeBlockData.NextBlock = refCodeBlockData.FirstBlockInCompoundStatement;
                    attachableCodeBlockData.HeadOfCompoundStatement = refCodeBlockCollider.gameObject;
                }
                else
                {
                    attachableCodeBlockData.PreviousBlock = refCodeBlockCollider.gameObject;
                    attachableCodeBlockData.NextBlock = refCodeBlockData.NextBlock;
                    attachableCodeBlockData.HeadOfCompoundStatement = refCodeBlockData.HeadOfCompoundStatement;
                }
            }
            else //AttachUnder == false
            {
                //We simply attach the new block above the overlapped one
                attachableCodeBlockData.PreviousBlock = refCodeBlockData.PreviousBlock;
                attachableCodeBlockData.NextBlock = refCodeBlockCollider.gameObject;
                attachableCodeBlockData.HeadOfCompoundStatement = refCodeBlockData.HeadOfCompoundStatement;
            }
        }
        else // Parameter CodeBlock
        {
            attachableCodeBlockData.HeadOfCompoundStatement = refCodeBlockCollider.gameObject;
        }
    }

    private void AttachPermanently()
    {
        if (!_thisCodeBlockData.PreviousBlock &&
            !_thisCodeBlockData.HeadOfCompoundStatement)
            Debug.LogError("Error: No block where to attach the current CodeBlock");

        /*
            We break the problem into two cases
            1. The new block is an Instruction block
            2. The new block is a Parameter block
        */

        if (_thisCodeBlockData.Type == "Instruction") // Instruction CodeBlock
        {
            if (_thisCodeBlockData.PreviousBlock != null)
            {
                _thisCodeBlockData.PreviousBlock.GetComponent<CodeBlock>().NextBlock = gameObject;
            }
            else //_thisCodeBlockData.HeadOfCompoundStatement != null
            {
                var headBlockData = _thisCodeBlockData.HeadOfCompoundStatement.GetComponent<CodeBlock>();
                headBlockData.FirstBlockInCompoundStatement = gameObject;
            }

            if (_thisCodeBlockData.NextBlock != null)
            {
                _thisCodeBlockData.NextBlock.GetComponent<CodeBlock>().PreviousBlock = gameObject;
            }
        }
        else //Parameter CodeBlock
        {
            if (_thisCodeBlockData.HeadOfCompoundStatement != null)
            {
                _thisCodeBlockData.HeadOfCompoundStatement.GetComponent<CodeBlock>().ParameterBlock = gameObject;
            }
            else
            {
                Debug.LogError("Error: No block where to attach the current CodeBlock");
            }
        }
    }

    #endregion

    private bool IsBottomCloser(Collider2D codeBlockColliderTop, Collider2D codeBlockColliderBottom)
    {
        var distTop = Vector3.Distance(transform.position,
            codeBlockColliderTop.gameObject.transform.position);
        var distBottom = Vector3.Distance(transform.position,
            codeBlockColliderBottom.gameObject.transform.position);
        return distBottom < distTop;
    }
}
