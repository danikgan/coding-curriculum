using System;
using UnityEngine;
using UnityEngine.Networking.NetworkSystem;
using Object = System.Object;

public class RunCode : MonoBehaviour
{
    public SceneReferences SceneReferencesScript;

    private GameObject _startProgramCodeBlock;
    private GameObject _player;
    private CharacterMovement _characterMovement;
    private Sensors _sensors;

    void Start()
    {
        _startProgramCodeBlock = SceneReferencesScript.StartProgramCodeBlock;
        _player = SceneReferencesScript.Player;
        _characterMovement = _player.GetComponent<CharacterMovement>();
        _sensors = _player.GetComponent<Sensors>();
    }

    void OnMouseDown()
    {
        RunUserCode();
    }

    public void RunUserCode()
    {
        //_allChildren = _dropArea.transform.Cast<Transform>().Select(t => t.gameObject).ToArray();
        var startProgramCodeBlockData = _startProgramCodeBlock.GetComponent<CodeBlock>();
        ParseStatementCodeBlocks(startProgramCodeBlockData.NextBlock);
    }

    void ParseStatementCodeBlocks(GameObject currentCodeBlock)
    {
        var codeBlockData = currentCodeBlock.GetComponent<CodeBlock>();
        ParseStatementCodeBlocks(codeBlockData);
    }

    private void ParseStatementCodeBlocks(CodeBlock codeBlockData)
    {
        switch (codeBlockData.Meaning)
        {
            case "go_forward":
            {
                go_forward(codeBlockData);
                break;
            }

            case "turn_right":
            {
                _characterMovement.turn_right();
                break;
            }

            case "turn_left":
            {
                _characterMovement.turn_left();
                break;
            }

            case "IF":
            {
                IF(codeBlockData);
                break;
            }

            case "WHILE":
            {
                WHILE(codeBlockData);
                break;
            }
            default:
            {
                Debug.LogError("Expression not recognized");
                break;
            }
        }
    }

    private void go_forward(CodeBlock codeBlockData)
    {
        if (codeBlockData.ParameterBlock)
        {
            var parameterCodeBlockData = codeBlockData.ParameterBlock.GetComponent<CodeBlock>();
            if (!parameterCodeBlockData.Meaning.StartsWith("Number"))
                Debug.LogError(
                    "Inappropriate parameter for go_forward. The only accepter parameter is a Number Code Block.");

            try
            {
                var number = Convert.ToInt32(parameterCodeBlockData.Meaning.Remove(0, 7));
                for (int i = 1; i < number; i++)
                    _characterMovement.go_forward();
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to extract an integer from a Number Code Block. " + e.Source + " : " + e.Message);
            }
        }
        else
        {
            _characterMovement.go_forward();
        }
    }

    private void IF(CodeBlock codeBlockData)
    {
        if (!codeBlockData.ParameterBlock)          
        {
            Debug.LogError("No condition available for IF");
            return;
        }

        var conditionCodeBlockData = codeBlockData.ParameterBlock.GetComponent<CodeBlock>();

        Object evaluteExpressionFunction = EvaluateExpressionCodeBlock(conditionCodeBlockData);             // ???
        //The result will be either a Bool or an int or a double, so just use double and represent bool as 1 and 0 if this doesn't work
       // Object evaluatedConditonCodeBlock = evaluteExpressionFunction;                                      // ???

        if (TurnToBOOL(evaluteExpressionFunction))       //Evaluate IF condition
        {
            //Condition evaluated as FALSE. Check if there is an ELSE attached to this IF and run it
            if (!codeBlockData.NextBlock)
                return;
            var getELSE = codeBlockData.NextBlock;
            var getELSEData = getELSE.GetComponent<CodeBlock>();
            if (getELSEData.Meaning != "ELSE")      //Check if the next block is an ELSE
                return;

            //The next block is an ELSE so we'll run the compound statement which has ELSE as its head
            var firstCodeBlockInCompoundStatement = getELSEData.FirstBlockInCompoundStatement;
            ParseStatementCodeBlocks(firstCodeBlockInCompoundStatement);

        }
        else
        {
            //Condition evaluated as TRUE. Run the compound statement which has IF as its head
            var firstCodeBlockInCompoundStatement = codeBlockData.FirstBlockInCompoundStatement;
            ParseStatementCodeBlocks(firstCodeBlockInCompoundStatement);
        }

    }

    delegate bool BoolDel();

    private Object EvaluateExpressionCodeBlock(CodeBlock expresionCodeBlockData)
    {
        switch (expresionCodeBlockData.Meaning)
        {
            case "CanGoForward":
            {
                    //return _sensors.CanGoForward();
                BoolDel canGoForwardDel = _sensors.CanGoForward;
                return canGoForwardDel;
            }

            default:
            {
                Debug.LogError("Expression not recognized");
                return new ErrorMessage();
            }
        }
    }

    private static bool TurnToBOOL(Object evaluatedConditonCodeBlock)
    {
        return Convert.ToInt32(evaluatedConditonCodeBlock) == 0 || evaluatedConditonCodeBlock == null ||
               Convert.ToBoolean(evaluatedConditonCodeBlock) == false;
    }

    private void WHILE(CodeBlock codeBlockData)
    {
        if (!codeBlockData.ParameterBlock)
        {
            Debug.LogError("No condition available for WHILE");
            return;
        }

        var conditionCodeBlockData = codeBlockData.ParameterBlock.GetComponent<CodeBlock>();

        Object evaluteExpressionFunction = EvaluateExpressionCodeBlock(conditionCodeBlockData);             // ???
       // Object evaluatedConditonCodeBlock = evaluteExpressionFunction;                                      // ???

        while (TurnToBOOL(evaluteExpressionFunction)) //Evaluate WHILE condition
        {
            ParseStatementCodeBlocks(codeBlockData.FirstBlockInCompoundStatement);
        }
    }
}
