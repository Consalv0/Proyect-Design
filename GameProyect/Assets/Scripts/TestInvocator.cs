using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInvocator : MonoBehaviour {
    public GameObject ObjectToPutPool;
    void Awake() {
        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        obj.name = "CubePrimitive";
        if (ObjectToPutPool) PoolManager.AddPoolHolder(obj, ObjectToPutPool, 10);
        PoolManager.CreatePoolHolder(obj);
        PoolManager.GetPoolHolder(obj);
    }
}
