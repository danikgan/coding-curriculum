using System.Linq;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MovementEvents : MonoBehaviour
{
    private static GameObject _player ;
    private SceneReferences _referencesScript;
	static public MovementEvents instance;

	void Awake(){ //called when an instance awakes in the game
		instance = this; //set our static reference to our newly initialized instance
	}

    void Start ()
    {
        var mainCamera = GameObject.Find("Main Camera");
        if (mainCamera)
            _referencesScript = mainCamera.GetComponent<SceneReferences>();
        else
            Debug.LogError("Error: Main Camera not found");

        _player = _referencesScript.Player;
    }

    public static void CheckForReachedDestination()
    {
        var hitColliders = Physics2D.OverlapPointAll(_player.transform.position);
        var isAtDestination = hitColliders.Any(hitCollider => hitCollider.gameObject.name.Equals("Finish"));

		if (isAtDestination) {

			instance.StartCoroutine (instance.SwitchLevel());
			instance.StartCoroutine(instance.WaitForKeyDown(KeyCode.Space));
		}
    }

	IEnumerator SwitchLevel() {


		var mainCamera = GameObject.Find("Main Camera");
		var fadeTime = mainCamera.GetComponent<FadeOut>().BeginFadeOut(); //begin fading out
		yield return new WaitForSeconds(fadeTime);

	}

	IEnumerator WaitForKeyDown(KeyCode keyCode)
	{
		while (!Input.GetKeyDown(keyCode))
			yield return null;
		instance.LevelTransition ();
	}

	void LevelTransition (){
		Debug.Log ("transition");
		int index = SceneManager.GetActiveScene ().buildIndex;

		Debug.Log (SceneManager.sceneCountInBuildSettings + " scenes ");
		Debug.Log (index);

		if (SceneManager.sceneCountInBuildSettings > index + 1) {
			Debug.Log ("transition1");
			SceneManager.LoadScene (index + 1);
		} else {
			Debug.Log ("transition2");
			SceneManager.LoadScene("Level1");
		}
	}

}
