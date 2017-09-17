using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateArround : MonoBehaviour {
	public string rotateInput = "Fire2";
	[Tooltip("0 is hypersensibility")]
	public float sensibility = 100;
	public float distance = 20;
	public float altitude = 5;
	public float angle = 0;
	public float angleSum = 90;
	[Range(0, 10)]
	public float speed = 1;
	public Transform target;

	Vector2 startMousePos;
	Vector2 dirMouse;

	Vector3 angleDir = new Vector3();
	Vector3 targetDir = new Vector3();

	void Update () {
		if (Input.GetButtonDown(rotateInput)) {
			startMousePos = Input.mousePosition;
		}
		if (Input.GetButton(rotateInput)) {
			dirMouse = Input.mousePosition;
			dirMouse = startMousePos - dirMouse;
			if (dirMouse.x > sensibility) {
				startMousePos = Input.mousePosition;
				angle += angleSum;
				angle = angle > 360 ? angle % 360 : angle;
			}
			if (dirMouse.x < -sensibility) {
				startMousePos = Input.mousePosition;
				angle -= angleSum;
				angle = angle < 0 ? angle % 360 : angle;
			}
		}

		targetDir = Vector3.Lerp(targetDir, angleDir, Time.deltaTime * speed);

		transform.position = target.position;
		angleDir.x = Mathf.Cos(Mathf.Deg2Rad * angle);
		angleDir.y = altitude;
		angleDir.z = Mathf.Sin(Mathf.Deg2Rad * angle);
		angleDir.Normalize();
		transform.position += targetDir * distance;
		transform.LookAt(target.position);
	}
}
