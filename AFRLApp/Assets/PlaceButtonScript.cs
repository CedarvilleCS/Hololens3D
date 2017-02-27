using UnityEngine;
using HoloToolkit.Unity;
using System.Collections;

public class PlaceButtonScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    void OnSelect ()
    {

        var placeButton = this.gameObject;
        var annotatedImage = placeButton.transform.parent.gameObject;
        var script = annotatedImage.GetComponent<SimpleTagalong>();
        script.enabled = !script.enabled;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
