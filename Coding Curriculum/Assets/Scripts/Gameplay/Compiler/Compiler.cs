using UnityEngine;

public class Compiler : MonoBehaviour
{
    private SceneReferences _referencesScript;
    private GameObject _startProgramCodeBlock;
    private RunStopSwitcher _runStopSwitcher;

    private GameObject PausedExecution_CodeBlock = null;
    public bool PausedExecution_ReadyToRestart = false;

    private Enumerations.CompilerStatusEnum _compilerStatus;
    private ConsolePrinter _console;

    public Enumerations.CompilerStatusEnum CompilerStatus
    {
        get { return _compilerStatus; }

        set
        {
            _compilerStatus = value;

            if (value == Enumerations.CompilerStatusEnum.Runing)
            {
                _referencesScript.Console.GetComponent<ConsolePrinter>().ResetConsole();
                _runStopSwitcher.ShowStop();
                RunCode();
            }
            else
            {
                _runStopSwitcher.ShowRun();
                StopCode();
                _referencesScript.Player.GetComponent<CharacterMovement>().GoToStartingPosition();
            }
        }
    }

    void Awake()
    {
        var mainCamera = GameObject.Find("Main Camera");
        if (mainCamera)
            _referencesScript = mainCamera.GetComponent<SceneReferences>();
        else
            Debug.LogError("Error: Main Camera not found");

        _startProgramCodeBlock = _referencesScript.StartProgramCodeBlock;
        _runStopSwitcher = _referencesScript.RunStopButton.GetComponent<RunStopSwitcher>();
        _console = _referencesScript.Console.GetComponent<ConsolePrinter>();
    }

    void Start()
    {
        CompilerStatus = Enumerations.CompilerStatusEnum.Stoped;
    }

    void Update()
    {
        if (CompilerStatus == Enumerations.CompilerStatusEnum.Stoped) return;
        if (!PausedExecution_ReadyToRestart) return;

        PausedExecution_ReadyToRestart = false;
        if (PausedExecution_CodeBlock != null)
        {
            var codeBlock = PausedExecution_CodeBlock;
            PausedExecution_CodeBlock = null;
            var compilingError = ExecuteCodeBlocks(codeBlock);
            if (compilingError != null)
            {
                _console.PushError("Compiling error: " + compilingError);
            }
        }
        else
        {
            _console.PushText("The execution of your program has got to an end.");
        }
    }

    void RunCode()
    {
        _console.PushText("Start running program...", "green");

        var compilingError = ExecuteCodeBlocks(_startProgramCodeBlock);
        if (compilingError != null)
        {
            _console.PushError("Compiling error: " + compilingError);
        }
    }

    internal void StopCode()
    {
        PausedExecution_ReadyToRestart = false;
        PausedExecution_CodeBlock = null;
    }

    public void ReverseCompilerStatus()
    {
        CompilerStatus = (Enumerations.CompilerStatusEnum)(1 - (int)(CompilerStatus));
    }

    public string ExecuteCodeBlocks(GameObject startCodeBlock)
    {
        var startCodeBlockData = startCodeBlock.GetComponent<CodeBlockData>();
        var currentCodeBlock = startCodeBlockData.Type == "Start" ? startCodeBlockData.FirstBlockInCompoundStatement : startCodeBlock;

        /*Next, we are using a iterative DFS type algorithm to reposition code blocks*/

        while (currentCodeBlock && CompilerStatus == Enumerations.CompilerStatusEnum.Runing)
        {
            //We move into updating the position of all the following code blocks
            while (currentCodeBlock && CompilerStatus == Enumerations.CompilerStatusEnum.Runing)
            {
                var currentCodeBlockData = currentCodeBlock.GetComponent<CodeBlockData>();

                var parameterData = new Structs.MultiTypes();
                CodeBlockData parameterCodeBlock = null;
                
                if (currentCodeBlockData.ParameterBlock)
                {
                    parameterCodeBlock = currentCodeBlockData.ParameterBlock.GetComponent<CodeBlockData>();
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
                        return "You forgot to attach a parameter block to one of the while blocks.";
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
                            var elseData = currentCodeBlockData.NextBlock.GetComponent<CodeBlockData>();
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
                        return "You forgot to attach a parameter block to one of the if blocks.";
                    }
                }

                if (currentCodeBlock != null && currentCodeBlockData.Meaning != "if" && currentCodeBlockData.Meaning != "else" && currentCodeBlockData.Meaning != "while")
                {
                    if (currentCodeBlockData.EvaluateDelegate != null)
                        currentCodeBlockData.EvaluateDelegate(parameterData);
                    else
                        Debug.LogError("Error: No delegate attached");

                    if (currentCodeBlockData.Meaning == "go_forward")
                    {
                        if (currentCodeBlockData.NextBlock)
                            PausedExecution_CodeBlock = currentCodeBlockData.NextBlock;
                        else
                        {
                            /*          var gofwd_headCodeBlock = currentCodeBlockData.HeadOfCompoundStatement;
                            var gofwd_headCodeBlockData = gofwd_headCodeBlock.GetComponent<CodeBlockData>();
                            PausedExecution_CodeBlock = gofwd_headCodeBlockData.Meaning == "while"
                                ? gofwd_headCodeBlock
                                : gofwd_headCodeBlockData.NextBlock;*/

                            while (true)
                            {
                                if (currentCodeBlockData.HeadOfCompoundStatement)
                                {
                                    currentCodeBlock = currentCodeBlockData.HeadOfCompoundStatement;
                                    currentCodeBlockData = currentCodeBlock.GetComponent<CodeBlockData>();
                                    if (currentCodeBlockData.Meaning == "while") break;
                                    if (!currentCodeBlockData.NextBlock) continue;
                                    currentCodeBlock = currentCodeBlockData.NextBlock;
                                    break;
                                }
                                currentCodeBlock = null;
                                break;
                            }
                            PausedExecution_CodeBlock = currentCodeBlock;
                        }
                        PausedExecution_ReadyToRestart = false;
                       // return "PausedExecution";
                        return null;
                    }
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

                while (true)
                {
                    if (currentCodeBlockData.HeadOfCompoundStatement)
                    {
                        currentCodeBlock = currentCodeBlockData.HeadOfCompoundStatement;
                        currentCodeBlockData = currentCodeBlock.GetComponent<CodeBlockData>();
                        if (currentCodeBlockData.Meaning == "while") break;
                        if (!currentCodeBlockData.NextBlock) continue;
                        currentCodeBlock = currentCodeBlockData.NextBlock;
                        break;
                    }
                    currentCodeBlock = null;
                    break;
                }
            }
        }
        return null;
    }
}
