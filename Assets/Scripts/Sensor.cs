using UnityEngine;
using System.Collections;

public class Sensor : MonoBehaviour {

	Vector3 start; //current position
	Vector3 end; //point that is checked for being inside of collider
	bool collision; //was there a collsion or not
	CharacterMovement movement;

	void Start (){

		movement = gameObject.GetComponent<CharacterMovement>();
		//access to CharMovement script
	}


	void Update() {

		if (Input.GetKeyDown (KeyCode.S)) {

			collision = CollisionSensor ();

			if (collision)
				print ("Don't go there boy");
			else
				print ("You good keep going");
		}
	}

	public bool CollisionSensor ()
	{
		start = transform.position; //current position
		end = start + goalPosition(); //checked point  is where player currently is + how much he will move
		bool collision = false; //no collision so far

		//track backwards - from end to start, if there is any collision
		if (Physics2D.Linecast(end, start,1).collider.name != "Player") //if collsion with any other object other than Player
		{
			collision = true;
			//there was a collision
		}

		return collision;

	}


	private Vector3 goalPosition (){
		//determines by how much and where Player will be moving according to his position
		switch (movement.direction) {

		case "-y":
			return	new Vector3 (0, -32, 0);

		case "y":
			return	new Vector3 (0, 32, 0);

		case "x":
			return	new Vector3 (32, 0, 0);

		case "-x":
			return	new Vector3 (-32, 0, 0);
		}

		return new Vector3 (0, 0, 0);

	}
}
