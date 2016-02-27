using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private const float Speed = 64f;        // Movement speed
    private const int Size = 32;            // Size of a tile

	public Enumerations.Directions Direction;     //direction of Player's movement
    private AnimationManager _animationManager;
    private RunCode _runCode;
    private SceneReferences _sceneReferences;
    private Sensors _sensors;

    void Start()
    {
        Direction = Enumerations.Directions.Down;
        _animationManager = GetComponent<AnimationManager>();
        _sceneReferences = GameObject.Find("Main Camera").GetComponent<SceneReferences>();
        _runCode = _sceneReferences.RunButton.GetComponent<RunCode>();
        _sensors = GetComponent<Sensors>();

        Dictionaries.MovementXY = new Dictionary<Enumerations.Directions, Structs.XYpair>
        {
            {Enumerations.Directions.Up, new Structs.XYpair(0, 1)},
            {Enumerations.Directions.Left, new Structs.XYpair(-1, 0)},
            {Enumerations.Directions.Right, new Structs.XYpair(1, 0)},
            {Enumerations.Directions.Down, new Structs.XYpair(0, -1)}
        };
    }

    public Structs.MultiTypes go_forward(Structs.MultiTypes parameter)
    {
        if (!_sensors.CanGoForward(new Structs.MultiTypes()).Bool)
            return new Structs.MultiTypes();

        var movement = Dictionaries.MovementXY[Direction];
        var newVector = new Vector2(movement.x, movement.y);
        _animationManager.SetAnimation(Direction, true);
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
        var currentDirectionInt = (int) Direction;
        var newDirectionInt = (currentDirectionInt + newDirection) % 4;
        if (newDirectionInt < 0)
            newDirectionInt = 3;
        Direction = (Enumerations.Directions) newDirectionInt;
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
        _runCode.PausedExecution_ReadyToRestart = true;
        yield return 0;
    }
}