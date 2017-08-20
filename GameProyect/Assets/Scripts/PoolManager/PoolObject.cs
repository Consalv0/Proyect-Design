using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObject : MonoBehaviour {
	[ReadOnly] [SerializeField] PoolHolder poolHolder;
	public PoolHolder holder {
		get {
			return poolHolder;
		}
	}

	/// <summary>
	/// Sets the pool holder.
	/// </summary>
	/// <param name="pool">Pool.</param>
	public void SetPoolHolder(PoolHolder pool) {
		if (poolHolder == null) {
			if (pool) {
				poolHolder = pool;
			}
		} else {
			Debug.LogWarning("PoolHolder is already setted. Ignoring...", gameObject);
		}
	}

	/// <summary>
	/// Recycle this instance.
	/// </summary>
	public void Recycle() {
		poolHolder.Recycle(gameObject);
	}

	void OnRecycle() { }
	void OnReuse() { }

#if UNITY_EDITOR
	bool appQuiting = false;
	void OnApplicationQuit() {
		appQuiting = true;
	}
	void OnDestroy() {
		if (!appQuiting && poolHolder) {
			Debug.LogWarning("You have destroyed a PoolObject, you must be using Recycle() instead", gameObject);
		}
	}
#endif
}
