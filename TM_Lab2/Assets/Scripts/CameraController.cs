using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	private Camera cam;

	void Awake () {
		cam = GetComponent <Camera> ();
	}

	public void MoveToCenter (int size) {
		Vector3 newPos = transform.position;
		float center = (size - 1) / 2f;
		newPos.x = center;
		newPos.y = center;
		transform.position = newPos;
		cam.orthographicSize = size / 2f + 1f;
	}
}
