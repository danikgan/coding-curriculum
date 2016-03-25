using System.Linq;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MovementEvents : MonoBehaviour
{
    private static GameObject _player ;
    private SceneReferences _referencesScript;
    private Compiler _compiler;
    private ConsolePrinter _console;

    void Start ()
    {
        var mainCamera = GameObject.Find("Main Camera");
        if (mainCamera)
            _referencesScript = mainCamera.GetComponent<SceneReferences>();
        else
            Debug.LogError("Error: Main Camera not found");

        _player = _referencesScript.Player;
        _compiler = _referencesScript.RunStopButton.GetComponent<Compiler>();
        _console = _referencesScript.Console.GetComponent<ConsolePrinter>();
    }

    public void CheckForMovementEvents()
    {
        var hitColliders = Physics2D.OverlapPointAll(_player.transform.position);

        var isAtDestination = hitColliders.Any(hitCollider => hitCollider.gameObject.transform.parent.gameObject.name.Equals("Finish"));
        if (isAtDestination)
        {
           _compiler.StopCode();
            StartCoroutine(SwitchLevel());
            StartCoroutine(WaitForKeyDown(KeyCode.Space));
            return;
        }

        var isInLava = hitColliders.Any(hitCollider => hitCollider.gameObject.transform.parent.gameObject.name.Equals("Lava"));
        if (isInLava)
        {
            _compiler.CompilerStatus = Enumerations.CompilerStatusEnum.Stoped;
            _console.PushError("You have fallen into a lava pool and died!");
        }
    }

	IEnumerator SwitchLevel()
    {
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
