﻿using UnityEngine;
using HoloToolkit.Unity;
using System.Collections;

public class PlaceButtonScript : MonoBehaviour {

    private bool locked;
    private Material material;
    // Use this for initialization
    void Start()
    {
        var placeButton = this;
        material = placeButton.GetComponent<Renderer>().material;
        material.color = Color.white;
        locked = false;
        OnSelect();
    }

    void OnSelect ()
    {
        locked = !locked;
        var placeButton = this;
        if (locked)
        {
            material.color = Color.red;
        }
        else
        {
            material.color = Color.white;
        }
        var parentPane = placeButton.transform.parent.gameObject;
        var SimpleTagalongScript = parentPane.GetComponent<SimpleTagalong>();
        SimpleTagalongScript.enabled = !SimpleTagalongScript.enabled;

        var BillboardScript = parentPane.GetComponent<Billboard>();
        BillboardScript.enabled = !BillboardScript.enabled;
    }

    public void OnSelectParam(bool CmdToUnlock)
    {
        var parentPane = this.transform.parent.gameObject;
        var script = parentPane.GetComponent<SimpleTagalong>();

        // Only respond to commands that would change current lock state
        if (CmdToUnlock && !script.enabled
            || !CmdToUnlock && script.enabled)
        {
            this.OnSelect();
        }
    }
	
}
