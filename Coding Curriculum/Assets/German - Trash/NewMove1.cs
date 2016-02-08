using UnityEngine;
using System.Collections;

public class NewMove1 : MonoBehaviour {
	
	Vector3 startPosition;
	Vector3 pos;                                // For movement
	float speed = 2.0f;


	void Start () {
		pos = transform.position;          // Take the initial position
	}

	void FixedUpdate () {
		if(Input.GetKey(KeyCode.A) && transform.position == pos) {        // Left
			pos += Vector3.left;
		}
		if(Input.GetKey(KeyCode.D) && transform.position == pos) {        // Right
			pos += Vector3.right;
		}
		if(Input.GetKey(KeyCode.W) && transform.position == pos) {        // Up
			pos += Vector3.up;

		}
		if(Input.GetKey(KeyCode.S) && transform.position == pos) {        // Down
			pos += Vector3.down;
		}
		startPosition = transform.position;

		transform.position = Vector3.MoveTowards(transform.position, pos, Time.deltaTime * speed);
		print ("I move");
		//if (transform.position != pos) {
			//transform.position = Vector3.MoveTowards(transform.position, startPosition, Time.deltaTime * speed);
	//	}
			
	}
}
