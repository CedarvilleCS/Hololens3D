using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanoMarkerController : MonoBehaviour {
    public int myIndex;
    public GameObject TakerController;
    public Vector3 starterScale;
	// Use this for initialization
	void Start () {
		starterScale = this.transform.parent.transform.localScale;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnSelect()
    {
        TakerController.GetComponent<PanoTakerController>().TakeSinglePicture(myIndex);
        this.Hide();
    }

    internal void Show()
    {
        this.transform.localScale = starterScale;
    }

    internal void Hide()
    {
        this.transform.localScale = new Vector3(0, 0, 0);
    }
}
