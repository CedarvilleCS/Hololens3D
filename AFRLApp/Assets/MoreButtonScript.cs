using UnityEngine;
using System.Collections;

public class MoreButtonScript : MonoBehaviour {

	// Use this for initialization
	void Start () {

    }
	
	

    public void OnSelect ()
    {
        GameObject ImagePaneCollection = this.transform.parent.gameObject;
        Quaternion ImagePaneRotation = ImagePaneCollection.transform.localRotation;
        Vector3 ImagePanePosition = ImagePaneCollection.transform.localPosition;
        GameObject ImageGallery = ImagePaneCollection.transform.Find("ImageGallery").gameObject;
        Vector3 OldGalleryScale = ImageGallery.GetComponent<ImageGalleryController>().OrigScale;
        int OldInstanceNum = ImageGallery.GetComponent<ImageGalleryController>().InstanceNum;
        Debug.Log("About to Instantiate: OldScale is " + OldGalleryScale);
        
        ImagePanePosition.x = ImagePanePosition.x + 2;
        GameObject NewCollection = (GameObject)Instantiate(ImagePaneCollection, ImagePanePosition, ImagePaneRotation);
        GameObject NewGallery = NewCollection.transform.Find("ImageGallery").gameObject;
        NewGallery.GetComponent<ImageGalleryController>().ResetInstanceNum = OldInstanceNum + 1;
        NewGallery.GetComponent<ImageGalleryController>().ResetScale = OldGalleryScale;
        Debug.Log("Scale is now changed.");

        GameObject OldPlaceButton = ImagePaneCollection.transform.Find("PlaceButton").gameObject;
        OldPlaceButton.GetComponent<PlaceButtonScript>().OnSelectParam(false);
        GameObject NewPlaceButton = NewCollection.transform.Find("PlaceButton").gameObject;
        NewPlaceButton.GetComponent<PlaceButtonScript>().OnSelectParam(true);
    }
}