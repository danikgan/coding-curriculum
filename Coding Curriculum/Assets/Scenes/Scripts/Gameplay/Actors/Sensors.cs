using UnityEngine;

public class Sensors : MonoBehaviour
{

	//Vector3 start; //current position
	//Vector3 end; //point that is checked for being inside of collider
	//bool collision; //was there a collsion or not
	CharacterMovement _movement;

	void Start ()
    {
		_movement = gameObject.GetComponent<CharacterMovement>();
	}

    /*void Update() {

		if (Input.GetKeyDown (KeyCode.S)) {

			collision = CollisionSensor ();


		}
	}*/

    public Structs.MultiTypes CanGoForward(Structs.MultiTypes parameter)        //TODO redo this function
    {
	    Vector3 start = transform.position;
        var x = Dictionaries.MovementXY[_movement.Direction];
        Vector3 end = start + 32*new Vector3(x.x, x.y, start.z);
        bool collision = Physics2D.Linecast(end, start, 1).collider.name != "Player";

        if (collision)
            print("Don't go there boy");
        else
            print("You good keep going");

        var returnValue = new Structs.MultiTypes { Bool = !collision };
        return returnValue;

    }

}
