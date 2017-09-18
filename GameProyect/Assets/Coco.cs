using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coco : MonoBehaviour {
	void OnTriggerEnter(Collider other) {
		if (other.GetComponent<Enemy>()) {
			Destroy(other.gameObject);
			Destroy(gameObject);
		}
	}
}
