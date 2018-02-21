using HoloToolkit.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanoPopupController : MonoBehaviour
{

    public Text alertText;
    private Vector3 starterScale;

    // Use this for initialization
    void Start()
    {
        //this.Hide();
        starterScale = transform.localScale;
    }

    internal void Show()
    {
        this.transform.localScale = starterScale;
        this.GetComponent<Billboard>().enabled = true;
        this.GetComponent<SimpleTagalong>().enabled = true;
    }

    internal void Hide()
    {
        this.GetComponent<Billboard>().enabled = false;
        this.GetComponent<SimpleTagalong>().enabled = false;
        this.transform.localScale = new Vector3(0, 0, 0);
    }

    internal void OnPanoRequestReceived(string ip)
    {
        alertText.text = "The system at IP address " + ip + " wants to take a panorama";
        this.Show();
    }
}
