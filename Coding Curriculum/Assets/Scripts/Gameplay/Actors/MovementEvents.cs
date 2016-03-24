using System.Linq;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MovementEvents : MonoBehaviour
{
    private static GameObject _player ;
    private SceneReferences _referencesScript;

    void Start ()
    {
        var mainCamera = GameObject.Find("Main Camera");
        if (mainCamera)
            _referencesScript = mainCamera.GetComponent<SceneReferences>();
        else
            Debug.LogError("Error: Main Camera not found");

        _player = _referencesScript.Player;
    }

    public void CheckForReachedDestination()
    {
        var hitColliders = Physics2D.OverlapPointAll(_player.transform.position);
        var isAtDestination = hitColliders.Any(hitCollider => hitCollider.gameObject.name.Equals("Finish"));

		if (isAtDestination) {

			StartCoroutine(SwitchLevel());
			StartCoroutine(WaitForKeyDown(KeyCode.Space));
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
		LevelTransition ();
	}

    static void LevelTransition()
    {
        Debug.Log("transition");
        int index = SceneManager.GetActiveScene().buildIndex; //get current level index

        Debug.Log(index);

		int total_num_scenes = SceneManager.sceneCountInBuildSettings;

		if (total_num_scenes > index + 1) //if the current level is not last
        {
            Debug.Log("transition1");
            SceneManager.LoadScene(index + 1);
        }
        else //if the current level is the last - revert to level1
        {
            Debug.Log("transition2");
            SceneManager.LoadScene("Level1");
        }
    }

}
