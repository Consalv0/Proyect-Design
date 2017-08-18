using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObject : MonoBehaviour {
	[ReadOnly][SerializeField] PoolHolder poolHolder;
	public PoolHolder holder {
		get {
			return poolHolder;
		}
	}

	public void SetPoolHolder(PoolHolder pool) {
		if (poolHolder == null) {
			poolHolder = pool;
		} else {
			Debug.LogWarning("PoolHolder is already setted.", gameObject);
		}
	}

	public virtual void OnRecycle() {
	}
	
	public void Recycle() {
		poolHolder.Recycle(gameObject);
	}
}
