using UnityEngine;
using System.Collections;

public class NewMove1 : MonoBehaviour {
	
	Vector3 startPosition;
	Vector3 pos;                                // For movement
	//float speed = 0.05f;

    public GameObject Map;
    private Transform Obstacles;


	void Start () {
		pos = transform.position;          // Take the initial position
	    Transform groundGameObject = Map.transform.Find("Ground");
	    Obstacles = groundGameObject.Find("Orange_Box32x32");
	}

	void FixedUpdate () {

        var rigidBody = GetComponent<Rigidbody2D>();

        Vector2 movementVetor = new Vector2();

        if (Input.GetKey(KeyCode.A)) {        // Left
           // rigidBody.velocity = new Vector2((float) (-32* 0.032), 0);
           movementVetor = new Vector2(-32,0);
        }
		if(Input.GetKey(KeyCode.D)) {        // Right
            //rigidBody.velocity = new Vector2((float) (32* 0.032), 0);
            movementVetor = new Vector2(32, 0);
        }
		if(Input.GetKey(KeyCode.W)) {        // Up
            //rigidBody.velocity = new Vector2(0, (float)(32 * 0.032));
            movementVetor = new Vector2(0, 32);

        }
		if(Input.GetKey(KeyCode.S)) {        // Down
          // rigidBody.velocity = new Vector2(0, (float) (-32 * 0.032));
            movementVetor = new Vector2(0, -32);
        }
		//startPosition = transform.position;
        if(!movementVetor.Equals(new Vector2()))
            rigidBody.MovePosition(rigidBody.position + movementVetor);
        //Vector3 endPos = transform.position + pos * Time.deltaTime * speed;

        // if (pos == startPosition)
        //     return;

        //var x = Obstacles.GetComponent<PolygonCollider2D>().bounds.Contains(pos);

        //  if (!Obstacles.GetComponent<PolygonCollider2D>().bounds.Contains(pos))



        //  float moveHorizontal = Input.GetAxis("Horizontal");
        //    float moveVertical = Input.GetAxis("Vertical");

        // rigidBody.velocity = new Vector2(Mathf.Ceil(moveHorizontal) * speed, Mathf.Ceil(moveVertical) * speed);


        //transform.position = Vector3.MoveTowards(transform.position, pos, Time.deltaTime * speed);
        print ("I move");
		//if (transform.position != pos) {
			//transform.position = Vector3.MoveTowards(transform.position, startPosition, Time.deltaTime * speed);
	//	}
			
	}
}
