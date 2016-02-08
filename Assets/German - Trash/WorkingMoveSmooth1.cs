using UnityEngine;
using System.Collections;

public class WorkingMoveSmooth1 : MonoBehaviour {

	bool a;
	float t;
	float movespeed = 10f;

	void Start () {

	}

	void Update () {

		var rigidBody = GetComponent<Rigidbody2D>();
		Vector2 movementVetor = new Vector2();
		Vector2 start = new Vector2();
		Vector2 finish = new Vector2();
		a = false;
		t = 0;

		if (Input.GetKeyDown(KeyCode.A)) {     // Left

			print("Key A pressed");
			movementVetor = new Vector2(-32, 0);
			print("Move assigned");
			Debug.Log(movementVetor);
			a = true;
		}
		if(Input.GetKeyDown(KeyCode.D)) {        // Right

			print("Key D pressed");
			movementVetor = new Vector2(32, 0);
			print("Move assigned");
			Debug.Log(movementVetor);
			a = true;
		}
		if(Input.GetKeyDown(KeyCode.W)) {        // Up

			print("Key W pressed");
			movementVetor = new Vector2(0, 32);
			print("Move assigned");
			Debug.Log(movementVetor);
			a = true;

		}
		if(Input.GetKeyDown(KeyCode.S)) {        // Down

			print("Key S pressed");
			movementVetor = new Vector2(0, -32);
			print("Move assigned");
			Debug.Log(movementVetor);
			a = true;

		}

		start = rigidBody.position;
		finish = rigidBody.position + movementVetor;

		if (a) {
			while (t < 1f) {
				Debug.Log (Vector2.Lerp(start, finish, t));
				t += Time.deltaTime/10f;
				Debug.Log (t);
				GetComponent<Rigidbody2D>().position =  Vector2.Lerp(start, finish, t);
			}
		}



	}
}


