using System;
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
        var isObstacle = CheckForObstacleAtPosition(endPosition);
        return new Structs.MultiTypes{ Bool = !isObstacle};
    }

    public Structs.MultiTypes NotCanGoForward(Structs.MultiTypes parameter)
    {
        var result = CanGoForward(new Structs.MultiTypes());
        result.Bool = !result.Bool;
        return result;
    }

    public Structs.MultiTypes NotReachedTarget(Structs.MultiTypes parameter)
    {
        var hitColliders = Physics2D.OverlapPointAll(transform.position);
        var isAtDestination = hitColliders.Any(hitCollider => hitCollider.gameObject.transform.parent.gameObject.name.Equals("Finish"));
        return new Structs.MultiTypes {Bool = !isAtDestination};
    }

    public Structs.MultiTypes NoObstaclesAround(Structs.MultiTypes parameter)
    {
        var startPosition = transform.position;
        var isObstacle =
            Dictionaries.MovementXY.Select(
                direction =>
                    startPosition +
                    (float) _movement.Size*new Vector3(direction.Value.x, direction.Value.y, startPosition.z))
                .Aggregate(false, (current, endPosition) => current || CheckForObstacleAtPosition(endPosition));

        return new Structs.MultiTypes {Bool = !isObstacle};
    }

    private bool CheckForObstacleAtPosition(Vector3 targetPosition)
    {
        var hitColliders = Physics2D.OverlapPointAll(targetPosition);
        return hitColliders.Any(hitCollider => !hitCollider.isTrigger);
    }
}
