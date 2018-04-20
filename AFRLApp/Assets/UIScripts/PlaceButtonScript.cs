using UnityEngine;
using HoloToolkit.Unity;
using System.Collections;
using System;

public class PlaceButtonScript : MonoBehaviour
{

    private bool locked;
    private Material material;
    public GameObject parentPane;
    // Use this for initialization
    void Start()
    {
        var placeButton = this;
        material = placeButton.GetComponent<Renderer>().material;
        material.color = Color.white;
        locked = false;
    }

    internal void OnSelect()
    {
        locked = !locked;
        var placeButton = this;


        var SimpleTagalongScript = parentPane.GetComponent<SimpleTagalong>();

        var BillboardScript = parentPane.GetComponent<Billboard>();

        if (locked)
        {
            material.color = Color.red;
            SimpleTagalongScript.enabled = false;
            BillboardScript.enabled = false;
        }
        else
        {
            material.color = Color.white;
            SimpleTagalongScript.enabled = true;
            BillboardScript.enabled = true;
        }
    }

    public void OnSelectParam(bool CmdToUnlock)
    {
        var script = parentPane.GetComponent<SimpleTagalong>();

        // Only respond to commands that would change current lock state
        if (CmdToUnlock && !locked
            || !CmdToUnlock && locked)
        {
            this.OnSelect();
        }
    }

    internal void InitMaterial()
    {
        material = this.GetComponent<Renderer>().material;
    }
}
