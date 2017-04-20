using UnityEngine;
using HoloToolkit.Unity;
using System.Collections;

public class PlaceButtonScript : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        var placeButton = this;
        var material = placeButton.GetComponent<Renderer>().material;
        material.color = Color.white;
    }
    void OnSelect ()
    {
        var placeButton = this;
        var material = placeButton.GetComponent<Renderer>().material;
        if (material.color != Color.red)
        {
            material.color = Color.red;
        }
        else
        {
            material.color = Color.white;
        }
        var annotatedImage = placeButton.transform.parent.gameObject;
        var script = annotatedImage.GetComponent<SimpleTagalong>();
        script.enabled = !script.enabled;

        var BillboardScript = annotatedImage.GetComponent<Billboard>();
        BillboardScript.enabled = !BillboardScript.enabled;
    }

    public void OnSelectParam(bool CmdToUnlock)
    {
        var placeButton = this;
        var annotatedImage = placeButton.transform.parent.gameObject;
        var script = annotatedImage.GetComponent<SimpleTagalong>();

        // Only respond to commands that would change current lock state
        if (CmdToUnlock && !script.enabled
            || !CmdToUnlock && script.enabled)
        {
            this.OnSelect();
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
