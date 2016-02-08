using System.Collections;
using UnityEngine;

class Functions1 : MonoBehaviour {

	private float speed = 70f;
	private float size = 32f;
	private Vector2 input;
	private bool isMoving = false;
	private Vector2 startPosition;
	private Vector2 endPosition;
	private bool pressed;
	public string position;
	private Animator animation;

	void Start ()
	{
		position = "-y";
		animation = GetComponent<Animator> ();
	}

	public void Update() {

		if (Input.GetKeyDown (KeyCode.R))
			turnRight();
		if (Input.GetKeyDown (KeyCode.L))
			turnLeft();
		if (Input.GetKeyDown (KeyCode.F))
			goForward();
	}
		

	public void turnRight ()
	{
		switch (position) 
		{
		case "-y":
			position = "-x";
			animation.SetFloat ("Input_x", -1);
			animation.SetFloat ("Input_y", 0);
			break;
		case "y":
			position = "x";
			animation.SetFloat ("Input_x", 1);
			animation.SetFloat ("Input_y", 0);
			break;
		case "-x":
			position = "y";
			animation.SetFloat ("Input_x", 0);
			animation.SetFloat ("Input_y", 1);
			break;
		case "x":
			position = "-y";
			animation.SetFloat ("Input_x", 0);
			animation.SetFloat ("Input_y", -1);
			break;
		}
	}

	public void turnLeft ()
	{
		switch (position) 
		{
		case "-y":
			position = "x";
			animation.SetFloat ("Input_x", 1);
			animation.SetFloat ("Input_y", 0);
			break;
		case "y":
			position = "-x";
			animation.SetFloat ("Input_x", -1);
			animation.SetFloat ("Input_y", 0);
			break;
		case "x":
			position = "y";
			animation.SetFloat ("Input_x", 0);
			animation.SetFloat ("Input_y", 1);
			break;
		case "-x":
			position = "-y";
			animation.SetFloat ("Input_x", 0);
			animation.SetFloat ("Input_y", -1);
			break;
		}
	}
	 

	public void goForward (){


		if (!isMoving) {

			Vector2 input = new Vector2 ();
			pressed = false;
		

			if (position == "-x") {     // Left

				input = new Vector2 (-1.0f, 0);
				pressed = true;
				animation.SetBool ("IsWalking", true);
				animation.SetFloat ("Input_x", -1);
				animation.SetFloat ("Input_y", 0);
			}
			if (position == "x") {        // Right

				input = new Vector2 (1.0f, 0);
				pressed = true;
				animation.SetBool ("IsWalking", true);
				animation.SetFloat ("Input_x", 1);
				animation.SetFloat ("Input_y", 0);
			}
			if (position == "y") {        // Up

				input = new Vector2 (0, 1.0f);
				pressed = true;
				animation.SetBool ("IsWalking", true);
				animation.SetFloat ("Input_y", 1);
				animation.SetFloat ("Input_x", 0);

			}
			if (position == "-y") {        // Down

				input = new Vector2 (0, -1.0f);
				pressed = true;
				animation.SetBool ("IsWalking", true);
				animation.SetFloat ("Input_y", -1);
				animation.SetFloat ("Input_x", 0);
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
		float i = 0;

		var rigidBody = GetComponent<Rigidbody2D>();
		endPosition = new Vector2(startPosition.x + System.Math.Sign(input.x) * size,
			startPosition.y +System.Math.Sign(input.y) * size);

		while (i < 1f) {
			i += Time.deltaTime * (speed/size);
			rigidBody.MovePosition (rigidBody.position + (Vector2.Lerp(startPosition, endPosition, i) - rigidBody.position));

			yield
			return null;
		}
		isMoving = false;
		animation.SetBool ("IsWalking", false);
		yield return 0;
	}
}