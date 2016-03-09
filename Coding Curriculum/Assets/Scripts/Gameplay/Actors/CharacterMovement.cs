using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public double Size         // Size of a tile
    {
        get { return _sceneReferences.MainCanvasScale.x * 32.0; }
    }

    private double Speed        // Movement speed
    {
        get { return Size*2; }
    }

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
        var direction = new Vector2(movement.x, movement.y);
        _animationManager.SetAnimation(Direction, true);
        StartCoroutine(Movement(transform, direction));
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

    public IEnumerator Movement(Transform playerTransform, Vector2 direction)
    {
        double step = 0;

        var playerRigidBody = GetComponent<Rigidbody2D>();
        var startPosition = playerTransform.position; 
        var endPosition = new Vector2((float) (startPosition.x + direction.x * Size), (float) (startPosition.y + direction.y * Size));

        while (step <= 1.0)
        {
            //gradual movement from start to end
            step += Time.deltaTime*(Speed/Size);
            playerRigidBody.MovePosition(Vector2.Lerp(startPosition, endPosition, (float) step));
            yield
                return null;
        }

        //finished, not moving anymore
        _animationManager.SetAnimation(Direction, false);
        _runCode.PausedExecution_ReadyToRestart = true;
        MovementEvents.CheckForReachedDestination();
        yield return 0;
    }
}