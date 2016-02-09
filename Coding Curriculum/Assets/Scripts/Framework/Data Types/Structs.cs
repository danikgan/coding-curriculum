using UnityEngine;

public class Structs : MonoBehaviour
{
    public struct MultiTypes
    {
        public bool Bool;
        public int Int;
    }

    public struct XYpair
    {
        public float x { get; set; }
        public float y { get; set; }

        public XYpair(float x, float y) : this()
        {
            this.x = x;
            this.y = y;
        }
    };
}