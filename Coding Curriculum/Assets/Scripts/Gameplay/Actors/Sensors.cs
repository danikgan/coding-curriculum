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

    public Structs.MultiTypes CanGoForward(Structs.MultiTypes parameter)        //TODO redo this function
    {
	    var start = transform.position;
        var x = Dictionaries.MovementXY[_movement.Direction];
        var end = start + 32*new Vector3(x.x, x.y, start.z);
        var colliderHit = Physics2D.Linecast(end, start, 1).collider;
        var collision = colliderHit.name != "Player" && !colliderHit.isTrigger;

        print(collision ? "Don't go there boy" : "You good keep going");

        var returnValue = new Structs.MultiTypes { Bool = !collision };
        return returnValue;

    }

}
