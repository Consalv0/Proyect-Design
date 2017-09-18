using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventCondition : MonoBehaviour {
	public bool[] conditions = new bool[1];
	public UnityEvent satisfyed;
	public UnityEvent unsatisfyed;

	int selectedCondition = 0;

	public void UpdateBehaviour() {
		for (int i = 0; i < conditions.Length; i++) {
			if (conditions[i] == false) {
				unsatisfyed.Invoke();
				return;
			}
		}
		satisfyed.Invoke();
	}

	public void SelectCondition(int index) {
		selectedCondition = Mathf.Clamp(index, 0, conditions.Length -1);
	}
	public void SetCondition(bool value) {
		SetCondition(selectedCondition, value);
	}
	public void SetCondition(int index, bool value) {
		conditions[index] = value;
		UpdateBehaviour();
	}
	public void SwitchCondition(int index) {
		conditions[index] = !conditions[index];
		UpdateBehaviour();
	}
}
