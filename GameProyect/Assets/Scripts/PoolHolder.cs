using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolHolder : MonoBehaviour {
	public bool parenting = true;
	public int growRate = 10;
	[SerializeField] GameObject poolPrefab;
	[ReadOnly][SerializeField] int poolID;
	 
	[SerializeField] Queue<GameObject> objectsInPool;

	void Start() {
		if (poolPrefab == null) {
			throw new System.NullReferenceException("Pool prefab is missing or null");
		} else {
			poolID = poolPrefab.GetInstanceID();
			objectsInPool = new Queue<GameObject>();
			if (PoolManager.AddPoolHolder(this, poolID) == false) {
				Destroy(this);
			}
		}
	}

	public void SetPoolPrefab(GameObject prefab) {
		if (poolPrefab == null) {
			poolPrefab = prefab;
			poolID = prefab.GetInstanceID();
			objectsInPool = new Queue<GameObject>();
		} else {
			Debug.LogWarning("Pool prefab is already setted");
		}
	}

	public void Add(int value) {
		Add(value, transform);
	}
	public void Add(int value, Transform parenting) {
		for (int i = 0; i < value; i++) {
			GameObject poolObject = Instantiate(poolPrefab);
			poolObject.name = poolObject.GetInstanceID() + "@" + name;
			poolObject.AddComponent<PoolObject>().SetPoolHolder(this);
			if (parenting && this.parenting) { 
				poolObject.transform.parent = parenting;
			}

			poolObject.SetActive(false);
			objectsInPool.Enqueue(poolObject);
		}
	}

	public void Recycle(GameObject gameObject) {
		gameObject.SetActive(false);
		objectsInPool.Enqueue(gameObject);
	}
	
	public GameObject PickObject() {
		GameObject pickedObject = TakeObject();
		if (!pickedObject) throw new System.Exception("PoolHolder is empty try adding objects first!");
		pickedObject.SetActive(false);
		objectsInPool.Enqueue(pickedObject);
		pickedObject.SetActive(true);
		return pickedObject;
	}

	public GameObject ForceTakeObject() {
		if (objectsInPool.Count == 0) {
			Add(growRate);
			return ForceTakeObject();
		} else {
			GameObject takedObject = objectsInPool.Dequeue();
			takedObject.SetActive(true);
			return takedObject;
		}
	}

	public GameObject TakeObject() {
		if (objectsInPool.Count == 0) {
			return null;
		} else {
			GameObject takedObject = objectsInPool.Dequeue();
			takedObject.SetActive(true);
			return takedObject;
		}
	}
	public GameObject[] TakeObjects(int value) {
		List<GameObject> takenObjects = new List<GameObject>();
		if (objectsInPool.Count == 0) {
			return null;
		} else {
			for (int i = 0; i < value; i++) {
				GameObject takedObject = TakeObject();
				if (!takedObject) break;
				takenObjects.Add(takedObject);
			}
			return takenObjects.ToArray();
		}
	}

	void OnDestroy() {
		PoolManager.RemovePoolHolder(poolID);
	}
}
