using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolHolder : MonoBehaviour {
	[ReadOnly][SerializeField] GameObject poolPrefab;
	[ReadOnly][SerializeField] int poolID;

	[SerializeField] Queue<GameObject> objectsInPool;

	//PoolHolder(GameObject prefab, int startSize) {
	//	poolPrefab = prefab;
	//	poolID = prefab.GetInstanceID();
	//	poolObjects = new Queue<GameObject>();
	//	Add(startSize);
	//}

	public void SetPoolPrefab(GameObject prefab) {
		poolPrefab = prefab;
		poolID = prefab.GetInstanceID();
		objectsInPool = new Queue<GameObject>();
	}

	public void Add(int value) {
		Add(value, null);
	}
	public void Add(int value, Transform parenting) {
		for (int i = 0; i < value; i++) {
			GameObject poolObject = Instantiate(poolPrefab);
			poolObject.name = poolObject.GetInstanceID() + "@" + name;
			poolObject.GetComponent<IPoolObject>().PoolID = poolID;
			if (parenting) {
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

	public GameObject ForceTakeObject() {
		return ForceTakeObject(10, transform);
	}
	public GameObject ForceTakeObject(int jumpSize) {
		return ForceTakeObject(jumpSize, transform);
	}
	public GameObject ForceTakeObject(int jumpSize, Transform parenting) {
		if (objectsInPool.Count == 0) {
			Add(jumpSize, parenting);
			return ForceTakeObject(jumpSize, parenting);
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
		GameObject[] takenObjects = new GameObject[value];
		if (objectsInPool.Count == 0) {
			return null;
		} else {
			for (int i = 0; i < value; i++) {
				GameObject takedObject = TakeObject();
				if (!takedObject) break;
				takenObjects[i] = takedObject;
			}
			return takenObjects;
		}
	}
}
