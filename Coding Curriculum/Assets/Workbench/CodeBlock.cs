using JetBrains.Annotations;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]

public class CodeBlock : MonoBehaviour
{
    public GameObject PreviousBlock = null;
    public GameObject NextBlock = null;
    public GameObject ParameterBlock = null;
    public GameObject FirstBlockInCompoundStatement = null;
    public GameObject HeadOfCompoundStatement = null;

    [NotNull] public string Type;                              //Used to describe the type of the block: "Instruction" or "Parameter" or "Start"
    [NotNull] public string Meaning;                           //Used to describe the meaning of th current codeblock. Use strings such as "go_forward" , "turn_left", 
                                                               // "turn_right" , "while", "if", "else", "do_while", "CanGoForward", "StartMain"
    public bool SupportsParameterBlock;
    public bool SupportsCompoundStatement;

    void Start()
    {
        if(Type != "Start")
            GetComponent<BoxCollider2D>().enabled = false;
    }
}
