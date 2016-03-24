using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    private Animator _animation;         //access to animation controller 		

	void Awake()
	{
        _animation = GetComponent<Animator>();
	}

    public void SetAnimation(Enumerations.DirectionsEnum directionEnum, bool isWalking)
    {
        var currentXYpair = Dictionaries.MovementXY[directionEnum];
        _animation.SetBool("IsWalking", isWalking);
        _animation.SetFloat("Input_x", currentXYpair.x);
        _animation.SetFloat("Input_y", currentXYpair.y);
    }
}

