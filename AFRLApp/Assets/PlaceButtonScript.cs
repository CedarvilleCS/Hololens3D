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
        var script = annotatedImage.GetComponent<Tagalong>();
        script.enabled = !script.enabled;

        var BillboardScript = annotatedImage.GetComponent<Billboard>();
        BillboardScript.enabled = !BillboardScript.enabled;
    }

    public void OnSelectParam(bool CmdToFollow)
    {
        var placeButton = this;
        var annotatedImage = placeButton.transform.parent.gameObject;
        var script = annotatedImage.GetComponent<Tagalong>();

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
