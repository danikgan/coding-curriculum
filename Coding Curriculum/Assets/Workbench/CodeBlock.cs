using UnityEngine;

public class CodeBlock : MonoBehaviour
{
    public GameObject PreviousBlock { get; } = null;
    public GameObject NextBlock { get; } = null;
    public GameObject ParameterBlock { get; } = null;
    public GameObject FirstBlockInCompoundStatement { get; } = null;
    public GameObject HeadOfCompoundStatement { get; } = null;

    public string Meaning = null;                           //Used to describe the meaning of th current codeblock. Use strings such as "go_forward" , "turn_left", 
                                                            // "turn_right" , "while", "if", "else", "do_while", "CanGoForward"
}
