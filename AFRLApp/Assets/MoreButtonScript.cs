using UnityEngine;
using System.Collections;

public class MoreButtonScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	

    void OnSelect ()
    {
        GameObject newViewer = (GameObject)Instantiate(Resources.Load("ImageViewer"));
    }
}
