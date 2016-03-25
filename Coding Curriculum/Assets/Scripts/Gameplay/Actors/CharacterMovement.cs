using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public const double StandardTileSize = 32;

    public double Size         // Size of a tile
    {
        get { return _sceneReferences.MapScale.x * StandardTileSize; }
    }

    private double Speed        // Movement speed
    {
        get { return Size * 2; }
    }

    [HideInInspector] public Enumerations.DirectionsEnum Direction;     //direction of Player's movement

    public Vector2 StartingPosition; // used for initialisation
    public Enumerations.DirectionsEnum StartingDirection;
    
    private AnimationManager _animationManager;
    private Compiler _compiler;
    private SceneReferences _sceneReferences;
    private Sensors _sensors;
    private MovementEvents _movementEvents;

    void Awake()
    {
        _animationManager = GetComponent<AnimationManager>();
        _sceneReferences = GameObject.Find("Main Camera").GetComponent<SceneReferences>();
        _sensors = GetComponent<Sensors>();
        _movementEvents = GetComponent<MovementEvents>();
        _compiler = _sceneReferences.RunStopButton.GetComponent<Compiler>();

        Dictionaries.MovementXY = new Dictionary<Enumerations.DirectionsEnum, Structs.XYpair>
        {
            {Enumerations.DirectionsEnum.Up, new Structs.XYpair(0, 1)},
            {Enumerations.DirectionsEnum.Left, new Structs.XYpair(-1, 0)},
            {Enumerations.DirectionsEnum.Right, new Structs.XYpair(1, 0)},
            {Enumerations.DirectionsEnum.Down, new Structs.XYpair(0, -1)}
        };
    }

    void Start()
    {
        GoToStartingPosition();
    }

    public void GoToStartingPosition()
    {
        ChangeDirection(StartingDirection);
        SetImmediatePosition(StartingPosition);
    }

    private void SetImmediatePosition(Vector2 newPosition)
    {
        var deltaPosition = new Vector3((float)((newPosition.x - 0.5) * Size), - (float)((newPosition.y - 0.5) * Size), 0);
        transform.position = transform.parent.position + deltaPosition;
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
        ChangeDirection((Enumerations.DirectionsEnum)newDirectionInt);
    }

    private void ChangeDirection(Enumerations.DirectionsEnum newDirectionEnum)
    {
        Direction = newDirectionEnum;
        _animationManager.SetAnimation(Direction, false);
    }

    public IEnumerator Movement(Transform playerTransform, Vector2 direction)
    {
        double step = 0;

        var startPosition = playerTransform.position;
        var endPosition = new Vector2((float) (startPosition.x + direction.x*Size),
            (float) (startPosition.y + direction.y*Size));

        while (step <= 1.0 && _compiler.CompilerStatus == Enumerations.CompilerStatusEnum.Runing)
        {
            //gradual movement from start to end
            step += Time.deltaTime*(Speed/Size);
            transform.position = Vector2.Lerp(startPosition, endPosition, (float) step);
            yield
                return null;
        }

        //finished, not moving anymore
        _animationManager.SetAnimation(Direction, false);
        _compiler.PausedExecution_ReadyToRestart = true;
        _movementEvents.CheckForMovementEvents();
        yield return 0;
    }
}