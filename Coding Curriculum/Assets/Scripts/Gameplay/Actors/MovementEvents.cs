using System.Linq;
using UnityEngine;

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

    public static void CheckForReachedDestination()
    {
        var hitColliders = Physics2D.OverlapPointAll(_player.transform.position);
        var isAtDestination = hitColliders.Any(hitCollider => hitCollider.gameObject.name.Equals("Finish"));
        if(isAtDestination)
            Debug.Log("Player got to destination");
    }

}
