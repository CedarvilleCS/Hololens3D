using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopOutController : MonoBehaviour {
    public TabSelector ts;	
	void OnSelect()
    {
        ts.MakePopout();
    }
}
