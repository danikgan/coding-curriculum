using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof (CodeBlockData))]

public class DragAndDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private SceneReferences _referencesScript;
    private CodeBlockData _thisCodeBlockDataData;
    private UpdateBlocksPositions _updateBlocksPositionsScript;
    private ConsolePrinter _console;

    void Awake()
    {
        _thisCodeBlockDataData = GetComponent<CodeBlockData>();
        if(!_thisCodeBlockDataData)
            Debug.LogError("Error: CodeBlock data can't be accessed");

        var mainCamera = GameObject.Find("Main Camera");
        if (mainCamera)
            _referencesScript = mainCamera.GetComponent<SceneReferences>();
        else
            Debug.LogError("Error: Main Camera not found");
        _updateBlocksPositionsScript = _referencesScript.DropArea.GetComponent<UpdateBlocksPositions>();
        _console = _referencesScript.Console.GetComponent<ConsolePrinter>();
    }

#region DragEvents

    public void OnBeginDrag(PointerEventData eventData) //Start moving the object
    {
        //We deactivate the collider of this CodeBlock as we don't want it to appear in CheckCollisionsWithCodeBlocks()
        GetComponent<BoxCollider2D>().enabled = false;

        var dragArea = _referencesScript.DragArea;

        if (transform.parent == dragArea.transform)
        {
            CodeBlocksManager.CreateNewBlock(transform.gameObject);
        }
        else
        {
            ExtractCurrentBlock();
        }
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
        _updateBlocksPositionsScript.UpdatePositionsTemporarily(gameObject);
        CheckCollisionsWithCodeBlocks();
    }

    public void OnEndDrag(PointerEventData eventData)   //Droping the object
    {
        //We assign the object to the OuterDropArea so that we can easier calculate his position
        transform.SetParent(_referencesScript.OuterDropArea.transform);

        //We check if the CodeBlock has been dropped in the DropArea. If not, we destroy it
        var dropAreaRectTransform = _referencesScript.DropArea.transform.GetComponent<RectTransform>();
        if (!dropAreaRectTransform.rect.Contains(transform.localPosition))
        {
            RemoveCodeBlock();
            return;
        }

        //The object is in the right area. We can now assign it to the actual DropArea
        transform.SetParent(_referencesScript.DropArea.transform);

        //The next step is to run one final check for collisions
        //The difference this time is that we also get if there is any error with the collisions and if there is, than we remove the current CodeBlock
        var collidersError = CheckCollisionsWithCodeBlocks();
        if (collidersError != null)
        {
            RemoveCodeBlock();
            _console.PushWarning("Positioning error: " + collidersError + " Your code block was removed as a result.");
            return;
        }

        //We (re)activate the collider of this CodeBlock if of type Instruction
        if(_thisCodeBlockDataData.Type == "Instruction")
            GetComponent<BoxCollider2D>().enabled = true;

        //We also connect the CodeBlock with the other code blocks around it and we update the position of all the code blocks
        AttachPermanently();
        _updateBlocksPositionsScript.UpdateArea(_referencesScript.StartProgramCodeBlock);
    }

    private void ExtractCurrentBlock()
    {
        var currentCodeBlockData = GetComponent<CodeBlockData>();

        if (currentCodeBlockData.Type.Equals("Parameter"))
        {
            currentCodeBlockData.HeadOfCompoundStatement.GetComponent<CodeBlockData>().ParameterBlock = null;
        }

        if (currentCodeBlockData.HeadOfCompoundStatement)
        {
            var headData = currentCodeBlockData.HeadOfCompoundStatement.GetComponent<CodeBlockData>();
            if (headData.FirstBlockInCompoundStatement == gameObject)
                headData.FirstBlockInCompoundStatement = currentCodeBlockData.NextBlock;
        }

        if (currentCodeBlockData.PreviousBlock)
        {
            var previousBlockData = currentCodeBlockData.PreviousBlock.GetComponent<CodeBlockData>();
            previousBlockData.NextBlock = currentCodeBlockData.NextBlock;
        }

        if (currentCodeBlockData.NextBlock)
        {
            var nextBlockData = currentCodeBlockData.NextBlock.GetComponent<CodeBlockData>();
            nextBlockData.PreviousBlock = currentCodeBlockData.PreviousBlock;
        }

        currentCodeBlockData.HeadOfCompoundStatement =
            currentCodeBlockData.NextBlock = currentCodeBlockData.PreviousBlock = null;
    }

    private void RemoveCodeBlock()
    {
        var currentCodeBlock = gameObject;
        var currentCodeBlockData = currentCodeBlock.GetComponent<CodeBlockData>();

        currentCodeBlockData.HeadOfCompoundStatement =
            currentCodeBlockData.NextBlock = currentCodeBlockData.PreviousBlock = null;

        var gameObjectsToBeDestroyed = new Stack<GameObject>();

        while (currentCodeBlock)
        {
            while (currentCodeBlock)
            {
                gameObjectsToBeDestroyed.Push(currentCodeBlock);

                //We check if this block has attached a parameter block. If it does, then we move it as well (same Y)
                currentCodeBlockData = currentCodeBlock.GetComponent<CodeBlockData>();
                if (currentCodeBlockData.ParameterBlock)
                    gameObjectsToBeDestroyed.Push(currentCodeBlockData.ParameterBlock);

                //If the current block is the head of a compound statement, then we move to it
                if (currentCodeBlockData.FirstBlockInCompoundStatement)
                {
                    currentCodeBlock = currentCodeBlockData.FirstBlockInCompoundStatement;
                    //Break the inner While
                    break;
                }

                if (currentCodeBlockData.NextBlock)
                {
                    currentCodeBlock = currentCodeBlockData.NextBlock;
                    //Continue the inner While
                    continue;
                }

                /*  
                    There is no NextBlock so we've got at the end of the current compound statement.
                    We move to the next block after the head of the current statement as we've already solved the head block.
                    If there is no head block, that means that we are at the first block (probably a Start block) so we assign
                    null to currentCodeBlock so that both WHILE's will end.
                    If there is a head block but no NextBlock, it means that the head is itself at the end of a compound statement
                    so we move repeatedly to its head until we find a head block with a NextBlock or until we get to the first block
                */
                while (true)
                {
                    if (currentCodeBlockData.HeadOfCompoundStatement)
                    {
                        currentCodeBlock = currentCodeBlockData.HeadOfCompoundStatement;
                        currentCodeBlockData = currentCodeBlock.GetComponent<CodeBlockData>();
                        if (!currentCodeBlockData.NextBlock) continue;
                        currentCodeBlock = currentCodeBlockData.NextBlock;
                        break;
                    }
                    currentCodeBlock = null;
                    break;
                }
                //Break the inner While
                break;
            }
        }

        while (gameObjectsToBeDestroyed.Count > 0)
        {
            var codeBlock = gameObjectsToBeDestroyed.Pop();
            if (codeBlock != null)
            {
                CodeBlocksManager.RemoveBlock(codeBlock);
            }
        }

        _updateBlocksPositionsScript.UpdateArea(_referencesScript.StartProgramCodeBlock);
    }

    #endregion

    private string CheckCollisionsWithCodeBlocks()
    {
        //The object has moved. Reset all parameters.
        _thisCodeBlockDataData.NextBlock = _thisCodeBlockDataData.PreviousBlock = _thisCodeBlockDataData.HeadOfCompoundStatement = null;

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
            return "No other code blocks were touched. Code blocks have to be attached one to another.";

        //The next step is to validate the colliders we've hit. The way we are doing this is by using the <CodeBlock> component inside each code block
        //Also, we are taking into consideration only the two highest (greater y) code blocks
        Collider2D codeBlockColliderTop = null;
        Collider2D codeBlockColliderBottom = null;
        foreach (var currentCollider in
            touchedColliders.Where(currentCollider => currentCollider.gameObject.GetComponent<CodeBlockData>()))
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
            return "No other valid code blocks were touched. Code blocks have to be attached one to another with respect to the properties of each block.";

        var codeBlockDataTop = codeBlockColliderTop.gameObject.GetComponent<CodeBlockData>();

        /*
          We break the solving of the problem into two cases
          Case 1: two colliders overlapped
          Case 2: one collider overlapped
        */

        if (codeBlockColliderTop && codeBlockColliderBottom) //Two overlapped colliders
        {
            var codeBlockDataBottom = codeBlockColliderBottom.gameObject.GetComponent<CodeBlockData>();

            /*
              We will now break this case into two sub-cases
              Case 1: The new block is an Instruction block
              Case 2: The new block is a Parameter block
            */

            if (_thisCodeBlockDataData.Type == "Instruction") // Instruction CodeBlock
            {
                //When we have two blocks with the same head of compound statement, we put the new block under the top one
                //If the two blocks have different heads, we attach it to the closest one
                if (codeBlockDataTop.NextBlock != codeBlockColliderBottom.gameObject &&
                    IsBottomCloser(codeBlockColliderTop, codeBlockColliderBottom))
                {
                    AttachTemporarilyToCodeBox(_thisCodeBlockDataData, codeBlockColliderBottom, codeBlockDataBottom,
                        false);
                }
                else
                    AttachTemporarilyToCodeBox(_thisCodeBlockDataData, codeBlockColliderTop, codeBlockDataTop, true);
            }
            else //Parameter CodeBlock
            {
                //If none of the two overlapped blocks don't support parameters, the user should get a visual feedback
                if (!codeBlockDataBottom.SupportsParameterBlock && !codeBlockDataTop.SupportsParameterBlock)
                    return "You have tried to attach a parameter block to a block that doesn't support parameters.";

                //If both blocks support parameters, then we just add it to the closer one
                if (codeBlockDataBottom.SupportsParameterBlock && codeBlockDataTop.SupportsParameterBlock)
                {
                    if (IsBottomCloser(codeBlockColliderTop, codeBlockColliderBottom))
                    {
                        AttachTemporarilyToCodeBox(_thisCodeBlockDataData, codeBlockColliderBottom,
                            codeBlockDataBottom, false);
                    }
                    else
                    {
                        AttachTemporarilyToCodeBox(_thisCodeBlockDataData, codeBlockColliderTop,
                            codeBlockDataTop, true);
                    }
                }
                //If just one block support parameter, we identity that block and we attach the new block to it
                else
                {
                    if (codeBlockDataBottom.SupportsParameterBlock)
                    {
                        AttachTemporarilyToCodeBox(_thisCodeBlockDataData, codeBlockColliderBottom,
                            codeBlockDataBottom, false);
                    }
                    else
                    {
                        AttachTemporarilyToCodeBox(_thisCodeBlockDataData, codeBlockColliderTop,
                            codeBlockDataTop, true);
                    }
                }
            }
        }
        else //One overlapped collider
        {
            if (_thisCodeBlockDataData.Type == "Parameter" && !codeBlockDataTop.SupportsParameterBlock)
                return "You have tried to attach a parameter block to a block that doesn't support parameters.";

            //We can attach it above or below the code block
            AttachTemporarilyToCodeBox(_thisCodeBlockDataData, codeBlockColliderTop, codeBlockDataTop,
                codeBlockColliderTop.gameObject.transform.position.y > transform.position.y);
        }

        return null;
    }

    #region AttachFunction

    private void AttachTemporarilyToCodeBox(CodeBlockData attachableCodeBlockDataData, Collider2D refCodeBlockCollider,
        CodeBlockData refCodeBlockDataData, bool attachUnder)
    {
        /*
          We will break the problem into two cases
          Case 1: The new block is an Instruction block
          Case 2: The new block is a Parameter block
        */

        if (attachableCodeBlockDataData.Type == "Instruction") //Instruction CodeBlock
        {
            if (attachUnder || refCodeBlockDataData.Type == "Start")
            {
                /*
                    If the overlapped codeblock supports a compound statement and the new block is under the 
                    right half of the overlapped block, then we attach the new block inside the compound statement
                    as the first block of that compound statement.

                    Also, if the overlapped block is a Start block, we attach the new block as the first block under
                    the start block however it is positioned.
                */

                if ((refCodeBlockDataData.SupportsCompoundStatement &&
                     transform.position.x > refCodeBlockCollider.gameObject.transform.position.x) ||
                    refCodeBlockDataData.Type == "Start")
                {
                    attachableCodeBlockDataData.NextBlock = refCodeBlockDataData.FirstBlockInCompoundStatement;
                    attachableCodeBlockDataData.HeadOfCompoundStatement = refCodeBlockCollider.gameObject;
                }
                else
                {
                    attachableCodeBlockDataData.PreviousBlock = refCodeBlockCollider.gameObject;
                    attachableCodeBlockDataData.NextBlock = refCodeBlockDataData.NextBlock;
                    attachableCodeBlockDataData.HeadOfCompoundStatement = refCodeBlockDataData.HeadOfCompoundStatement;
                }
            }
            else //AttachUnder == false
            {
                //We simply attach the new block above the overlapped one
                attachableCodeBlockDataData.PreviousBlock = refCodeBlockDataData.PreviousBlock;
                attachableCodeBlockDataData.NextBlock = refCodeBlockCollider.gameObject;
                attachableCodeBlockDataData.HeadOfCompoundStatement = refCodeBlockDataData.HeadOfCompoundStatement;
            }
        }
        else // Parameter CodeBlock
        {
            attachableCodeBlockDataData.HeadOfCompoundStatement = refCodeBlockCollider.gameObject;
        }
    }

    private void AttachPermanently()
    {
        if (!_thisCodeBlockDataData.PreviousBlock &&
            !_thisCodeBlockDataData.HeadOfCompoundStatement)
            Debug.LogError("Error: No block where to attach the current CodeBlock");

        /*
            We break the problem into two cases
            1. The new block is an Instruction block
            2. The new block is a Parameter block
        */

        if (_thisCodeBlockDataData.Type == "Instruction") // Instruction CodeBlock
        {
            if (_thisCodeBlockDataData.PreviousBlock != null)
            {
                _thisCodeBlockDataData.PreviousBlock.GetComponent<CodeBlockData>().NextBlock = gameObject;
            }
            else //_thisCodeBlockData.HeadOfCompoundStatement != null
            {
                var headBlockData = _thisCodeBlockDataData.HeadOfCompoundStatement.GetComponent<CodeBlockData>();
                headBlockData.FirstBlockInCompoundStatement = gameObject;
            }

            if (_thisCodeBlockDataData.NextBlock != null)
            {
                _thisCodeBlockDataData.NextBlock.GetComponent<CodeBlockData>().PreviousBlock = gameObject;
            }
        }
        else //Parameter CodeBlock
        {
            if (_thisCodeBlockDataData.HeadOfCompoundStatement != null)
            {
                var headBlockData = _thisCodeBlockDataData.HeadOfCompoundStatement.GetComponent<CodeBlockData>();
                Destroy(headBlockData.ParameterBlock);
                headBlockData.ParameterBlock = gameObject;
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
