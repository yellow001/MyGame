using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {
    TTT t=TTT.asdwqe;
	// Use this for initialization
	void Start () {
        Debug.Log(t.GetType()+"  "+t.ToString());
	}
}

enum TTT {
    asdb,
    asdwqe
}
