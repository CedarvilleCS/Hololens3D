using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterHider : MonoBehaviour {

    private Vector3 starterScale;

    private void Start()
    {
        starterScale = this.transform.localScale;
    }

    public void Hide()
    {
        this.transform.localScale = new Vector3(0, 0, 0);
    }

    public void Show()
    {
        this.transform.localScale = starterScale;
    }
}
