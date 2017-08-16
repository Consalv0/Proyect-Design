using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour, IPoolObject {
	[ReadOnly] int poolID;
	public int PoolID {
		get {
			return poolID;
		}
		set {
			poolID = value;
		}
	}

	public void Recycle() {
		Debug.Log(PoolID);
		PoolHolder ph = PoolManager.GetPoolHolder(PoolID);
		if (ph) ph.Recycle(gameObject);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
