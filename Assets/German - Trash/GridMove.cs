using System.Collections;
using UnityEngine;

class GridMove : MonoBehaviour {
	private float moveSpeed = 70f;
	private float gridSize = 32f;
	private enum Orientation {
		Horizontal,
		Vertical
	};
	private Vector2 input;
	private bool isMoving = false;
	private Vector3 startPosition;
	private Vector3 endPosition;
	private float t;


	public void Update() {


		if (!isMoving) 
		{
			input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

		if (Mathf.Abs(input.x) > Mathf.Abs(input.y)) 
			{
			input.y = 0;
			} 
			else 
			{
			input.x = 0;
			}

			if (input != Vector2.zero) 
			{
				Debug.Log (input.x);
				Debug.Log (input.y);
			StartCoroutine(move(transform));
			}
		}
	}

	public IEnumerator move(Transform transform) {
		isMoving = true;
		startPosition = transform.position;
		t = 0;
	
		var rigidBody = GetComponent<Rigidbody2D>();
		Debug.Log (System.Math.Sign(input.x));
		Debug.Log (System.Math.Sign(input.y));
		endPosition = new Vector3(startPosition.x + System.Math.Sign(input.x) * gridSize,
			startPosition.y + System.Math.Sign(input.y) * gridSize, startPosition.z);

		while (t < 1f) {
			t += Time.deltaTime * (moveSpeed/gridSize);
			rigidBody.MovePosition (rigidBody.position + (Vector2.Lerp(startPosition, endPosition, t) - rigidBody.position));

			yield
			return null;
		}

		isMoving = false;
		yield return 0;
	}
}