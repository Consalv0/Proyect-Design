using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFontain : MonoBehaviour {
	public GameObject prefab;
	public Vector3 dir;
	public float spawnTime;

	void Start() {
		StartCoroutine(Fontain());
	}

	void OnDrawGizmos() {
		Gizmos.DrawLine(transform.position, dir + transform.position);	
	}

	IEnumerator Fontain() {
		while (true) {
			if (Input.GetAxis("Horizontal") <= 0) {
				var prefabPicked = PoolManager.FindPoolHolder(prefab).PickObject();
				var prefabRB = prefabPicked.GetComponent<Rigidbody>();
				prefabPicked.transform.position = transform.position;
				prefabRB.velocity = Vector3.zero;
				prefabRB.AddForce(dir, ForceMode.Impulse);
			} else {
				var prefabInstace = Instantiate(prefab);
				var prefabRB = prefabInstace.GetComponent<Rigidbody>();
				prefabInstace.transform.position = transform.position;
				prefabRB.velocity = Vector3.zero;
				prefabRB.AddForce(dir, ForceMode.Impulse);
			}
			yield return new WaitForSeconds(spawnTime + 0.00001f);
		}
	}
}
