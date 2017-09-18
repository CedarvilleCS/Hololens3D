using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowHideTrayScript : MonoBehaviour {
    private GameObject TrayAddButton; 


	// Use this for initialization
	void Start () {
        TrayAddButton = GameObject.Find("TrayAddButton");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void showTray()
    {
        TrayAddButton.SetActive(true);
        this.GetComponent<Material>().SetTexture("HideTrayMaterial", (Texture2D)Resources.Load("004-hidetray.png"));
    }

    public void hideTray()
    {
        TrayAddButton.SetActive(false);
        this.GetComponent<Material>().SetTexture("HideTrayMaterial", (Texture2D)Resources.Load("005-showtray.png"));
    }
}
