using UnityEngine;
using System.Collections.Generic;
using JetBrains.Annotations;

public class UpdateBlocksPositions : MonoBehaviour
{
    private const int Speed = 100;
    private float DeltaX;
    private float DeltaY;

    private SceneReferences _referencesScript;

    private struct UpdateObject
    {
        internal GameObject CodeBlock;
        internal Vector3 FinalPosition;

        internal UpdateObject(GameObject codeBlock, Vector3 finalPosition)
        {
            CodeBlock = codeBlock;
            FinalPosition = finalPosition;
        }
    }

    [NotNull] readonly Queue<UpdateObject> _updateQueue = new Queue<UpdateObject>();

    void Start()
    {
        var mainCamera = GameObject.Find("Main Camera");
        _referencesScript = mainCamera.GetComponent<SceneReferences>();
        var _startProgramCodeBlock = _referencesScript.StartProgramCodeBlock;
        var codeBlockSize = _startProgramCodeBlock.GetComponent<RectTransform>().sizeDelta;

        DeltaX = (codeBlockSize.x/4 + 3) *_referencesScript.MainCanvasScale.x;
        DeltaY = (codeBlockSize.y + 7) * _referencesScript.MainCanvasScale.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (_updateQueue.Count == 0)
            return;

        var currentObject = _updateQueue.Peek();
        currentObject.CodeBlock.transform.position = Vector3.MoveTowards(currentObject.CodeBlock.transform.position,
            currentObject.FinalPosition, Speed*Time.deltaTime);

        if (currentObject.CodeBlock.transform.position == currentObject.FinalPosition)
            _updateQueue.Dequeue();
    }

    public void UpdatePositions(GameObject currentCodeBlock)
    {
        var nextPosition = currentCodeBlock.transform.position;

        /*Next, we are using a iterative DFS type algorithm to reposition code blocks*/

        while (currentCodeBlock)
        {
            //We move into updating the position of all the following code blocks
            while (currentCodeBlock)
            {
                //We move the current block to the requires position
                _updateQueue.Enqueue(new UpdateObject(currentCodeBlock, nextPosition));

                //We check if this block has attached a parameter block. If it does, then we move it as well (same Y)
                var currentCodeBlockData = currentCodeBlock.GetComponent<CodeBlock>();
                if (currentCodeBlockData.ParameterBlock)
                {
                    var parameter = currentCodeBlockData.ParameterBlock;
                    var parameterPosition = nextPosition;
                    parameterPosition.x += currentCodeBlock.GetComponent<RectTransform>().sizeDelta.x * _referencesScript.MainCanvasScale.x;
                    _updateQueue.Enqueue(new UpdateObject(parameter, parameterPosition));
                }

                //We update the Y for the next block we'll put (move down)
                nextPosition.y -= DeltaY;

                //If the current block is the head of a compound statement, then we move to it
                if (currentCodeBlockData.FirstBlockInCompoundStatement)
                {
                    currentCodeBlock = currentCodeBlockData.FirstBlockInCompoundStatement;
                    //We update the X for the next block that we will reposition (move to the right / indentation)
                    nextPosition.x += DeltaX;
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
                    We move to the next block after the head of the current statement as we've already positioned the head block.
                    If there is no head, that means that we are at the first block (probably a Start block) so we assign null to 
                    currentCodeBlock so that both WHILE's will end.
                */
                currentCodeBlock = currentCodeBlockData.HeadOfCompoundStatement
                    ? currentCodeBlockData.HeadOfCompoundStatement.GetComponent<CodeBlock>().NextBlock
                    : null;
                //Move back to the left / remove extra-indentation
                nextPosition.x -= DeltaX;
                //Break the inner While
                break;
            }
        }
    }
}
