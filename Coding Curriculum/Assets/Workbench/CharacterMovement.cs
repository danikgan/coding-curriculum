using UnityEngine;

public class CharacterMovement : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    public Structs.MultiTypes go_forward(Structs.MultiTypes parameter)
    {
        Debug.Log("go forward");
        return new Structs.MultiTypes();
    }

    public Structs.MultiTypes turn_right(Structs.MultiTypes parameter)
    {
        Debug.Log("turn right");
        return new Structs.MultiTypes();
    }

    public Structs.MultiTypes turn_left(Structs.MultiTypes parameter)
    {
        Debug.Log("turn_left");
        return new Structs.MultiTypes();
    }
}
