using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainHut : MonoBehaviour {
	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.GetComponent<Enemy>()) {
			Destroy(gameObject);
		}
	}
}
