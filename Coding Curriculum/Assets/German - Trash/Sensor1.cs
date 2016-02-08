using UnityEngine;
using System.Collections;

public class Sensor1 : MonoBehaviour {

	Vector3 startPos; 
	Vector3 goal; 
	bool collision;
	Functions1 functions;

	void Start (){
		
		functions = gameObject.GetComponent<Functions1>();
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
		startPos = transform.position;
		goal = startPos + goalPosition();
		bool collision = false;

		if (Physics2D.Linecast(goal, startPos,1).collider.name != "Player")
		{
			collision = true;
		}
			
		return collision;
		
	}


	private Vector3 goalPosition (){

		switch (functions.position) {

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