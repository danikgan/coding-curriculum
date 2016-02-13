using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    private Animator _animation;         //access to animation controller 		

	void Start ()
	{
        _animation = GetComponent<Animator>();
	}

    public void SetAnimation(Enumerations.Directions direction, bool isWalking)
    {
        var currentXYpair = Dictionaries.MovementXY[direction];
        _animation.SetBool("IsWalking", isWalking);
        _animation.SetFloat("Input_x", currentXYpair.x);
        _animation.SetFloat("Input_y", currentXYpair.y);
    }
}

