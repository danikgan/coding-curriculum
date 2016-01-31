using UnityEngine;
using System.Collections;

public class NewMove1 : MonoBehaviour {
	
	Vector3 startPosition;
	Vector3 pos;                                // For movement
	float speed = 2.0f;

    public GameObject Map;
    private Transform Obstacles;


	void Start () {
		pos = transform.position;          // Take the initial position
	    Transform groundGameObject = Map.transform.Find("Ground");
	    Obstacles = groundGameObject.Find("Orange_Box32x32");
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
        //Vector3 endPos = transform.position + pos * Time.deltaTime * speed;

	    if (pos == startPosition)
	        return;

	    //var x = Obstacles.GetComponent<PolygonCollider2D>().bounds.Contains(pos);

        if (!Obstacles.GetComponent<PolygonCollider2D>().bounds.Contains(pos))
		    transform.position = Vector3.MoveTowards(transform.position, pos, Time.deltaTime * speed);
		print ("I move");
		//if (transform.position != pos) {
			//transform.position = Vector3.MoveTowards(transform.position, startPosition, Time.deltaTime * speed);
	//	}
			
	}
}
