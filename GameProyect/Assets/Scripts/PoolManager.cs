using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Behaviour {
	public static Dictionary<int, PoolHolder> poolHolders = new Dictionary<int, PoolHolder>();
	public static GameObject poolManager = new GameObject("PoolManager");

	/// <summary>
	/// Gets the pool holder of the given prefab.
	/// </summary>
	/// <returns>The pool holder.</returns>
	/// <param name="prefab">Prefab.</param>
	public static PoolHolder GetPoolHolder(GameObject prefab) {
		return GetPoolHolder(prefab.GetInstanceID());
	}
	/// <summary>
	/// Gets the pool holder of the given prefab ID.
	/// </summary>
	/// <returns>The pool holder.</returns>
	/// <param name="prefabID">Prefab identifier.</param>
	public static PoolHolder GetPoolHolder(int prefabID) {
		if (poolHolders.Count == 0) {
			Debug.LogWarning("No 'PoolHolder' instances. Try creating one first.");
			return null;
		} else {
			PoolHolder ph;
			poolHolders.TryGetValue(prefabID, out ph);
			return ph;
		}
	}

	/// <summary>
	/// Adds a PoolHolder to a given GameObject.
	/// </summary>
	/// <returns>The pool holder.</returns>
	/// <param name="prefab">Prefab.</param>
	/// <param name="parent">Parent.</param>
	public static PoolHolder AddPoolHolder(GameObject prefab, GameObject parent) {
		return AddPoolHolder(prefab, parent, 0);
	}
	/// <summary>
	/// Adds a PoolHolder to a given GameObject with a start size.
	/// </summary>
	/// <returns>The pool holder.</returns>
	/// <param name="prefab">Prefab.</param>
	/// <param name="parent">Parent.</param>
	/// <param name="poolSize">Pool size.</param>
	public static PoolHolder AddPoolHolder(GameObject prefab, GameObject parent, int poolSize) {
		int prefabID = prefab.GetInstanceID();
		if (poolHolders.ContainsKey(prefabID)) {
			Debug.Log("Pool already exists");
			return poolHolders[prefabID];
		} else {
			PoolHolder newPoolHolder = parent.AddComponent<PoolHolder>();
			newPoolHolder.SetPoolPrefab(prefab);
			newPoolHolder.Add(poolSize);
			poolHolders.Add(prefabID, newPoolHolder);

			return newPoolHolder;
		}
	}

	/// <summary>
	/// Creates a pool holder.
	/// </summary>
	/// <returns>The pool holder.</returns>
	/// <param name="prefab">Prefab.</param>
	public static PoolHolder CreatePoolHolder(GameObject prefab, bool parenting) {
		return CreatePoolHolder(prefab, 0, parenting);
	}
	/// <summary>
	/// Creates a pool holder with a start size.
	/// </summary>
	/// <returns>The pool holder.</returns>
	/// <param name="prefab">Prefab.</param>
	/// <param name="poolSize">Pool size.</param>
	public static PoolHolder CreatePoolHolder(GameObject prefab, int poolSize, bool parenting) {
		int prefabID = prefab.GetInstanceID();

		if (poolHolders.ContainsKey(prefabID)) {
			Debug.Log("Pool already exists");
			return poolHolders[prefabID];
		} else {
			GameObject newPool = new GameObject(prefab.name + "Pool");
			newPool.transform.parent = poolManager.transform;
			PoolHolder newPoolHolder = newPool.AddComponent<PoolHolder>();
			newPoolHolder.SetPoolPrefab(prefab);
			if (parenting) {
				newPoolHolder.Add(poolSize, newPool.transform);
			} else {
				newPoolHolder.Add(poolSize);
			}
			poolHolders.Add(prefabID, newPoolHolder);

			return newPoolHolder;
		}
	}
}
