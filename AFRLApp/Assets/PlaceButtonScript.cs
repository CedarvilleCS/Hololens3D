using UnityEngine;
using HoloToolkit.Unity;
using System.Collections;

public class PlaceButtonScript : MonoBehaviour {

	// Use this for initialization
	void Start () {

    }

    void OnSelect ()
    {

        var placeButton = this;
        var annotatedImage = placeButton.transform.parent.gameObject;
        var script = annotatedImage.GetComponent<SimpleTagalong>();
        script.enabled = !script.enabled;
        if (!script.enabled)
        {
            this.GetComponent<Renderer>().material.color = Color.red;
        }
        else
        {
            this.GetComponent<Renderer>().material.color = Color.white;
        }

        var BillboardScript = annotatedImage.GetComponent<Billboard>();
        BillboardScript.enabled = !BillboardScript.enabled;
    }

    public void OnSelectParam(bool CmdToFollow)
    {
        var placeButton = this;
        var annotatedImage = placeButton.transform.parent.gameObject;
        var script = annotatedImage.GetComponent<SimpleTagalong>();

        // Only respond to commands that would change current follow state
        if (CmdToFollow && !script.enabled
            || !CmdToFollow && script.enabled)
        {
            this.OnSelect();
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
