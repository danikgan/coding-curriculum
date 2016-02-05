using System.Collections;
using UnityEngine;

class SmoothAnim : MonoBehaviour {
	
	private float moveSpeed = 70f;
	private float gridSize = 32f;
	private Vector2 input;
	private bool isMoving = false;
	private Vector2 startPosition;
	private Vector2 endPosition;
	private float t;
	private bool pressed;
	private string position = "-y";
	Animator anim;


	public void Update() {

		if (!isMoving) {

			anim = GetComponent<Animator> ();
			Vector2 input = new Vector2 ();
			pressed = false;

			anim.SetBool ("IsWalking", false);

			if (Input.GetKeyDown (KeyCode.A)) {     // Left

				print ("Key A pressed");
				input = new Vector2 (-1.0f, 0);
				pressed = true;
				anim.SetBool ("IsWalking", true);
				anim.SetFloat ("Input_x", -1);
				anim.SetFloat ("Input_y", 0);
			}
			if (Input.GetKeyDown (KeyCode.D)) {        // Right

				input = new Vector2 (1.0f, 0);
				pressed = true;
				anim.SetBool ("IsWalking", true);
				anim.SetFloat ("Input_x", 1);
				anim.SetFloat ("Input_y", 0);
			}
			if (Input.GetKeyDown (KeyCode.W)) {        // Up

				input = new Vector2 (0, 1.0f);
				pressed = true;
				anim.SetBool ("IsWalking", true);
				anim.SetFloat ("Input_y", 1);
				anim.SetFloat ("Input_x", 0);

			}
			if (Input.GetKeyDown (KeyCode.S)) {        // Down

				input = new Vector2 (0, -1.0f);
				pressed = true;
				anim.SetBool ("IsWalking", true);
				anim.SetFloat ("Input_y", -1);
				anim.SetFloat ("Input_x", 0);
			}

			if (Mathf.Abs (input.x) > Mathf.Abs (input.y)) {
				input.y = 0;
			} else {
				input.x = 0;
			}

			if (pressed) {
				StartCoroutine (move (transform, input));
			}


		}

	}

	public IEnumerator move(Transform transform, Vector2 input) {
		isMoving = true;
		startPosition = transform.position;
		t = 0;

		var rigidBody = GetComponent<Rigidbody2D>();
		endPosition = new Vector2(startPosition.x + System.Math.Sign(input.x) * gridSize,
			startPosition.y +System.Math.Sign(input.y) * gridSize);

		while (t < 1f) {
			t += Time.deltaTime * (moveSpeed/gridSize);
			rigidBody.MovePosition (rigidBody.position + (Vector2.Lerp(startPosition, endPosition, t) - rigidBody.position));

			yield
			return null;
		}

		isMoving = false;
		yield return 0;
	}
}