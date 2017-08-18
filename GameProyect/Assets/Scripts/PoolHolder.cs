using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolHolder : MonoBehaviour {
	[SerializeField] GameObject poolPrefab;
	public GameObject prefab {
		get {
			return poolPrefab;
		}
	}
	[ReadOnly][SerializeField] int poolID;
	public int prefabID {
		get {
			return poolID;
		}
	}
	public bool parenting = true;
	public bool stateSwicher = true;
	public int growRate = 10;
	public int awakeSize = 0;

	[SerializeField] Queue<GameObject> objectsInPool;

	void Awake() {
		if (poolPrefab == null) {
			throw new System.NullReferenceException("Pool prefab is missing or null");
		} else {
			poolID = poolPrefab.GetInstanceID();
			objectsInPool = new Queue<GameObject>();
			if (PoolManager.AddExistingPoolHolder(this).GetInstanceID() == GetInstanceID()) {
				Add(awakeSize);
			} else {
				poolPrefab = null;
				poolID = 0;
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
			if (poolObject.GetComponent<PoolObject>() == null) {
				poolObject.AddComponent<PoolObject>().SetPoolHolder(this);
			} else {
				poolObject.GetComponent<PoolObject>().SetPoolHolder(this);
			}
			if (parenting && this.parenting) { 
				poolObject.transform.parent = parenting;
			}
			if (stateSwicher) poolObject.SetActive(false);
			objectsInPool.Enqueue(poolObject);
		}
	}

	public void Recycle(GameObject gameObject) {
		if (stateSwicher) gameObject.SetActive(false);
		gameObject.SendMessage("OnRecycle", SendMessageOptions.DontRequireReceiver);
		objectsInPool.Enqueue(gameObject);
	}

	public GameObject PickObject() {
		GameObject pickedObject = TakeObject();
		if (!pickedObject) throw new System.Exception("PoolHolder is empty try adding objects first!");
		if (stateSwicher) pickedObject.SetActive(false);
		pickedObject.SendMessage("OnRecycle", SendMessageOptions.DontRequireReceiver);
		objectsInPool.Enqueue(pickedObject);
		if (stateSwicher) pickedObject.SetActive(true);
		return pickedObject;
	}

	public GameObject ForceTakeObject() {
		if (objectsInPool.Count == 0) {
			Add(growRate);
			return ForceTakeObject();
		} else {
			GameObject takedObject = objectsInPool.Dequeue();
			if (stateSwicher) takedObject.SetActive(true);
			return takedObject;
		}
	}

	public GameObject TakeObject() {
		if (objectsInPool.Count == 0) {
			Debug.LogWarning("Pool is empty, try using ForceTakeObject() instead", gameObject);
			return null;
		} else {
			GameObject takedObject = objectsInPool.Dequeue();
			if (stateSwicher) takedObject.SetActive(true);
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
		if (poolID != 0) {
			PoolManager.RemovePoolHolder(poolID);
		}
	}
}
