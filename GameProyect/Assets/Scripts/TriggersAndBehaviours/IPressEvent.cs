using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPressEvent : ITrigger {
	void Press();
	void Release();
}
