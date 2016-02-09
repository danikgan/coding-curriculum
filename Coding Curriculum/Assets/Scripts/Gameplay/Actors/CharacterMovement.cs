using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private const float Speed = 64f;        // Movement speed
    private const int Size = 32;            // Size of a tile

	public Enumerations.Directions Direction;     //direction of Player's movement
    private AnimationManager _animationManager;

    void Start()
    {
        _animationManager = GetComponent<AnimationManager>();
        Direction = Enumerations.Directions.Down;

        Dictionaries.MovementXY = new Dictionary<Enumerations.Directions, Structs.XYpair>
        {
            {Enumerations.Directions.Up, new Structs.XYpair(0, 1)},
            {Enumerations.Directions.Left, new Structs.XYpair(-1, 0)},
            {Enumerations.Directions.Right, new Structs.XYpair(1, 0)},
            {Enumerations.Directions.Down, new Structs.XYpair(0, -1)}
        };
    }

    /*public void Update() {

		if (Input.GetKeyDown (KeyCode.R)) // turn right on R pressed
			turnRight();
		if (Input.GetKeyDown (KeyCode.L)) //turn left on L pressed
			turnLeft();
		if (Input.GetKeyDown (KeyCode.F)) // go forward by one tile on F pressed
			goForward();
	}*/

    public Structs.MultiTypes go_forward(Structs.MultiTypes parameter)
    {
        var movement = Dictionaries.MovementXY[Direction];
        var newVector = new Vector2(movement.x, movement.y);
        _animationManager.SetAnimation(Direction, true);



        //when a new coordinate is entered into the newVector, second one has to be erased
        /*      if (Mathf.Abs(newVector.x) > Mathf.Abs(newVector.y))
              {
                  newVector.y = 0;
              }
              else
              {
                  newVector.x = 0;
              }*/

        StartCoroutine(Movement(transform, newVector));

        Debug.Log("go forward");

        return new Structs.MultiTypes();
    }

    public Structs.MultiTypes turn_right(Structs.MultiTypes parameter)
    {
        ChangeDirection(+1);
        Debug.Log("turn right");
        return new Structs.MultiTypes();
    }

    public Structs.MultiTypes turn_left(Structs.MultiTypes parameter)
    {
        ChangeDirection(-1);
        Debug.Log("turn_left");
        return new Structs.MultiTypes();
    }

    private void ChangeDirection(int newDirection)
    {
        var currentDirectionInt = Convert.ToInt32(Enum.GetName(typeof(Enumerations.Directions), Direction));
        Direction = (Enumerations.Directions)((currentDirectionInt - newDirection) % 4);
        _animationManager.SetAnimation(Direction, false);
    }

    public IEnumerator Movement(Transform transform, Vector2 newVector) //TODO refactor this
    {
        //performs actual movement

        var startPosition = transform.position; //current position
        float i = 0;

        var rigidBody = GetComponent<Rigidbody2D>(); //access to rigid body component of Player

        var endPosition = new Vector2(startPosition.x + newVector.x*Size, startPosition.y + newVector.y*Size);
        //where Player will be moved to

        while (i < 1f)
        {
            //gradual movement from start to end
            i += Time.deltaTime*(Speed/Size);
            rigidBody.MovePosition(Vector2.Lerp(startPosition, endPosition, i));
            yield
                return null;
        }

        //finished, not moving anymore
        _animationManager.SetAnimation(Direction, false);
        yield return 0;
    }
}