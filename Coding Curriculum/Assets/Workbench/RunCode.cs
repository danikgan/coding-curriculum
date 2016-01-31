using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class RunCode : MonoBehaviour
{
    public References ReferencesScript;

    private GameObject DropArea;

    struct Instruction
    {
        public float X;
        public float Y;
        public string command;
    };

    private List<Instruction> Instructions;

    void Start()
    {
        DropArea = ReferencesScript.DropArea;
    }

    void OnMouseDown()
    {
        RunUserCode();
    }

    public void RunUserCode()
    {
        Instructions = new List<Instruction>();
        ParseInstructionObjects();
        SortInstructions();

        foreach (var VARIABLE in Instructions)
        {
            Debug.Log(VARIABLE.command);
        }
    }

    void ParseInstructionObjects()
    {
        GameObject[] allChildren = DropArea.transform.Cast<Transform>().Select(t => t.gameObject).ToArray();
        foreach (GameObject child in allChildren)
        {
            Instruction objectInstruction = new Instruction();
            objectInstruction.X = child.transform.position.x;
            objectInstruction.Y = child.transform.position.y;
            objectInstruction.command = child.transform.tag;
            Instructions.Add(objectInstruction);
        }
    }

    void SortInstructions()
    {
        Instructions.Sort((a, b) => b.Y.CompareTo(a.Y));
    }
}
