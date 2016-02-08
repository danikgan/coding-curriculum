using UnityEngine;
using System.Collections;

public class WorkingMove1 : MonoBehaviour {

	bool a;
	Animator anim;

	void Start () {
		anim = GetComponent<Animator> ();
	}

	void Update () {

		var rigidBody = GetComponent<Rigidbody2D>();
		Vector2 movementVetor = new Vector2();
		a = false;

		anim.SetBool("IsWalking", false);

		if (Input.GetKeyDown(KeyCode.A)) {     // Left

			print("Key A pressed");
			movementVetor = new Vector2(-32, 0);
			print("Move assigned");
			Debug.Log(movementVetor);
			a = true;
			anim.SetBool("IsWalking", true);
			anim.SetFloat("Input_x", -1);
		}
		if(Input.GetKeyDown(KeyCode.D)) {        // Right

			print("Key D pressed");
			movementVetor = new Vector2(32, 0);
			print("Move assigned");
			Debug.Log(movementVetor);
			a = true;
			anim.SetBool("IsWalking", true);
			anim.SetFloat("Input_x", 1);
		}
		if(Input.GetKeyDown(KeyCode.W)) {        // Up

			print("Key W pressed");
			movementVetor = new Vector2(0, 32);
			print("Move assigned");
			Debug.Log(movementVetor);
			a = true;
			anim.SetBool("IsWalking", true);
			anim.SetFloat("Input_y", 1);

		}
		if(Input.GetKeyDown(KeyCode.S)) {        // Down

			print("Key S pressed");
			movementVetor = new Vector2(0, -32);
			print("Move assigned");
			Debug.Log(movementVetor);
			a = true;
			anim.SetBool("IsWalking", true);
			anim.SetFloat("Input_y", -1);

		}

		if (a) {
			Debug.Log (rigidBody.position + movementVetor);
			rigidBody.MovePosition (rigidBody.position + movementVetor);
		}



	}
}
