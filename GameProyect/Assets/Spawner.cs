using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
	public GameObject target;
	public GameObject prefab;
	public Vector2 randomRadius = Vector2.one * 2;
	public float radomTime;

	void Awake() {
		StartCoroutine(Spawn());
	}

	IEnumerator Spawn() {
		while (target) {
			yield return new WaitForSeconds(Random.Range(1, radomTime));
			Instantiate(prefab,
			            transform.position + new Vector3(Random.Range(-randomRadius.x, randomRadius.x), 0, Random.Range(-randomRadius.y, randomRadius.y)),
			            Quaternion.identity);
		}
	}
}
