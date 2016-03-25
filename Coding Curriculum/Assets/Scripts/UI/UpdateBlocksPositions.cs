using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class UpdateBlocksPositions : MonoBehaviour
{
    private const int Speed = 120;
    private float Indentation;
    private float SpaceBetweenBlocks;
    private Vector2 BlocksSize;

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

    void Awake()
    {
        var mainCamera = GameObject.Find("Main Camera");
        _referencesScript = mainCamera.GetComponent<SceneReferences>();
        var _startProgramCodeBlock = _referencesScript.StartProgramCodeBlock;
        BlocksSize = _startProgramCodeBlock.GetComponent<RectTransform>().sizeDelta;
    }

    void Start()
    {
        Indentation = BlocksSize.x / 4 * _referencesScript.MapScale.x;
        SpaceBetweenBlocks = BlocksSize.y + 20 * _referencesScript.MapScale.x;

        UpdateArea(_referencesScript.StartProgramCodeBlock);
    }

    // Update is called once per frame
    void Update()
    {
        for (var index = 0; index < _updateList.Count; index++)
        {
            var currentObject = _updateList[index];
            if (currentObject.CodeBlock.transform.localPosition == currentObject.FinalPosition)
            {
                _updateList.RemoveAt(index);
                index--;
            }
            else
                currentObject.CodeBlock.transform.localPosition =
                    Vector3.MoveTowards(currentObject.CodeBlock.transform.localPosition, currentObject.FinalPosition,
                        Speed*Time.deltaTime);
        }
    }

    public void UpdateArea(GameObject currentCodeBlock)
    {
        var initialBlock = currentCodeBlock;
        var nextPosition = AdjustAreaSizes(currentCodeBlock);
        UpdatePositions(initialBlock, nextPosition);
    }

    public void UpdatePositions(GameObject currentCodeBlock, Vector3 nextPosition)
    {
        /*Next, we are using a iterative DFS type algorithm to reposition code blocks*/

        while (currentCodeBlock)
        {
            //We move into updating the position of all the following code blocks
            while (currentCodeBlock)
            {
                //We move the current block to the requires position
                _updateList.Add(new UpdateObject(currentCodeBlock, nextPosition));

                //We check if this block has attached a parameter block. If it does, then we move it as well (same Y)
                var currentCodeBlockData = currentCodeBlock.GetComponent<CodeBlockData>();
                if (currentCodeBlockData.ParameterBlock)
                {
                    var parameter = currentCodeBlockData.ParameterBlock;
                    var parameterPosition = nextPosition;
                    parameterPosition.x += BlocksSize.x + Indentation; /** _referencesScript.MainCanvasScale.x*/
                    _updateList.Add(new UpdateObject(parameter, parameterPosition));
                }

                //We update the Y for the next block we'll put (move down)
                nextPosition.y -= SpaceBetweenBlocks;

                //If the current block is the head of a compound statement, then we move to it
                if (currentCodeBlockData.FirstBlockInCompoundStatement)
                {
                    currentCodeBlock = currentCodeBlockData.FirstBlockInCompoundStatement;
                    //We update the X for the next block that we will reposition (move to the right / indentation)
                    nextPosition.x += Indentation;
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

                        //Move back to the left / remove extra-indentation
                        nextPosition.x -= Indentation;

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
    }

    private Vector3 AdjustAreaSizes(GameObject currentCodeBlock)
    {
        float numberOfBlocksHorizontally = 1;
        float numberOfBlocksVertically = 0;
        float temp_numberOfBlocksHorizontally = 1;

        while (currentCodeBlock)
        {
            //We move into all the following code blocks
            while (currentCodeBlock)
            {
                //We check if this block has attached a parameter block. If it does, then we move it as well (same Y)
                var currentCodeBlockData = currentCodeBlock.GetComponent<CodeBlockData>();
                if (currentCodeBlockData.ParameterBlock)
                    numberOfBlocksHorizontally = Math.Max(temp_numberOfBlocksHorizontally + 1,
                        numberOfBlocksHorizontally);

                //We update the Y for the next block we'll put (move down)
                numberOfBlocksVertically++;

                //If the current block is the head of a compound statement, then we move to it
                if (currentCodeBlockData.FirstBlockInCompoundStatement)
                {
                    currentCodeBlock = currentCodeBlockData.FirstBlockInCompoundStatement;
                    temp_numberOfBlocksHorizontally += 0.5f;
                    numberOfBlocksHorizontally = Math.Max(temp_numberOfBlocksHorizontally, numberOfBlocksHorizontally);
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

                        //Move back to the left / remove extra-indentation
                        temp_numberOfBlocksHorizontally -= 0.5f;

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

        var neededWidth = numberOfBlocksHorizontally * BlocksSize.x +
                          (numberOfBlocksHorizontally + 1) * Indentation;
        var neededHeight = numberOfBlocksVertically * BlocksSize.y +
                           (numberOfBlocksVertically - 1) * SpaceBetweenBlocks + 2 * Indentation;

        var outerDropAreaRectTransform = _referencesScript.OuterDropArea.GetComponent<RectTransform>();
        var dropAreaRectTransform = _referencesScript.DropArea.GetComponent<RectTransform>();

        var width = Math.Max(neededWidth, outerDropAreaRectTransform.sizeDelta.x);
        var height = Math.Max(neededHeight, outerDropAreaRectTransform.sizeDelta.y);
        dropAreaRectTransform.sizeDelta = new Vector2(width, height);
        dropAreaRectTransform.position = dropAreaRectTransform.parent.position;

        return new Vector3(dropAreaRectTransform.rect.xMin + Indentation + BlocksSize.x / 2,
            dropAreaRectTransform.rect.yMax - Indentation - BlocksSize.y / 2, 0);
    }

    public void UpdatePositionsTemporarily(GameObject currentCodeBlock)
    {
        var currentCodeBlockData = currentCodeBlock.GetComponent<CodeBlockData>();

        currentCodeBlockData.HeadOfCompoundStatement =
            currentCodeBlockData.NextBlock = currentCodeBlockData.PreviousBlock = null;

        var nextPosition = currentCodeBlock.transform.localPosition;

        /*Next, we are using a iterative DFS type algorithm to reposition code blocks*/

        while (currentCodeBlock)
        {
            //We move into updating the position of all the following code blocks
            while (currentCodeBlock)
            {
                //We move the current block to the requires position

                currentCodeBlock.transform.localPosition = nextPosition;

                //We check if this block has attached a parameter block. If it does, then we move it as well (same Y)
                currentCodeBlockData = currentCodeBlock.GetComponent<CodeBlockData>();
                if (currentCodeBlockData.ParameterBlock)
                {
                    var parameter = currentCodeBlockData.ParameterBlock;
                    var parameterPosition = nextPosition;
                    parameterPosition.x += BlocksSize.x + Indentation; /** _referencesScript.MainCanvasScale.x*/
                    parameter.transform.localPosition = parameterPosition;
                }

                //We update the Y for the next block we'll put (move down)
                nextPosition.y -= SpaceBetweenBlocks;

                //If the current block is the head of a compound statement, then we move to it
                if (currentCodeBlockData.FirstBlockInCompoundStatement)
                {
                    currentCodeBlock = currentCodeBlockData.FirstBlockInCompoundStatement;
                    //We update the X for the next block that we will reposition (move to the right / indentation)
                    nextPosition.x += Indentation;
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

                        //Move back to the left / remove extra-indentation
                        nextPosition.x -= Indentation;

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
    }
}
