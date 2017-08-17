using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInvocator : MonoBehaviour {
	public GameObject prefab;

	void Start() {
		PoolHolder poolHolder = PoolManager.FindPoolHolder(prefab);
		poolHolder.Add(100);
	}
}
