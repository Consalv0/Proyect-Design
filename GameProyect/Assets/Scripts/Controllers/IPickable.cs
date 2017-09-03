using UnityEngine;

public interface IPickable : IUserInteraction {
	void Pick(Vector3 position);
	void Drop();
}
