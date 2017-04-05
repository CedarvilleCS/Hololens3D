using UnityEngine;
using System.Collections;

public class CloseButtonScript : MonoBehaviour {
    public Vector3 OrigScale;
    public Vector3 ResetScale;

    // Use this for initialization
    void Start () {
        OrigScale = this.transform.localScale;
        GameObject ImagePaneCollection = this.transform.parent.gameObject;
        bool isFirstWindow = ImagePaneCollection.GetComponent<ImageReceiver>().FirstInstance;
        Debug.Log("isFirstWindow is " + isFirstWindow);
        Debug.Log("ResetScale is " + ResetScale);
        if (isFirstWindow)
        {
            this.transform.localScale = new Vector3(0, 0, 0);
        }
        else
        {
            OrigScale = ResetScale;
            this.transform.localScale = OrigScale;
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnSelect ()
    {
        Destroy(this.transform.root.gameObject);
    }
}
