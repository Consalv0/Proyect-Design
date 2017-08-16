using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolObject {
	int PoolID { get; set; }
	void Recycle();
}
