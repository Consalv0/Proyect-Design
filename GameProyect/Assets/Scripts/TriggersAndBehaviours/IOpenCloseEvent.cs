using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IOpenCloseEvent : ITrigger {
	void Open();
	void Close();
}
