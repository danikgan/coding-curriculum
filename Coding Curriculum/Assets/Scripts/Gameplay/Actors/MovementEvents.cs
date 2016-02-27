﻿using UnityEngine;

public class MovementEvents : MonoBehaviour
{
    private GameObject _player ;
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

    public void CheckForPositionEvent()
    {
       //TODO: German, implement here Destination
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Reached destination");
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Reached destination");
    }
}