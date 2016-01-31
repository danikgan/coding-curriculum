using UnityEngine;
using System.Collections;

public class NewMove : MonoBehaviour {

	private bool isMoving;
	private Vector3 startPosition;
	private Vector3 endPosition;

	void Start () {

		isMoving = false; 


	}
	
	// Update is called once per frame
	void Update () {

		startPosition = transform.position;


		if(Input.GetKeyDown(KeyCode.UpArrow) && !isMoving)
			{
			isMoving = true;
			endPosition = new Vector3(startPosition.x, startPosition.y + 1.02f, startPosition.z);
			transform.position = Vector3.Lerp (startPosition, endPosition, 1);
			}
		if(Input.GetKeyUp(KeyCode.UpArrow) && isMoving)
		{
			isMoving = false;
		}


	
	}
}
