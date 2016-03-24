using UnityEngine;

public class Enumerations : MonoBehaviour
{
    public enum DirectionsEnum : int
    {
        Down = 0,
        Left = 1,
        Up = 2,
        Right = 3
    };

    public enum CompilerStatusEnum : int
    {
        Stoped = 0,
        Runing = 1
    };
}
