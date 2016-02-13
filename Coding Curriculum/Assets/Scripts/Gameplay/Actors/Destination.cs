using UnityEngine;

public class Destination : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    void OnCollisionEnter(Collider collider)
    {
        Debug.Log("Reached destination");
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Reached destination");
    }
}
