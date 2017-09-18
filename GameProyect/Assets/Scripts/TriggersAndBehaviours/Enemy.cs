using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Enemy : MonoBehaviour {
	public MainHut target;
	public Vector3 offsetTarget;
	public float vel = 10;
	public float move;

	void Start() {
		move = vel * Time.deltaTime;
		target = FindObjectOfType<MainHut>();
	}

	void Update() {
		if (target != null) {
			transform.LookAt(target.transform.position);
			transform.position = Vector3.MoveTowards(transform.position, target.transform.position + offsetTarget, move);
		}
	}
}
