using JetBrains.Annotations;
using UnityEngine;

[RequireComponent(typeof (BoxCollider2D))]

public class CodeBlockData : MonoBehaviour
{
    public GameObject PreviousBlock = null;
    public GameObject NextBlock = null;
    public GameObject ParameterBlock = null;
    public GameObject FirstBlockInCompoundStatement = null;
    public GameObject HeadOfCompoundStatement = null;

    public GameObject CurrentCodeBlockPrefab = null;

    [NotNull] public string Type;           //Used to describe the type of the block: "Instruction" or "Parameter" or "Start"
    [NotNull] public string Meaning;        //Used to describe the meaning of the current CodeBlock. Use strings such as "go_forward" , "turn_left", 
                                            // "turn_right" , "while", "if", "else", "do_while", "CanGoForward", "StartMain"
    public bool SupportsParameterBlock;
    public bool SupportsCompoundStatement;

    [CanBeNull] public Delegates.EvaluateDelegateType EvaluateDelegate;

    void Start()
    {
        var boxCollider = GetComponent<BoxCollider2D>();

        if (Type != "Start")
            boxCollider.enabled = false;

        if (EvaluateDelegate == null && !SupportsCompoundStatement)
            EvaluateDelegate = ParseCommand();
    }

    private Delegates.EvaluateDelegateType ParseCommand()
    {
        var player = GameObject.Find("Main Camera").GetComponent<SceneReferences>().Player;

        if (Type == "Instruction")
        {
            var characterMovement = player.GetComponent<CharacterMovement>();

            switch (Meaning)
            {
                case "go_forward":
                {
                    return characterMovement.go_forward;
                }

                case "turn_right":
                {
                    return characterMovement.turn_right;
                }

                case "turn_left":
                {
                    return characterMovement.turn_left;
                }

                default:
                {
                    Debug.LogError("Instruction not recognized");
                    return null;
                }
            }
        }


        var sensors = player.GetComponent<Sensors>();

        switch (Meaning)
        {
            case "CanGoForward":
            {
                return sensors.CanGoForward;
            }

            case "noObstaclesAround":
            {
                return sensors.NoObstaclesAround;
            }

            case "notCanGoForward":
            {
                return sensors.NotCanGoForward;
            }

            case "not_ReachedTarget":
            {
                return sensors.NotReachedTarget;
            }

            default:
            {
                Debug.LogError("Expression not recognized");
                return null;
            }
        }
    }
}
