using UnityEngine;
using System.Collections;

public class Move3 : MonoBehaviour {


	public Rigidbody rb;

	void Start () {

		rb = GetComponent<Rigidbody>();

	}

	void Update () {

		var rigidBody = GetComponent<Rigidbody2D>();


		Vector3 movementVetor = new Vector3();
		Vector3 nextpos = new Vector3();
		Vector3 speed = new Vector3();


		if (Input.GetKeyDown(KeyCode.A)) {     // Left
			// rigidBody.velocity = new Vector2((float) (-32* 0.032), 0);
			print("Key A pressed");
			movementVetor = new Vector3(-32, 0);
			nextpos = transform.position + movementVetor;
			print("Move assigned");
		}
		if(Input.GetKeyDown(KeyCode.D)) {        // Right
			//rigidBody.velocity = new Vector2((float) (32* 0.032), 0);
			print("Key D pressed");
			movementVetor = new Vector3(32, 0);
			nextpos = transform.position + movementVetor;
			print("Move assigned");
		} 
		if(Input.GetKeyDown(KeyCode.W)) {        // Up
			//rigidBody.velocity = new Vector2(0, (float)(32 * 0.032));
			print("Key W pressed");
			movementVetor = new Vector3(0, 32);
			nextpos = transform.position + movementVetor;
			print("Move assigned");

		}
		if(Input.GetKeyDown(KeyCode.S)) {        // Down
			// rigidBody.velocity = new Vector2(0, (float) (-32 * 0.032));
			print("Key S pressed");
			movementVetor = new Vector2(0, -32);
			nextpos = transform.position + movementVetor;
			print("Move assigned");
		}

		// speed = (nextpos-transform.position).normalized * 2f;
		rb.velocity = (nextpos-transform.position)/Time.deltaTime;

		//Vector3 endPos = transform.position + pos * Time.deltaTime * speed;

		// if (pos == startPosition)
		//     return;

		//var x = Obstacles.GetComponent<PolygonCollider2D>().bounds.Contains(pos);

		//  if (!Obstacles.GetComponent<PolygonCollider2D>().bounds.Contains(pos))



		//  float moveHorizontal = Input.GetAxis("Horizontal");
		//    float moveVertical = Input.GetAxis("Vertical");

		// rigidBody.velocity = new Vector2(Mathf.Ceil(moveHorizontal) * speed, Mathf.Ceil(moveVertical) * speed);


		//transform.position = Vector3.MoveTowards(transform.position, pos, Time.deltaTime * speed);
		//  print ("I move");
		//if (transform.position != pos) {
		//transform.position = Vector3.MoveTowards(transform.position, startPosition, Time.deltaTime * speed);
		//	}



	}
}
