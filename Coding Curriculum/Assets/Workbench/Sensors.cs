using UnityEngine;

public class Sensors : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
    public Structs.MultiTypes CanGoForward(Structs.MultiTypes parameter)
    {
        var returnValue = new Structs.MultiTypes {Bool = true};
        return returnValue;
    }
}
