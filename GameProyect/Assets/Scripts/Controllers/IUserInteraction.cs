using UnityEngine;

public interface IUserInteraction {
	bool canInteract { get; }
	void Interact(Vector3 interactPosition, params GameObject[] objects);
	Transform transform { get; }
	GameObject gameObject { get; }
}
