using System;
using UnityEngine;
using System.Collections.Generic;
using JetBrains.Annotations;

public class UpdateBlocksPositions : MonoBehaviour
{
    private const int Speed = 90;
    private float Identation;
    private float SpaceBetweenBlocks;
 
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

    [NotNull] readonly List<UpdateObject> _updateList = new List<UpdateObject>();

    void Start()
    {
        var mainCamera = GameObject.Find("Main Camera");
        _referencesScript = mainCamera.GetComponent<SceneReferences>();
        var _startProgramCodeBlock = _referencesScript.StartProgramCodeBlock;
        var codeBlockSize = _startProgramCodeBlock.GetComponent<RectTransform>().sizeDelta;

        Identation = (codeBlockSize.x / 7) /**_referencesScript.MainCanvasScale.x*/;
       // SpaceBetweenBlocks = (codeBlockSize.y - 5) * _referencesScript.MainCanvasScale.y;
        SpaceBetweenBlocks = codeBlockSize.y + 10;
    }

    // Update is called once per frame
    void Update()
    {
        for (var index = 0; index < _updateList.Count; index++)
        {
            var currentObject = _updateList[index];
            currentObject.CodeBlock.transform.localPosition = Vector3.MoveTowards(currentObject.CodeBlock.transform.localPosition, currentObject.FinalPosition, Speed*Time.deltaTime);

            if (currentObject.CodeBlock.transform.localPosition == currentObject.FinalPosition)
            {
                _updateList.RemoveAt(index);
                index--;
            }
        }
    }

    public void UpdatePositions(GameObject currentCodeBlock)
    {
        var nextPosition = currentCodeBlock.transform.localPosition;
        var minX = nextPosition.x;
        var minY = nextPosition.y;
        var maxX = nextPosition.x;
        var maxY = nextPosition.y;
        var outerDropAreaRectTransform = _referencesScript.OuterDropArea.GetComponent<RectTransform>();
        var dropAreaRectTransform = _referencesScript.DropArea.GetComponent<RectTransform>();
        var blockSize = currentCodeBlock.GetComponent<RectTransform>().sizeDelta;

        /*Next, we are using a iterative DFS type algorithm to reposition code blocks*/

        while (currentCodeBlock)
        {
            //We move into updating the position of all the following code blocks
            while (currentCodeBlock)
            {
                //We move the current block to the requires position
                _updateList.Add(new UpdateObject(currentCodeBlock, nextPosition));

                //We check if the DropArea is big enough
                var height = maxY - minY;
                var width = maxX - minX;
                if (height < outerDropAreaRectTransform.sizeDelta.y)
                    height = outerDropAreaRectTransform.sizeDelta.y;
                if (width < outerDropAreaRectTransform.sizeDelta.x)
                    width = outerDropAreaRectTransform.sizeDelta.x;
                dropAreaRectTransform.sizeDelta = new Vector2(width, height);

                //We check if this block has attached a parameter block. If it does, then we move it as well (same Y)
                var currentCodeBlockData = currentCodeBlock.GetComponent<CodeBlock>();
                if (currentCodeBlockData.ParameterBlock)
                {
                    var parameter = currentCodeBlockData.ParameterBlock;
                    var parameterPosition = nextPosition;
                    parameterPosition.x += blockSize.x + Identation; /** _referencesScript.MainCanvasScale.x*/
                    _updateList.Add(new UpdateObject(parameter, parameterPosition));
                    maxX = Math.Max(maxX, parameterPosition.x + blockSize.x);
                }

                //We update the Y for the next block we'll put (move down)
                nextPosition.y -= SpaceBetweenBlocks;
                minY = Math.Min(minY, nextPosition.y - blockSize.y - SpaceBetweenBlocks);

                //If the current block is the head of a compound statement, then we move to it
                if (currentCodeBlockData.FirstBlockInCompoundStatement)
                {
                    currentCodeBlock = currentCodeBlockData.FirstBlockInCompoundStatement;
                    //We update the X for the next block that we will reposition (move to the right / indentation)
                    nextPosition.x += Identation;
                    maxX = Math.Max(maxX, nextPosition.x + blockSize.x + Identation);
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
                nextPosition.x -= Identation;
                //Break the inner While
                break;
            }
        }
    }
}
