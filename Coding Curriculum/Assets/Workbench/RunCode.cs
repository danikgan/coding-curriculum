using System;
using JetBrains.Annotations;
using UnityEngine;

public class RunCode : MonoBehaviour
{
    [NotNull] private SceneReferences _referencesScript;

    private GameObject _startProgramCodeBlock;

    void Start()
    {
        var mainCamera = GameObject.Find("Main Camera");
        if (mainCamera)
            _referencesScript = mainCamera.GetComponent<SceneReferences>();
        else
            Debug.LogError("Error: Main Camera not found");

        _startProgramCodeBlock = _referencesScript.StartProgramCodeBlock;
    }

    void OnMouseDown()
    {
        var compilingError = ExecuteCodeBlocks(_startProgramCodeBlock);
        if (compilingError != null)
        {
            Debug.Log("Compiling error: " + compilingError); //TODO: Show them to the user
        }
    }

    public String ExecuteCodeBlocks(GameObject startCodeBlock)
    {
        var currentCodeBlock = startCodeBlock.GetComponent<CodeBlock>().FirstBlockInCompoundStatement;

        /*Next, we are using a iterative DFS type algorithm to reposition code blocks*/

        while (currentCodeBlock)
        {
            //We move into updating the position of all the following code blocks
            while (currentCodeBlock)
            {
                var currentCodeBlockData = currentCodeBlock.GetComponent<CodeBlock>();

                var parameterData = new Structs.MultiTypes();
                CodeBlock parameterCodeBlock = null;
                
                if (currentCodeBlockData.ParameterBlock)
                {
                    parameterCodeBlock = currentCodeBlockData.ParameterBlock.GetComponent<CodeBlock>();
                    if (parameterCodeBlock.EvaluateDelegate != null)
                        parameterData = parameterCodeBlock.EvaluateDelegate(new Structs.MultiTypes());
                    else
                    {
                        Debug.LogError("Error: No delegate attached");
                    }
                }

                if (currentCodeBlockData.Meaning == "while")
                {
                    if (parameterCodeBlock)
                    {
                        if (parameterData.Bool)
                        {
                            //continue with the content of the compound statement attached to the WHILE
                            currentCodeBlock = currentCodeBlockData.FirstBlockInCompoundStatement;
                            continue;
                        }
                    }
                    else
                    {
                        return "Error: No condition attached to while";
                    }
                }

                if (currentCodeBlockData.Meaning == "if")
                {
                    if (parameterCodeBlock)
                    {
                        if (parameterData.Bool)
                        {
                            //continue with the content of the compound statement attached to the IF
                            currentCodeBlock = currentCodeBlockData.FirstBlockInCompoundStatement;
                            continue;
                        }

                        if (currentCodeBlockData.NextBlock != null)
                        {
                            var elseData = currentCodeBlockData.NextBlock.GetComponent<CodeBlock>();
                            if (elseData.Meaning == "else")
                            {
                                //continue with the content of the compound statement attached to the ELSE
                                currentCodeBlock = elseData.FirstBlockInCompoundStatement;
                                continue;
                            }
                        }
                    }
                    else
                    {
                        return "Error: No condition attached to if";
                    }
                }

                if (currentCodeBlock != null && currentCodeBlockData.Meaning != "else")
                {
                    if (currentCodeBlockData.EvaluateDelegate != null)
                        currentCodeBlockData.EvaluateDelegate(parameterData);
                    else
                        Debug.LogError("Error: No delegate attached");
                }

                if (currentCodeBlockData.NextBlock)
                {
                    //continue with the next block
                    currentCodeBlock = currentCodeBlockData.NextBlock;
                    continue;
                }

                /*
                    There is no NextBlock so we've got at the end of the current compound statement.
                    We check if we were in a repetitive structure because in that case, we need to also check the condition.
                    To avoid rewriting the code for checking the condition, we will simply set currentCodeBlock to
                    HeadOfCompoundStatement if HeadOfCompoundStatement is a WHILE and to HeadOfCompoundStatement.NextBlock
                    if it's not.

                    If there is no head, that means that we are at the first block (a Start block) so we assign null to 
                    currentCodeBlock so that both While will end
                */

                var headCodeBlock = currentCodeBlockData.HeadOfCompoundStatement;
                if (headCodeBlock)
                {
                    var headCodeBlockData = headCodeBlock.GetComponent<CodeBlock>();
                    currentCodeBlock = headCodeBlockData.Meaning == "while"
                        ? headCodeBlock
                        : headCodeBlockData.NextBlock;
                    continue;
                }

                currentCodeBlock = null;
            }
        }

        return null;
    }



    /*private void go_forward(CodeBlock codeBlockData) //TODO implement this into final code
    {
        var number = 1;
        if (codeBlockData.ParameterBlock)
        {
            var parameterCodeBlockData = codeBlockData.ParameterBlock.GetComponent<CodeBlock>();
            if (!parameterCodeBlockData.Meaning.StartsWith("Number"))
                Debug.LogError(
                    "Inappropriate parameter for go_forward. The only accepter parameter is a Number Code Block.");  //TODO: Return it to the user
            try
            {
                number = Convert.ToInt32(parameterCodeBlockData.Meaning.Remove(0, 7));
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to extract an integer from a Number Code Block. " + e.Source + " : " + e.Message);
            }
        }

        while (number-- != 0)
            _characterMovement.go_forward();
    }*/
}
