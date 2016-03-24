using System.Linq;
using UnityEngine;

public class Sensors : MonoBehaviour
{
	CharacterMovement _movement;

	void Start ()
    {
		_movement = gameObject.GetComponent<CharacterMovement>();
	}

    public Structs.MultiTypes CanGoForward(Structs.MultiTypes parameter)
    { 
	    var startPosition = transform.position;
        var direction = Dictionaries.MovementXY[_movement.Direction];
        var endPosition = startPosition + (float) _movement.Size * new Vector3(direction.x, direction.y, startPosition.z);

        var hitColliders = Physics2D.OverlapPointAll(endPosition);
       
        return hitColliders.Any(hitCollider => !hitCollider.isTrigger) ? new Structs.MultiTypes { Bool = false} : new Structs.MultiTypes { Bool = true};
    }

}
