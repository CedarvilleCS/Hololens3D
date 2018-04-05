using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hider : MonoBehaviour {

    //ThingToHide can also be the current object
    public GameObject thingToHide;
    private Vector3 starterScale;
    private bool hidden;
    private Material minimizeMat;
    private Material maximizeMat;

    // Use this for initialization
    void Start()
    {
        hidden = false;
        starterScale = thingToHide.transform.localScale;
        minimizeMat = Resources.Load("Images/Materials/Minimize", typeof(Material)) as Material;
        maximizeMat = Resources.Load("Images/Materials/Maximize", typeof(Material)) as Material;
    }

    void OnSelect()
    {
        hidden = !hidden;
        if (hidden)
        {
            Show();
            this.GetComponent<Renderer>().material = minimizeMat;
        }
        else
        {
            Hide();
            this.GetComponent<Renderer>().material = maximizeMat;
        }
    }

    internal void Hide()
    {
        thingToHide.transform.localScale = new Vector3(0, 0, 0);
    }

    internal void Show()
    {
        thingToHide.transform.localScale = starterScale;
    }
}
