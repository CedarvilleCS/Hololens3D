using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskCheckController : MonoBehaviour {
    public bool boxChecked;
    public Material CheckedMat;
    public Material UncheckedMat;
	// Use this for initialization
	void Start () {
        boxChecked = false;
        CheckedMat = Resources.Load("Materials/CheckedBox") as Material;
        UncheckedMat = Resources.Load("Materials/UncheckedBox") as Material;
	}
	
	// Update is called once per frame
	void OnSelect()
    {
        boxChecked = !boxChecked;
        this.transform.parent.GetComponent<TaskController>().Checked(boxChecked);
        if (boxChecked)
        {
            this.GetComponent<Renderer>().material = CheckedMat;
        } else
        {
            this.GetComponent<Renderer>().material = UncheckedMat;
        }
    }
}
