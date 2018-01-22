using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskCheckController : MonoBehaviour {
    public bool boxChecked;
	// Use this for initialization
	void Start () {
        boxChecked = false;
	}
	
	// Update is called once per frame
	void OnSelect()
    {
        boxChecked = !boxChecked;
        this.transform.parent.GetComponent<TaskController>().Checked(boxChecked);
        
    }
}
