using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObject : MonoBehaviour {
	[ReadOnly][SerializeField] PoolHolder poolHolder;

	public void SetPoolHolder(PoolHolder pool) {
		if (poolHolder == null) {
			poolHolder = pool;
		} else {
			Debug.LogWarning("PoolHolder is already setted.");
		}
	}
	
	public void Recycle() {
		SendMessage("OnRecycle");
		poolHolder.Recycle(gameObject);
	}
}
