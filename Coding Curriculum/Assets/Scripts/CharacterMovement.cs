using System.Collections;
using UnityEngine;

class CharacterMovement : MonoBehaviour {

	private float speed = 64f; //movememnt speed
	private float size = 32f; //size of a tile
	private Vector2 newVector; //by how much Player will move
	private bool inMove = false; //is Player moving or not
	private Vector2 start; //start position of Player
	private Vector2 end; //end position of Player
	public string direction; //direction of Player's movement: x,-x,y,-y
	private Animator animation; //access to animation controller

	void Start ()
	{
		direction = "-y"; //initilal direction that Player is facing on start - negative Y
		animation = GetComponent<Animator> (); 
	}

	public void Update() {

		if (Input.GetKeyDown (KeyCode.R)) // turn right on R pressed
			turnRight();
		if (Input.GetKeyDown (KeyCode.L)) //turn left on L pressed
			turnLeft();
		if (Input.GetKeyDown (KeyCode.F)) // go forward by one tile on F pressed
			goForward();
	}


	public void turnRight ()
	{
		switch (direction) //switches direction of Player according to his current direction (clockwise)
		//also changes animation accordingly
		{
		case "-y":
			direction = "-x";
			animation.SetFloat ("Input_x", -1);
			animation.SetFloat ("Input_y", 0);
			break;
		case "y":
			direction = "x";
			animation.SetFloat ("Input_x", 1);
			animation.SetFloat ("Input_y", 0);
			break;
		case "-x":
			direction = "y";
			animation.SetFloat ("Input_x", 0);
			animation.SetFloat ("Input_y", 1);
			break;
		case "x":
			direction = "-y";
			animation.SetFloat ("Input_x", 0);
			animation.SetFloat ("Input_y", -1);
			break;
		}
	}

	public void turnLeft () //switches direction of Player according to his current direction (anti-clockwise)
	//also changes animation accordingly
	{
		switch (direction) 
		{
		case "-y":
			direction = "x";
			animation.SetFloat ("Input_x", 1);
			animation.SetFloat ("Input_y", 0);
			break;
		case "y":
			direction = "-x";
			animation.SetFloat ("Input_x", -1);
			animation.SetFloat ("Input_y", 0);
			break;
		case "x":
			direction = "y";
			animation.SetFloat ("Input_x", 0);
			animation.SetFloat ("Input_y", 1);
			break;
		case "-x":
			direction = "-y";
			animation.SetFloat ("Input_x", 0);
			animation.SetFloat ("Input_y", -1);
			break;
		}
	}


	public void goForward (){
		//moves Player one tile in his current direction 


		if (!inMove) { //if is not moving (so each movement step is completed before the next starts)

			Vector2 newVector = new Vector2 ();



			if (direction == "-x") {     // goes left

				newVector = new Vector2 (-1.0f, 0);
				animation.SetBool ("IsWalking", true);
				animation.SetFloat ("Input_x", -1);
				animation.SetFloat ("Input_y", 0);
			}
			if (direction == "x") {        // goes right

				newVector = new Vector2 (1.0f, 0);
				animation.SetBool ("IsWalking", true);
				animation.SetFloat ("Input_x", 1);
				animation.SetFloat ("Input_y", 0);
			}
			if (direction == "y") {        // goes up

				newVector = new Vector2 (0, 1.0f);
				animation.SetBool ("IsWalking", true);
				animation.SetFloat ("Input_y", 1);
				animation.SetFloat ("Input_x", 0);

			}
			if (direction == "-y") {        // goes down

				newVector = new Vector2 (0, -1.0f);
				animation.SetBool ("IsWalking", true);
				animation.SetFloat ("Input_y", -1);
				animation.SetFloat ("Input_x", 0);
			}

			//when a new coordinate is entered into the newVector, second one has to be erased
			if (Mathf.Abs (newVector.x) > Mathf.Abs (newVector.y)) {
				newVector.y = 0;
			} else {
				newVector.x = 0;
			}


			StartCoroutine (movement(transform, newVector));

		}
	}


	public IEnumerator movement(Transform transform, Vector2 newVector) {
		//performs actual movement

		inMove = true;
		start = transform.position; //current position
		float i = 0;

		var rigid = GetComponent<Rigidbody2D>(); //access to rigidbody component of Player

		end = new Vector2(start.x + System.Math.Sign(newVector.x) * size, start.y +System.Math.Sign(newVector.y) * size);
		//where Player will be moved to

		while (i < 1f) {
			//gradual movement from start to end
			i += Time.deltaTime * (speed/size);
			rigid.MovePosition (Vector2.Lerp(start, end, i));

			yield
			return null;
		}
		//finished, not moving anymore
		inMove = false;
		animation.SetBool ("IsWalking", false);
		yield return 0;
	}
}