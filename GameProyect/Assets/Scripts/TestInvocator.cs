using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInvocator : MonoBehaviour {
	public GameObject ObjectToPutPool;
	public GameObject prefab;

	void Awake() {
		GameObject obj = prefab;
		if (ObjectToPutPool) PoolManager.AddPoolHolder(obj, ObjectToPutPool, 10);
		PoolManager.CreatePoolHolder(obj, true);
		var tobj = PoolManager.GetPoolHolder(obj).ForceTakeObject();
		tobj.SetActive(true);
	}
}
