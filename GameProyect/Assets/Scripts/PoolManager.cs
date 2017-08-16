using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Behaviour {
    public static Dictionary<int, PoolHolder> poolHolders = new Dictionary<int, PoolHolder>();
    public static GameObject poolManager = new GameObject("PoolManager");

    public static PoolHolder GetPoolHolder(GameObject prefab) {
        return GetPoolHolder(prefab.GetInstanceID());
    }
    public static PoolHolder GetPoolHolder(int prefabID) {
        if (poolHolders.Count == 0) {
            Debug.LogWarning("No 'PoolHolder' instances. Try creating one first.");
            return null;
        } else {
            return poolHolders[prefabID];
        }
    }

    public static PoolHolder AddPoolHolder(GameObject prefab, GameObject parent) {
        return AddPoolHolder(prefab, parent, 0);
    }
    public static PoolHolder AddPoolHolder(GameObject prefab, GameObject parent, int poolSizes) {
        int prefabID = prefab.GetInstanceID();
        if (poolHolders.ContainsKey(prefabID)) {
            return poolHolders[prefabID];
        } else {
            GameObject newPool = new GameObject(prefab.name + "Pool");
            newPool.transform.parent = parent.transform;
            PoolHolder newPoolHolder = newPool.AddComponent<PoolHolder>();
            poolHolders.Add(prefabID, newPoolHolder);

            for (int i = 0; i < poolSizes; i++) {
                GameObject poolObject = Instantiate(prefab);
                poolObject.name = poolObject.GetInstanceID() + "@" + newPoolHolder.name;
                poolObject.transform.parent = newPoolHolder.transform;
            }
            return newPoolHolder;
        }
    }

    public static PoolHolder CreatePoolHolder(GameObject prefab) {
        return CreatePoolHolder(prefab, 10);
    }
    public static PoolHolder CreatePoolHolder(GameObject prefab, int poolSize) {
        int prefabID = prefab.GetInstanceID();

        if (poolHolders.ContainsKey(prefabID)) {
            return poolHolders[prefabID];
        } else {
            GameObject newPool = new GameObject(prefab.name + "Pool");
            newPool.transform.parent = poolManager.transform;
            PoolHolder newPoolHolder = newPool.AddComponent<PoolHolder>();
            poolHolders.Add(prefabID, newPoolHolder);

            for (int i = 0; i < poolSize; i++) {
                GameObject poolObject = Instantiate(prefab);
                poolObject.name = poolObject.GetInstanceID() + "@" + newPoolHolder.name;
                poolObject.transform.parent = newPoolHolder.transform;
            }
            return newPoolHolder;
        }
    }
}
